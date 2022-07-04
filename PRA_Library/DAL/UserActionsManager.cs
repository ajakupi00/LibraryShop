using Microsoft.ApplicationBlocks.Data;
using RWA_Library.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace RWA_Library.DAL
{
    public class UserActionsManager : IPurchase, ILoan
    {
        private string CS;
        public BookManager bm;
        public UserActionsManager(string connectionString)
        {
            CS = connectionString;
            bm = new BookManager(connectionString);
        }

        public void DeleteLoan(int loanid)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(DeleteLoan), loanid);
        }

        public IEnumerable<Loan> GetAllLoan(User user)
        {
            var tblLoan = SqlHelper.ExecuteDataset(CS, nameof(GetAllLoan), user.IDUser).Tables[0];
            if (tblLoan == null) return null;
            IList<Loan> result = new List<Loan>();
            foreach (DataRow row in tblLoan.Rows)
            {
                result.Add(new Loan
                {
                    IDLoan = (int)row[nameof(Loan.IDLoan)],
                    Book = bm.Get((int)row[nameof(Book.IDBook)]),
                    User = user,
                    LoanBeginDate = (DateTime)row[nameof(Loan.LoanBeginDate)],
                    LoanEndDate = (DateTime)row[nameof(Loan.LoanEndDate)],
                    TotalDelayAmount = (decimal)row[nameof(Loan.TotalDelayAmount)],
                    DelayPrice = (decimal)row[nameof(Loan.DelayPrice)],
                    IsSettled = (bool)row[nameof(Loan.IsSettled)]
                });
            }
            return result;

        }

        public IEnumerable<Purchase> GetAllPurchase(User user)
        {
            var tblPurchase = SqlHelper.ExecuteDataset(CS, nameof(GetAllPurchase), user.IDUser).Tables[0];
            if (tblPurchase == null) return null;
            IList<Purchase> result = new List<Purchase>();
            foreach (DataRow row in tblPurchase.Rows)
            {
                result.Add(new Purchase
                {
                    IDPurchase = (int)row[nameof(Purchase.IDPurchase)],
                    Book = bm.Get((int)row[nameof(Purchase.Book.IDBook)]),
                    User = user,
                    PurchaseDate = (DateTime)row[nameof(Purchase.PurchaseDate)]
                });
            }
            return result;
        }

        public void LoanBook(Loan loan)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(LoanBook),
                loan.User.IDUser, loan.Book.Title, loan.Book.LoanPrice,
                loan.LoanBeginDate, loan.LoanEndDate);
        }

        public void PurchaseBook(Purchase purchase)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(PurchaseBook),
                purchase.User.IDUser, purchase.Book.IDBook);
        }

        public void RefreshLoan(int userid)
        {
            SqlHelper.ExecuteNonQuery(CS, nameof(RefreshLoan), userid);
        }


        public IEnumerable<Loan> GettOngoingLoans()
        {
            var tblLoan = SqlHelper.ExecuteDataset(CS, nameof(GettOngoingLoans)).Tables[0];
            if (tblLoan == null) return null;
            IList<Loan> result = new List<Loan>();
            UserManager um = new UserManager(CS);
            foreach (DataRow row in tblLoan.Rows)
            {
                result.Add(new Loan
                {
                    IDLoan = (int)row[nameof(Loan.IDLoan)],
                    Book = bm.Get((int)row[nameof(Book.IDBook)]),
                    User = um.Get((int)row[nameof(User.IDUser)]),
                    LoanBeginDate = (DateTime)row[nameof(Loan.LoanBeginDate)],
                    LoanEndDate = (DateTime)row[nameof(Loan.LoanEndDate)],
                    TotalDelayAmount = (decimal)row[nameof(Loan.TotalDelayAmount)],
                    DelayPrice = (decimal)row[nameof(Loan.DelayPrice)]
                });
            }
            return result;

        }
    }
}
