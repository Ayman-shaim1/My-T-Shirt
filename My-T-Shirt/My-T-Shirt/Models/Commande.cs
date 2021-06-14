using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace My_T_Shirt.Models
{
    public class Commande
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

        public Commande()
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdCommande = myuuidAsString;
            this.Date_Com = DateTime.Now;
            this.isDelivered = false;
            Date_Livraison = null;
        }

        public Commande(string idUtilisateur,string Adresse_Com,string Ville_Com,string CodePostal_Com,string Pays_Com)
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            IdCommande = myuuidAsString;

            this.idUtilisateur = idUtilisateur;
            this.Adresse_Com = Adresse_Com;
            this.Ville_Com = Ville_Com;
            this.CodePostal_Com = CodePostal_Com;
            this.Pays_Com = Pays_Com;
            this.Date_Com = DateTime.Now;
            this.isDelivered = false;
            Date_Livraison = null;
        }

        public static bool existe(string idCommande)
        {
            bool existe = false;
            int nbr = 0; 
            string req = string.Format("select count(*) from commande where idCommande = '{0}'", idCommande);
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            if(cmd.ExecuteScalar() != null)
                nbr = int.Parse(cmd.ExecuteScalar().ToString());
            d.cn.Close();
            if(nbr != 0)
                existe = true;
            
            return existe;
        }

        public static Commande Recherche(string idCommande)
        {
            Commande commande = new Commande();
            int nbr = 0;
            string req1 = string.Format("select count(*) from commande where idCommande = '{0}'", idCommande);
            SqlCommand cmd1 = new SqlCommand(req1, d.cn);
            d.cn.Open();
            if (cmd1.ExecuteScalar() != null)
                nbr = int.Parse(cmd1.ExecuteScalar().ToString());
            d.cn.Close();
            if(nbr != 0)
            {
                string req2 = string.Format("select * from Commande where idCommande = '{0}' ", idCommande);
                SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                d.cn.Open();
                SqlDataReader dataR = cmd2.ExecuteReader();
                if(dataR.Read())
                {
                    commande.IdCommande = dataR[0].ToString();
                    commande.idUtilisateur = dataR[1].ToString();
                    commande.Adresse_Com = dataR[2].ToString();
                    commande.Ville_Com = dataR[3].ToString();
                    commande.CodePostal_Com = dataR[4].ToString();
                    commande.Pays_Com = dataR[5].ToString();
                    commande.Date_Com = DateTime.Parse(dataR[6].ToString());
                    commande.isDelivered =bool.Parse(dataR[7].ToString());
                    if (dataR[8].ToString() != "")
                    {
                        commande.Date_Livraison = DateTime.Parse(dataR[8].ToString());
                    }
                }
                dataR.Close();
                d.cn.Close();
            }

            return commande;
        }

        public static List<Commande> Afficher(bool? isDelivered, DateTime? dateDebut, DateTime? dateFin)
        {
            List<Commande> lstCommandes = new List<Commande>();
            string req = string.Format("select * from commande ");


            if (isDelivered != null && isDelivered == true)
                req += "where isDelivered = 'True' ";


            if (isDelivered != null && isDelivered == false)
                req += "where isDelivered = 'False'";


            if (isDelivered != null && dateDebut != null && dateFin != null)
                req += string.Format("and date_Com >= '{0}' and date_Com <= '{1}'", dateDebut, dateFin);

            if (isDelivered == null && dateDebut != null && dateFin != null)
                req += string.Format("where date_Com >= '{0}' and date_Com <= '{1}'", dateDebut, dateFin);

            req += "order by date_Com desc";
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            SqlDataReader dataR = cmd.ExecuteReader();
            while (dataR.Read())
            {
                Commande commande = new Commande();
                commande.IdCommande = dataR[0].ToString();
                commande.idUtilisateur = dataR[1].ToString();
                commande.Adresse_Com = dataR[2].ToString();
                commande.Ville_Com = dataR[3].ToString();
                commande.CodePostal_Com = dataR[4].ToString();
                commande.Pays_Com = dataR[5].ToString();
                commande.Date_Com = DateTime.Parse(dataR[6].ToString());
                commande.isDelivered = bool.Parse(dataR[7].ToString());
                if (dataR[8].ToString() != "")
                {
                    commande.Date_Livraison = DateTime.Parse(dataR[8].ToString());
                }
                lstCommandes.Add(commande);
            }
            d.cn.Close();
            return lstCommandes;
        }

        public static float getMontantTotal(string idCommande)
        {
            float mt = 0;
            string req = string.Format(@"select SUM(qte*dbo.getPrixSolde(p.idProduit)) 
                                         from Commande c inner join ligneCommande l
                                         on c.idCommande = l.idCommande inner join Produit p
                                         on l.idProduit = p.idProduit
                                         where c.idCommande='{0}' ", idCommande);

            SqlCommand cmd = new SqlCommand(req,d.cn);
            d.cn.Open();
            if (cmd.ExecuteScalar() != null)
                mt = float.Parse(cmd.ExecuteScalar().ToString());
            d.cn.Close();
            return mt;
        }

        public static string getNom(string idCommande)
        {
            string nom = "";
            string req = string.Format(@"select nom from commande c inner join utilisateur u 
                                        on c.idUtilisateur = u.idUtilisateur
                                        where c.idCommande = '{0}'", idCommande);
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            if(cmd.ExecuteScalar() != null)
            {
                nom = cmd.ExecuteScalar().ToString();
            }
            d.cn.Close();
            return nom;
        }

        public static string getPrenom(string idCommande)
        {
            string prenom = "";
            string req = string.Format(@"select prenom from commande c inner join utilisateur u 
                                        on c.idUtilisateur = u.idUtilisateur
                                        where c.idCommande = '{0}'", idCommande);
            SqlCommand cmd = new SqlCommand(req, d.cn);
            d.cn.Open();
            if (cmd.ExecuteScalar() != null)
            {
                prenom = cmd.ExecuteScalar().ToString();
            }
            d.cn.Close();
            return prenom;
        }
    }
}