using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace My_T_Shirt.Models
{
    public class Taille
    {
        public string IdTaille { get; set; }
        public string Nom { get; set; }
            
        public Taille()
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdTaille = myuuidAsString;
        }

        public Taille(string Nom)
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdTaille = myuuidAsString;

            this.Nom = Nom;
        }

        public bool Ajouter()
        {
            int nbr = 0;
            string req1 = string.Format("select count(*) from Taille where Nom = '{0}' ", Nom.ToLower());
            SqlCommand cmd1 = new SqlCommand(req1,d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if(nbr == 0)
            {
                string req2 = string.Format("insert into Taille values('{0}','{1}')", IdTaille, Nom);
                SqlCommand cmd = new SqlCommand(req2, d.cn);
                d.cn.Open();
                cmd.ExecuteNonQuery();
                d.cn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<Taille> Afficher()
        {
            List<Taille> lsttaille = new List<Taille>();
            string req = string.Format("select * from Taille");
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            SqlDataReader dataR = cmd.ExecuteReader();
            while (dataR.Read())
            {
                Taille taille = new Taille();
                taille.IdTaille = dataR[0].ToString();
                taille.Nom = dataR[1].ToString();
                lsttaille.Add(taille);
            }
            dataR.Close();
            d.cn.Close();
            return lsttaille;
        }


        public static bool Supprimer(string idTaille)
        {
            int nbr = 0;
            string req1 = string.Format("select count(*) from Taille where idTaille = '{0}' ", idTaille);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr != 0)
            {
                string req2 = string.Format("delete from Taille where idTaille = '{0}' ", idTaille);
                SqlCommand cmd = new SqlCommand(req2, d.cn);
                d.cn.Open();
                cmd.ExecuteNonQuery();
                d.cn.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

  


        public static Taille Recherche(string idTaille)
        {
            Taille taille = new Taille();
            int nbr = 0;
            string req1 = string.Format("select count(*) from Taille where idTaille = '{0}' ", idTaille);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr != 0)
            {
                string req2 = string.Format("select * from Taille where idTaille = '{0}' ", idTaille);
                SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                d.cn.Open();
                SqlDataReader dataR = cmd2.ExecuteReader();
                if (dataR.Read())
                {
                    taille.IdTaille = dataR[0].ToString();
                    taille.Nom = dataR[1].ToString();
                }
                dataR.Close();
                d.cn.Close();
            }
            return taille;
        }

        public static bool Modifier(Taille taille)
        {
            int nbr = 0;
            string req1 = string.Format("select count(*) from Taille where idTaille = '{0}' ", taille.IdTaille);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr != 0)
            { 
                string req2 = string.Format("update Taille set Nom = '{0}' where idTaille = '{1}' ", taille.Nom, taille.IdTaille);
                SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                d.cn.Open();
                cmd2.ExecuteNonQuery();
                d.cn.Close();
                return true;
            }
            return false;
        }

        public static string getNom(string idTaille)
        {
            string nom = "";
            string req = string.Format("select Nom  from Taille where idTaille = '{0}'", idTaille);
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            if (cmd.ExecuteScalar() != null)
            {
                nom = cmd.ExecuteScalar().ToString();
            }
            d.cn.Close();
            return nom;
        }

    }
}