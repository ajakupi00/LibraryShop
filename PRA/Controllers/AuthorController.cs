using RWA_Library.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace PRA.Controllers
{
    public class AuthorController : Controller
    {
        private AuthorManager am = new AuthorManager(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        private BookManager bm = new BookManager(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        // GET: Author
        public ActionResult Index(int id)
        {
            IEnumerable<Book> otherBooks = bm.GetAll().Where(b => b.Author.IDAuthor == id);
            List<Book> books = new List<Book>();
            if(otherBooks.Count() > 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    books.Add(otherBooks.ElementAt(i));
                }
            }
            
            ViewBag.books = books;
            return View(am.Get(id));
        }

        [HttpGet]
        public ActionResult New()
        {

            User user = (User)Session["user"];
            if (user != null && user.IsAdmin)
            {
                return View();

            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult New(Author author)
        {
            if (ModelState.IsValid)
            {
                am.Create(author);
                ViewBag.added = $"{author.FullName} has been succesfully added!";
                return RedirectToAction("Index", "Home");
            }
            return View(author);
        }

    }
}