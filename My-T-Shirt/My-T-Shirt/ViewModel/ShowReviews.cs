using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_T_Shirt.ViewModel
{
    public class ShowReviews
    {
        public string IdReview { get; set; }
        public string IdUtilisateur { get; set; }
        public string IdProduit { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Rating { get; set; }
        public string Commentaire { get; set; }
        public DateTime dateReview { get; set; }
    }
}