using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_T_Shirt.Models
{
    public class ligneCommande
    {
        public string IdCommande { get; set; }
        public string IdProduit { get; set; }
        public int qte { get; set; }
        public string taille_Choisie { get; set; }
        
    }
}