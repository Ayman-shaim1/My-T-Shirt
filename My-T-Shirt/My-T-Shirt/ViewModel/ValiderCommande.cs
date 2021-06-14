using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_T_Shirt.ViewModel
{
    public class ValiderCommande
    {
        public AdresseCommande adresse { get; set; }
        public List<ProduitPanier> produits { get; set; }
    }
}