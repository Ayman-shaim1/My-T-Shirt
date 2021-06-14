using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using My_T_Shirt.Helpers;
namespace My_T_Shirt.Models
{
    public class Utilisateur
    {
        public string IdUtilisateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public bool IsAdmin { get; set; }

        public Utilisateur()
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdUtilisateur = myuuidAsString;
        }

        public Utilisateur(string Nom,string Prenom,string Email,string MotDePasse,bool IsAdmin)
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdUtilisateur = myuuidAsString;

            this.Nom = Nom;
            this.Prenom = Prenom;
            this.Email = Email;
            this.MotDePasse = MotDePasse;
            this.IsAdmin = IsAdmin;
        }

        public bool Ajouter()
        {
            int nbr = 0;
            string req1 = string.Format("select count(*) from Utilisateur where email = '{0}' ", Email);
            SqlCommand cmd = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr == 0)
            {
                string req2 = string.Format("insert into Utilisateur values ('{0}','{1}','{2}','{3}','{4}','{5}')", IdUtilisateur, Nom, Prenom, Email, Cryptage.Encrypt(MotDePasse), IsAdmin.ToString());
                SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                d.cn.Open();
                cmd2.ExecuteNonQuery();
                d.cn.Close();
                return true;
            }
            return false;
        }
        public static bool Supprimer(string id)
        {
            int nbr = 0;
            string req1 = string.Format("select count(*) from Utilisateur where idUtilisateur = '{0}' ", id);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if (nbr > 0)
            {
                string req2 = string.Format("Delete from Utilisateur where idUtilisateur = '{0}' ", id);
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

        
        

    }
}