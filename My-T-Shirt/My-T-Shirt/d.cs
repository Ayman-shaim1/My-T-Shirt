using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace My_T_Shirt
{
    public class d
    {
        public static SqlConnection cn = new SqlConnection("server=.;database=My-t-shirt;integrated security=true");
    }
}