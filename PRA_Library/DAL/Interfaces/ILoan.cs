using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace RWA_Library.DAL.Interfaces
{
    public interface ILoan
    {
        void LoanBook(Loan loan);
        void DeleteLoan(int loanid);
        void RefreshLoan(int userid);
        IEnumerable<Loan> GetAllLoan(User user);
    }
}
