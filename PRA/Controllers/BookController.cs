

using PRA.Models;
using RWA_Library.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace PRA.Controllers
{
    public class BookController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        private BookManager bm;
        private UserActionsManager uam;
        public BookController()
        {
            bm = new BookManager(connectionString);
            uam = new UserActionsManager(connectionString);
        }

        public ActionResult Index(string title, string message)
        {
            Book book = bm.GetByTitle(title);
            Author author = book.Author;
            ViewBag.msg = message;
            return View(book);
        }
        [HttpGet]
        public ActionResult Loan(string title)
        {
            if ((Session["user"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            Book book = bm.GetByTitle(title);
            if (book == null) throw new Exception("Book with that title does not exits!");
            return View(book);
        }
        [HttpPost]
        public ActionResult Loan(string title, DateTime pocetak, DateTime kraj)
        {
            IEnumerable<Loan> loans = uam.GetAllLoan((User)Session["user"]).Where(l => !l.IsSettled);
            if (loans.Count() < 3)
                return RedirectToAction("Transaction", new { placanje = "loan", title = title, pocetak = pocetak, kraj = kraj });
            else
            {
                return RedirectToAction("Index", new { title = title, message = "You have passed the maximum amount of loans!" });
            }
        }

        [HttpGet]
        public ActionResult Purchase(string title)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account", null);
            }
            Book book = bm.GetByTitle(title);
            if (book == null) throw new Exception("Book with that ID not found");
            return View(book);
        }
        [HttpPost]
        public ActionResult Purchase(string placanje, string title)
        {
            if (placanje == "stripe")
            {
                //NAPRAVI TRANSAKCIJU
                return RedirectToAction("Index", "Stripe", new { book = title });
            }
            else
            {
                return RedirectToAction("Transaction", new { placanje = placanje, title = title, pocetak = DateTime.Now, kraj = DateTime.Now });
            }
        }
        public void SendMail2Step(string SMTPServer, int SMTP_Port, string From, string Password, string To, string Subject, string Body, string[] FileNames)
        {
            var smtpClient = new SmtpClient(SMTPServer, SMTP_Port)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true
            };
            smtpClient.Credentials = new NetworkCredential(From, Password); //Use the new password, generated from google!

            var message = new MailMessage
            {
                From = new MailAddress(From, "Library d.o.o"),
                Subject = Subject,
                Body = Body,
            };
            message.To.Add(new MailAddress(To, To));
            smtpClient.Send(message);
        }


        [HttpGet]
        public ActionResult Transaction(string placanje, string title, DateTime pocetak, DateTime kraj)
        {
            Book book = bm.GetByTitle(title);
            User user = (User)Session["user"];
            if (placanje == "loan")
            {
                Loan loan = new Loan
                {
                    Book = book,
                    User = user,
                    LoanBeginDate = pocetak,
                    LoanEndDate = kraj
                };
                uam.LoanBook(loan);
                user.Loans.Add(loan);
            }
            else
            {
                uam.PurchaseBook(new Purchase
                {
                    Book = book,
                    User = user,
                });
            }
            SendMail2Step("smtp.gmail.com", 587, "aapps333@gmail.com",
           "tkpuatpsqjeoypjo",
           "aapps333@gmail.com", "Library: Transaction confirmation", $"Dear,\nThank you for {((placanje == "loan") ? "lending" : "buying")} our book: " +
           $"{book.Title} by {book.Author.FullName}."
           + $"{((placanje == "loan") ? $"\nYou have to return your book by {kraj.ToShortDateString()}" : "")}"
           + $"\nRegards.", null);

            ViewBag.placanje = placanje;
            ViewBag.book = book;
            return View();
        }
        [HttpGet]
        public ActionResult Edit(string title)
        {

            User user = (User)Session["user"];
            if (title is null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!user.IsAdmin || user == null || Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Book book = bm.GetByTitle(title);
            ViewBag.authors = bm.am.GetAll();
            Status @new = bm.GetStatus(1);
            Status @used = bm.GetStatus(2);
            IList<SelectListItem> statuses = new List<SelectListItem> {
            new SelectListItem { Text = @new.StatusName, Value = @new.IdStatus.ToString(), Selected = book.Status.IdStatus == @new.IdStatus},
            new SelectListItem { Text = @used.StatusName, Value = @used.IdStatus.ToString(), Selected = book.Status.IdStatus == @used.IdStatus},
            };
            ViewBag.bm = bm;
            ViewBag.s = statuses;
            List<SelectListItem> ddlauthors = new List<SelectListItem>();
            IList<Author> authors = ViewBag.authors as IList<Author>;
            authors.ToList().ForEach(a => ddlauthors.Add(new SelectListItem { Text = a.FullName, Value = a.IDAuthor.ToString(), Selected = (book.Author.IDAuthor == a.IDAuthor) }));
            ViewBag.ddlauthors = ddlauthors;
            ViewBag.id = book.IDBook;
            return View(BookModel.Parse(book));
        }

        [HttpPost]
        public ActionResult Edit(BookModel book, int bookid, string status, string author)
        {
            Book oldbook = bm.Get(bookid);
            if (ModelState.IsValid)
            {
                Book newbook = BookModel.ParseFromBookModel(book, bookid, bm.am.Get(int.Parse(author)), bm.GetStatus(int.Parse(status)));
                if (status == "2")
                    bm.UpdateToUsed(newbook);
                else
                    bm.Update(newbook);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.authors = bm.am.GetAll();
            Status @new = bm.GetStatus(1);
            Status @used = bm.GetStatus(2);
            IList<SelectListItem> statuses = new List<SelectListItem> {
            new SelectListItem { Text = @new.StatusName, Value = @new.IdStatus.ToString(), Selected = oldbook.Status.IdStatus == @new.IdStatus},
            new SelectListItem { Text = @used.StatusName, Value = @used.IdStatus.ToString(), Selected = oldbook.Status.IdStatus == @used.IdStatus},
            };
            ViewBag.bm = bm;
            ViewBag.s = statuses;
            return View(BookModel.Parse(oldbook));
        }

        [HttpGet]
        public ActionResult New()
        {
            //User user = (User)Session["user"];
            //if (!user.IsAdmin || user == null || Session["user"] == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            ViewBag.authors = bm.am.GetAll();
            Status @new = bm.GetStatus(1);
            Status @used = bm.GetStatus(2);
            IList<SelectListItem> statuses = new List<SelectListItem> {
            new SelectListItem { Text = @new.StatusName, Value = @new.IdStatus.ToString(), Selected = true},
            new SelectListItem { Text = @used.StatusName, Value = @used.IdStatus.ToString()},
            };
            ViewBag.s = statuses;
            List<SelectListItem> ddlauthors = new List<SelectListItem>();
            bm.am.GetAll().ToList().ForEach(a => ddlauthors.Add(new SelectListItem { Text = a.FullName, Value = a.IDAuthor.ToString() }));
            ViewBag.ddlauthors = ddlauthors;
            return View();
        }

        [HttpPost]
        public ActionResult New(BookModel mbook, string status, string author)
        {
            if (ModelState.IsValid && status != null && author != null)
            {
                Book book = BookModel.ParseFromBookModel(mbook, bm.am.Get(int.Parse(author)), bm.GetStatus(int.Parse(status)));
                bm.Create(book);
                ViewBag.added = $"{book.Title} was succesfully added!";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.authors = bm.am.GetAll();
            Status @new = bm.GetStatus(1);
            Status @used = bm.GetStatus(2);
            IList<SelectListItem> statuses = new List<SelectListItem> {
            new SelectListItem { Text = @new.StatusName, Value = @new.IdStatus.ToString(), Selected = true},
            new SelectListItem { Text = @used.StatusName, Value = @used.IdStatus.ToString()},
            };
            ViewBag.s = statuses;
            List<SelectListItem> ddlauthors = new List<SelectListItem>();
            bm.am.GetAll().ToList().ForEach(a => ddlauthors.Add(new SelectListItem { Text = a.FullName, Value = a.IDAuthor.ToString() }));
            ViewBag.ddlauthors = ddlauthors;
            return View();
        }

    }
}

