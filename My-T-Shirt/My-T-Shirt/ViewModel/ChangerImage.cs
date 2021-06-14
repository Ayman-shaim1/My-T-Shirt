using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_T_Shirt.ViewModel
{
    public class ChangerImage
    {
        public string idProduit { get; set; }
        public HttpPostedFile upload { get; set; }
}
}