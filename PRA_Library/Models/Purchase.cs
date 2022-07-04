using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Purchase
    {
        public int IDPurchase { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        public DateTime PurchaseDate { get; set; }

    }
}