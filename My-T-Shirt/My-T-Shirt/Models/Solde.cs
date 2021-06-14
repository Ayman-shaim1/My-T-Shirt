using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace My_T_Shirt.Models
{
    public class Solde
    {
        public string IdSolde { get; set; }
        public string IdProduit { get; set; }
        public int pourcentage { get; set; }

        public Solde ()
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdSolde = myuuidAsString;
        }

        public Solde(string IdProduit, int pourcentage)
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdSolde = myuuidAsString;

            this.IdProduit = IdProduit;
            this.pourcentage = pourcentage;
        }

        public bool Ajouter()
        {
            int nbr = 0;
            string req1 = string.Format("select count(*) from Solde where idProduit='{0}' ",IdProduit);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr == 0)
            {
                string req2 = string.Format("insert into Solde  values('{0}','{1}',{2})", IdSolde, IdProduit, pourcentage);
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

        public static bool inSolde(string idProduit)
        {
            int nbr = 0;
            string req1 = string.Format("select count(*) from Solde where idProduit='{0}' ", idProduit);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr != 0)
            {
                
                return true;
            }
            else
            {
                return false;
            }
        }
        public static float getPourcentage(string idProduit)
        {
            int pourcentage = 0;
            string req = string.Format("select pourcentage from solde where idProduit = '{0}'", idProduit);
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            if(cmd.ExecuteScalar() != null)
            pourcentage = int.Parse(cmd.ExecuteScalar().ToString());
            d.cn.Close();
            return pourcentage;
        }

        public static float getPrixSolde(string idProduit)
        {
            float prix = 0;
            string req = string.Format(@"select dbo.getPrixSolde('{0}')", idProduit);
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            if(cmd.ExecuteScalar() != null)
            {
                prix = float.Parse(cmd.ExecuteScalar().ToString());
            }
            d.cn.Close();
            return prix;
        }

    }
}