using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace My_T_Shirt.Models
{
    public class Marque
    {
        public string IdMarque { get; set; }
        public string Nom { get; set; }

        public Marque()
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdMarque = myuuidAsString;
        }

        public Marque(string Nom)
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdMarque = myuuidAsString;
            this.Nom = Nom;
        }

        public bool AjouterMarque()
        {

            int nbr = 0;
            string req1 = string.Format("select count(*) from Marque where Nom = '{0}' ", Nom.ToLower());
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
               nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if(nbr == 0)
            {
                string req2 = string.Format("insert into marque values('{0}','{1}')", IdMarque, Nom);
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

        public static List<Marque> Afficher()
        {
            List<Marque> lstmarque = new List<Marque>();
            string req = string.Format("select * from Marque");
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            SqlDataReader dataR = cmd.ExecuteReader();
            while(dataR.Read())
            {
                Marque marque = new Marque();
                marque.IdMarque = dataR[0].ToString();
                marque.Nom = dataR[1].ToString();
                lstmarque.Add(marque);
            }
            dataR.Close();

            d.cn.Close();
            return lstmarque;
        }

        public static bool Supprimer(string idMarque)
        {
            int nbr = 0;
            string req1 = string.Format("select count(*) from Marque where idMarque = '{0}' ", idMarque);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn); 

            d.cn.Open();
                nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();

            if (nbr != 0)
            {
                string req2 = string.Format("Delete Marque where idMarque = '{0}' ", idMarque);
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

        public static Marque Recherche(string idMarque)
        {
            Marque marque = new Marque();
            int nbr = 0;
            string req1 = string.Format("select count(*) from Marque where idMarque = '{0}' ", idMarque);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr != 0)
            {
                string req2 = string.Format("select * from Marque where idMarque = '{0}' ", idMarque);
                SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                d.cn.Open();
                SqlDataReader dataR = cmd2.ExecuteReader();
                if (dataR.Read())
                {
                    marque.IdMarque = dataR[0].ToString();
                    marque.Nom = dataR[1].ToString();
                }
                dataR.Close();
                d.cn.Close();
                return marque;
            }
            return marque;
        }

        public static bool Modifier(Marque marque)
        {
            int nbr = 0;
            string req1 = string.Format("select count(*) from Marque where idMarque = '{0}' ", marque.IdMarque);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr != 0)
            {
                string req2 = string.Format("update Marque set Nom = '{0}' where idMarque = '{1}' ",marque.Nom,marque.IdMarque);
                SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                d.cn.Open();
                cmd2.ExecuteNonQuery();
                d.cn.Close();
                return true;
            }
            return false;
        }

        public static string getNom(string idMarque)
        {
            string nom = "";
            string req = string.Format("select Nom  from Marque where idMarque = '{0}'", idMarque);
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            if(cmd.ExecuteScalar() != null)
            {
                nom = cmd.ExecuteScalar().ToString();
            }
            d.cn.Close();
            return nom;
        }
        public static string getIdMarque(string Nom)
        {
            string idMarque = "";
            string req = string.Format("select idMarque from Marque where Nom Like'{0}%'",Nom.ToLower());
            SqlCommand cmd = new SqlCommand(req, d.cn);

            d.cn.Open();
            if (cmd.ExecuteScalar() != null)
                idMarque = cmd.ExecuteScalar().ToString();
            d.cn.Close();

            return idMarque;
        }
    }
}