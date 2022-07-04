using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Loan
    {
        public int IDLoan { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        [DataType(DataType.Date)]
        public DateTime LoanBeginDate { get; set; }
        public DateTime LoanEndDate { get; set; }
        public decimal TotalDelayAmount { get; set; }
        public decimal DelayPrice { get; set; }
        public bool IsSettled { get; set; }
    }
}