using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RWA_Library.DAL
{
    public static class ConnectionString
    {
        public static string GetConnectionString() => ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
    }
}
