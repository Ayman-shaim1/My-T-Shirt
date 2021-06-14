using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using My_T_Shirt.Models;
using My_T_Shirt.ViewModel;
using My_T_Shirt.Helpers;
using System.Data.SqlClient;

namespace My_T_Shirt.Controllers
{
    public class CommandeController : Controller
    {
        // GET: Commande
        public ActionResult Index(string idCommande)
        {
            if (idCommande != null && idCommande != "" && Authentication.isConnected() && Commande.existe(idCommande))
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                ShowCommande commande = new ShowCommande();
                string req1 = string.Format("select * from commande where idCommande = '{0}' ", idCommande);
                string req2 = string.Format(@"select p.idProduit,qte,idTaille,nom,Image ,prix
                                                from ligneCommande l inner join produit p 
                                                on l.idProduit = p.idProduit
                                                where idCommande = '{0}' ", idCommande);

                SqlCommand cmd1 = new SqlCommand(req1,d.cn);
                SqlCommand cmd2 = new SqlCommand(req2, d.cn);

                d.cn.Open();
                SqlDataReader datar1 = cmd1.ExecuteReader();
               

                if(datar1.Read())
                {
                    commande.IdCommande = datar1[0].ToString();
                    commande.idUtilisateur = datar1[1].ToString();
                    commande.Adresse_Com = datar1[2].ToString();
                    commande.Ville_Com = datar1[3].ToString();
                    commande.CodePostal_Com = datar1[4].ToString();
                    commande.Pays_Com = datar1[5].ToString();
                    commande.Date_Com = DateTime.Parse(datar1[6].ToString());
                    commande.isDelivered = bool.Parse(datar1[7].ToString());

                    if(datar1[8].ToString() != "")
                    {
                        commande.Date_Livraison = DateTime.Parse(datar1[8].ToString());
                    }
                }

                datar1.Close();
                SqlDataReader datar2 = cmd2.ExecuteReader();
                while (datar2.Read())
                {
                    ProduitPanier produit = new ProduitPanier();
                    produit.idProduit = datar2[0].ToString();
                    produit.qte = int.Parse(datar2[1].ToString());
                    produit.idTaille = datar2[2].ToString();
                    produit.Nom = datar2[3].ToString();
                    produit.Image = datar2[4].ToString();
                    produit.prix = float.Parse(datar2[5].ToString());

                    commande.produits.Add(produit);
                }

                datar2.Close();

                d.cn.Close();
                
                return View(commande);
            }
            else
            {
                return RedirectToAction("", "MonCompte");
            }
        }
    }
}