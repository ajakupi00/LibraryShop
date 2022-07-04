using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace RWA_Library.DAL.Interfaces
{
    public interface IUserManager : IRepo<User>
    {
        void CreateEmployee(User user, string password);

        void ConfirmEmail(int userid);
        void ConfirmPassword(int userid);
    }
}
