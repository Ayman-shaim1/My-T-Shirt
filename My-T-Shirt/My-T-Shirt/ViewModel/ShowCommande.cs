using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_T_Shirt.ViewModel
{
    public class ShowCommande
    {
        public string IdCommande { get; set; }
        public string idUtilisateur { get; set; }
        public string Adresse_Com { get; set; }
        public string Ville_Com { get; set; }
        public string CodePostal_Com { get; set; }
        public string Pays_Com { get; set; }
        public DateTime Date_Com { get; set; }
        public bool isDelivered { get; set; }
        public Nullable<System.DateTime> Date_Livraison { get; set; }
        public List<ProduitPanier> produits { get; set; }

        public ShowCommande()
        {
            this.produits = new List<ProduitPanier>();
        }
    }
}