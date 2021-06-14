using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace My_T_Shirt.Models
{
    public class Review
    {
        public string IdReview { get; set; }
        public string IdUtilisateur { get; set; }
        public string IdProduit { get; set; }
        public int Rating { get; set; }
        public string Commentaire { get; set; }
        public DateTime dateReview { get; set; }

        public Review()
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdReview = myuuidAsString;
            dateReview = DateTime.Now;

        }

        public Review(string IdReview, string IdUtilisateur, string IdProduit, int Rating, string Commentaire)
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdReview = myuuidAsString;
            dateReview = DateTime.Now;

            this.IdUtilisateur = IdUtilisateur;
            this.IdProduit = IdProduit;
            this.Rating = Rating;
            this.Commentaire = Commentaire;
        }

        public bool Ajouter()
        {
            int nbr = 0;
            string req1 = string.Format("select count(*) from Review where idProduit='{0}' and idUtilisateur='{1}' ", IdProduit, IdUtilisateur);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr == 0)
            {
                string req2 = string.Format("insert into Review  values('{0}','{1}','{2}',{3},'{4}','{5}')", IdReview, IdUtilisateur, IdProduit, Rating, Commentaire,dateReview);
                SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                d.cn.Open();
                cmd2.ExecuteNonQuery();
                d.cn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool inReview(string idProduit, string idUtilisateur)
        {

            int nbr = 0;
            string req1 = string.Format("select count(*) from Review where idProduit='{0}' and idUtilisateur='{1}' ", idProduit, idUtilisateur);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static int getRating(string idProduit)
        {
            int nbr = 0;
            string req = string.Format(@"select top 1 Rating from Produit p inner join Review r on p.idProduit = r.idProduit
                                            where p.idProduit = '{0}'
                                            group by Rating
                                            order by COUNT(*) desc",idProduit);
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            if (cmd.ExecuteScalar() != null) {
                nbr = int.Parse(cmd.ExecuteScalar().ToString());
            }
            d.cn.Close();
            return nbr;
        }

        
    }
  }