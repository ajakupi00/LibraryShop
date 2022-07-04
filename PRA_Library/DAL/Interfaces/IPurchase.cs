using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace RWA_Library.DAL.Interfaces
{
    public interface IPurchase
    {
        void PurchaseBook(Purchase purchase);
        IEnumerable<Purchase> GetAllPurchase(User user);
    }
}
