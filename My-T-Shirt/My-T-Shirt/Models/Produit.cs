using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace My_T_Shirt.Models
{
    public class Produit
    {
        public string IdProduit { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public float Prix { get; set; }
        public string Image { get; set; }
        public string idMarque {get;set;}
        
        public Produit()
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdProduit = myuuidAsString;

        }

        public Produit(string Nom, string Description,  string Genre, float Prix, string Image,  string idMarque)
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdProduit = myuuidAsString;

            this.Nom = Nom;
            this.Description = Description;
            
            this.Genre = Genre;
            this.Prix = Prix;
            this.Image = Image;
            this.idMarque = idMarque;
        }

        public void Ajouter()
        {

            string req = string.Format("insert into Produit values('{0}','{1}','{2}','{3}',{4},'{5}','{6}')", IdProduit, Nom, Description,  Genre, Prix, Image, idMarque);
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            cmd.ExecuteNonQuery();
            d.cn.Close();

        } 

        public static List<Produit> Affcher(string recherche = "" ,string genre = "" )
        {
            bool bl_recherche = false;
            bool bl_genre = false;
            List<Produit> lstprod = new List<Produit>();
            string req = "select * from produit ";

            if (recherche != null && recherche != "")
            {
                bl_recherche = true;
            }

           if(genre != null && genre != "")
            {
                bl_genre = true;
            }
            

            if(bl_recherche && !bl_genre)
            {
                

                req += string.Format("where idMarque = '{0}' or nom Like'{1}%' ", Marque.getIdMarque(recherche), recherche);
            }

            if(!bl_recherche && bl_genre)
            {
                req += string.Format("where genre='{0}' ", genre);
            }

            if(bl_genre && bl_recherche)
            {
                req += string.Format("where genre='{0}' and idMarque '{1}' or nom Like'{2}%' ", genre, Marque.getIdMarque(recherche), recherche);
            }

            SqlCommand cmd = new SqlCommand(req,d.cn);
            d.cn.Open();
            SqlDataReader dataR = cmd.ExecuteReader();
            while(dataR.Read())
            {
                Produit produit = new Produit();

                produit.IdProduit = dataR[0].ToString();
                produit.Nom = dataR[1].ToString();
                produit.Description = dataR[2].ToString();
                produit.Genre = dataR[3].ToString();
                produit.Prix =float.Parse(dataR[4].ToString());
                produit.Image = dataR[5].ToString();
                produit.idMarque = dataR[6].ToString();


                lstprod.Add(produit);
            }
            dataR.Close();
            d.cn.Close();
            return lstprod;
        }

        public static bool Existe(string idProduit)
        {
            int nbr = 0;
            string req1 = string.Format("Select COUNT(*) from Produit where idProduit='{0}' ", idProduit);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static Produit Recherche(string idProduit)
        {
            Produit produit = new Produit();
            string req = string.Format("select * from produit where idProduit = '{0}' ", idProduit);
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            SqlDataReader dataR = cmd.ExecuteReader();
            if(dataR.Read())
            {
                produit.IdProduit = dataR[0].ToString();
                produit.Nom = dataR[1].ToString();
                produit.Description = dataR[2].ToString();
                produit.Genre = dataR[3].ToString();
                produit.Prix = float.Parse(dataR[4].ToString());
                produit.Image = dataR[5].ToString();
                produit.idMarque = dataR[6].ToString();
            }
            d.cn.Close();
            return produit;
        }

        public static bool supprimer(string idProduit)
        {
            int nbr = 0;
            string req1 = string.Format("Select COUNT(*) from Produit where idProduit='{0}' ", idProduit);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr == 0)
            {
                return false;
            }
            else
            {
                string req2 = string.Format("delete Produit where idProduit='{0}' ", idProduit);
                SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                d.cn.Open();
                cmd2.ExecuteNonQuery();
                d.cn.Close();
                return true;
            }
        }
        public bool modifer()
        {
            int nbr = 0;
            string req1 = string.Format("Select COUNT(*) from Produit where idProduit='{0}' ", IdProduit);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr == 0)
            {
                return false;
            }
            else
            {
                string req2 = string.Format(@"Update Produit 
                                              set nom='{0}',description='{1}',genre='{2}',prix={3},idMarque = '{4}'
                                              where idProduit='{5}' ",Nom,Description,Genre,Prix,idMarque,IdProduit);
                SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                d.cn.Open();
                cmd2.ExecuteNonQuery();
                d.cn.Close();
                return true;
            }
        }

       public static int getTailleQteStock (string idProduit,string idTaille)
        {
            int nbr = 0;
            string req = string.Format(@"select qteStock
                                        from QteStock_Taille
                                        where idProduit = '{0}'
                                        and idTaille = '{1}' ", idProduit, idTaille);
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            if (cmd.ExecuteScalar() != null)
            {
                nbr = int.Parse(cmd.ExecuteScalar().ToString());
            }
            d.cn.Close();
            return nbr;
        }
       
    }
}