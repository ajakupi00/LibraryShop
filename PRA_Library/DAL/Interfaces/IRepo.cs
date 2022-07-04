using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWA_Library.DAL
{
    public interface IRepo<T> where T : class
    {
        void Create(T model, params object[] args);
        void Update(T model, params object[] args);
        void Delete(T model);
        T Get(int id);
        IEnumerable<T> GetAll();
    }
}
