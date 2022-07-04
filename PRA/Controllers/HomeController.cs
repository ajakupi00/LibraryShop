using RWA_Library.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Collections;
using RWA_Library.Models;

namespace PRA.Controllers
{
    public class HomeController : Controller
    {

        private string connectionString = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        private BookManager bm;
        public HomeController()
        {
            bm = new BookManager(connectionString);
        }
        public ActionResult Index(string search)
        {
            ViewBag.user = Session["user"];
            
            IList<Book> lbooks = bm.GetAll().ToList();
            SortedSet<Book> books = new SortedSet<Book>();
            lbooks.ToList().ForEach(b => books.Add(b));
            if (search != null)
            {
                return View(books.Where(b => b.Title.Contains(search) || b.Author.FullName.Contains(search)).ToList());
            }
            return View(books);
        }
        [HttpPost]
        public ActionResult Index(string search, string filter, string price)
        {

            ViewBag.user = Session["user"];
            if (price != null && price == READING_FILTER.MOST.ToString())
            {
                return View(bm.GetAllBooksByReading(READING_FILTER.MOST));
            }
            else if(price != null && price == READING_FILTER.LEAST.ToString())
            {
                return View(bm.GetAllBooksByReading(READING_FILTER.LEAST));
            }
            IEnumerable<Book> lbooks = bm.GetAll().ToList();
            switch (filter)
            {
                case "za":
                    lbooks.ToList().OrderBy(x => x.Title);
                    lbooks = lbooks.Reverse();
                    if (search != null)
                    {
                        return View(lbooks.Where(b => b.Title.Contains(search) || b.Author.FullName.Contains(search)).ToList());
                    }
                    return View(lbooks);
                default:
                    SortedSet<Book> sbooks = new SortedSet<Book>();
                    lbooks.ToList().ForEach(b => sbooks.Add(b));

                    if (search != null)
                    {
                        return View(sbooks.Where(b => b.Title.Contains(search) || b.Author.FullName.Contains(search)).ToList());
                    }
                    return View(sbooks);
            }

        }



    }
}