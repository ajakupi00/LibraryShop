using RWA_Library.DAL;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace PRA.Controllers
{
    public class StripeController : Controller
    {
        private BookManager bm = new BookManager(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        public string clientSecret;
        [HttpGet]
        public ActionResult Index(string book)
        {
            if (Session["user"] == null) throw new Exception("Can not directly acces this page!");
            Book bookModel = bm.GetByTitle(book);
            var options = new PaymentIntentCreateOptions
            {
                Amount = 1000,
                Currency = "usd",
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);
            clientSecret = paymentIntent.ClientSecret;

            ViewBag.clientSecret = clientSecret;
           
            ViewBag.book = bookModel;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string called, string title)
        {
            if (title != "")
                return RedirectToAction("Transaction", "Book", new { placanje = "stripe", title = title, pocetak = DateTime.Now, kraj = DateTime.Now });
            else
                return View();
        }

    }
}