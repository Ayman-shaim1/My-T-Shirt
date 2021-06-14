using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_T_Shirt.ViewModel
{
    public class ProduitPanier
    {
        public string idProduit { get; set; }
        public string idTaille { get; set; }
        public string Image { get; set; }
        public string Nom { get; set; }
        public int qte { get; set; }
        public float prix { get; set; }
        public int qteStock { get; set; }
    }
}