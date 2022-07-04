using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace RWA_Library.DAL.Interfaces
{
    public interface IBookManager : IRepo<Book>
    {
        void RemoveBook(int bookid);
    }
}
