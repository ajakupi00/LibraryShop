using PRA.Models;
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
    public class AdminController : Controller
    {
        private UserActionsManager uam = new UserActionsManager(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        private UserManager um = new UserManager(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        
        public ActionResult New()
        {
            return View();
        }
        

        [HttpPost]
        public ActionResult New(AdminModel model)
        {
            if (ModelState.IsValid)
            {
                string password = model.ConfirmPassword;
                User user = AdminModel.ParseToUser(model);
                um.CreateEmployee(user, password);
                return RedirectToAction("Index", "Account");

            }
            return View();  
        }

        [HttpGet]
        public ActionResult Loans()
        {
            User user = (User)Session["user"];
            if (user is null || !user.IsAdmin)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.bm = uam.bm;
            IEnumerable<Loan> loans = uam.GettOngoingLoans();
            return View(loans);
        }

        [HttpPost]
        public ActionResult Return(string id)
        {
            IEnumerable<Loan> loans = uam.GettOngoingLoans();
            loans.ToList().ForEach(l => uam.RefreshLoan(l.User.IDUser));
            uam.DeleteLoan(int.Parse(id));
            return RedirectToAction("Loans",uam.GettOngoingLoans());
        }

        [HttpPost]
        public ActionResult RefreshLoans()
        {
            IEnumerable<Loan> loans = uam.GettOngoingLoans();
            loans.ToList().ForEach(l => uam.RefreshLoan(l.User.IDUser));
            loans = uam.GettOngoingLoans();
            return RedirectToAction("Loans", loans);
        }

    }
}