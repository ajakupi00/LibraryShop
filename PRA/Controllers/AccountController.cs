using PRA.Models;
using RWA_Library.DAL;
using rwaLib.Models;
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
    public class AccountController : Controller
    {
        private UserManager um = new UserManager(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterUserModel ruser)
        {
            if (ModelState.IsValid)
            {

                um.Create(new User
                {
                    FirstName = ruser.FirstName,
                    LastName = ruser.LastName,
                    Email = ruser.Email,
                    DateOfBirth = ruser.DateOfBirth,
                    CityName = ruser.CityName,
                    ZipCode = int.Parse(ruser.ZipCode),
                    Address = ruser.Address
                }, ruser.Password);
                User user = um.GetAll().FirstOrDefault(u => u.Email == ruser.Email);
                SendMail2Step("smtp.gmail.com", 587, "aapps333@gmail.com",
        "tkpuatpsqjeoypjo",
        "aapps333@gmail.com", "Library: Confirm your email", "Dear,\nThis is your confirmation link: " + Url.Action("ConfirmEmail", new { allow = user.IDUser }), null);
                return RedirectToAction("Login", "Account", new { emailSent = "true" });
            }
            return View("Register");
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


        [HttpPost]
        public ActionResult Update(string firstname, string lastname, string address, string cityname, string zipcode, DateTime dateofbirth)
        {
            User user = (User)Session["user"];
            user.FirstName = firstname;
            user.LastName = lastname;
            user.Address = address;
            user.CityName = cityname;
            user.ZipCode = int.Parse(zipcode);
            user.DateOfBirth = dateofbirth;
            string password = um.GetUserPassword(user.IDUser);
            um.Update(user, password);
            return View("Index");
        }
        [HttpGet]
        public ActionResult Login(string email, string password, string emailSent)
        {
            ViewBag.emailSent = emailSent;
            if (email != null && password != null)
            {
                User user = um.AuthUser(email, password);
                if (user == null)
                {
                    ViewBag.msg = "User with that password and email does not exist!";
                    return View("Login");
                }
                if (um.CheckEmail(user) || user.IsAdmin)
                {
                    Session["user"] = user;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    SendMail2Step("smtp.gmail.com", 587, "aapps333@gmail.com",
            "tkpuatpsqjeoypjo",
            "aapps333@gmail.com", "Library: Confirm your email", "Dear,\nThis is your confirmation link: " + Url.Action("ConfirmEmail", new { allow = user.IDUser }), null);
                    ViewBag.msg = "Please confirm your e-mail, so you can log-in into your account!";
                    return View("Login");
                }

            }

            return View("Login");
        }

        [HttpPost]
        public ActionResult UpdatePassword(string password, string confirmPassword)
        {
            User user = (User)Session["user"];//noviPassword123!
            bool t1 = password.Any(char.IsUpper);
            bool t2 = password.Any(char.IsDigit);
            bool t3 = password.Any(char.IsSymbol);
            if (password == confirmPassword && password.Length >= 8 && password.Any(char.IsUpper) && password.Any(char.IsDigit) && password.Any(char.IsSymbol) )
            {
                //CHANGE
                um.ChangePassword(user, Cryptography.HashPassword(password));
                ViewBag.msg = "Password succesfully updated!";
                return View("Index", user);
            }
            ViewBag.msg = "Password don't match or password has to be longer than 7 charachter, has to contain minimum 1 uppercase letter, 1 lowercase letter, 1 number and 1 symbol";
            return View("Index", user);
        }
        [HttpGet]
        public ActionResult Index()
        {
            User user = (User)Session["user"];
            if (user == null) return View("Login");
            if (!user.IsAdmin)
                ViewBag.loans = user.Loans.Count.ToString();
            return View(user);
        }

        [HttpPost]
        public ActionResult DeleteAcc(int id)
        {
            User user = new User() { IDUser = id };
            um.Delete(user);
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult PasswordRecovery()
        {
            return View();
        }



        [HttpPost]
        public ActionResult PasswordRecovery(string email)
        {
            User user = um.GetUserByEmail(email);
            if (user == null)
            {
                ViewBag.msg = "User with that email doesn not exist!";
                return View();
            }
            SendMail2Step("smtp.gmail.com", 587, "aapps333@gmail.com",
            "tkpuatpsqjeoypjo",
            "aapps333@gmail.com", "Library: Recover password", Url.Action("NewPassword", new { allow = email }), null);
            ViewBag.msg = "Check your email!";



            return View("Login");
        }



        [HttpGet]
        public ActionResult NewPassword(string allow)
        {
            if (allow == null)
            {
                return RedirectToAction("Login");
            }
            User user = um.GetUserByEmail(allow);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.id = user.IDUser;
            ViewBag.email = allow;
            return View();
        }



        [HttpPost]
        public ActionResult NewPassword(int id, string allow, PasswordModel passwords)
        {
            if (ModelState.IsValid && passwords.Password != null && passwords.ConfirmPassword != null)
            {
                um.ChangePassword(um.Get(id), passwords.ConfirmPassword);
                ViewBag.msg = "Password succesfully changed!";
                return View("Login");
            }
            User user = um.GetUserByEmail(allow);
            ViewBag.email = allow;
            ViewBag.id = user.IDUser;
            return View();
        }

        [HttpGet]
        public ActionResult Loans()
        {
            User user = (User)Session["user"];
            if (user is null) return RedirectToAction("Login");
            IEnumerable<Loan> loans = (IEnumerable<Loan>)um.uam.GetAllLoan(user);
            return View(loans);
        }

        [HttpGet]
        public ActionResult Purchases()
        {
            User user = (User)Session["user"];
            if (user is null) return RedirectToAction("Login");
            IEnumerable<Purchase> purchases = (IEnumerable<Purchase>)um.uam.GetAllPurchase(user);
            return View(purchases);
        }

        public ActionResult LogOut()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmEmail(string allow)
        {
            if (allow == null) throw new Exception("Can't access this page directly!");
            um.ConfirmEmail(int.Parse(allow));
            ViewBag.msg = "E-mail succesfully confirmed! Now, please log-in again!";
            return View("Login");

        }
    }
}