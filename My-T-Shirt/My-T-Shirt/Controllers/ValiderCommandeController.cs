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
    public class ValiderCommandeController : Controller
    {
        // GET: ValiderCommande
        public ActionResult Index()
        {
            if (Authentication.isConnected())
            {
                if (GrProduit.isAdr_Com_Existe())
                {
                    if (d.cn.State == System.Data.ConnectionState.Open)
                        d.cn.Close();
                    return View(new ValiderCommande() {
                        adresse= GrProduit.GetAdresseCommande(),
                        produits = GrProduit.GetPanier(),
                    });
                }
                else
                {
                    return RedirectToAction("", "AdresseCommande");
                }
            }
            else
            {
                return RedirectToAction("", "SeConnecter");
            }
        }

        [HttpPost]
        public ActionResult Index(ValiderCommande model)
        {

            Commande commande = null;
            string req1 = "";
            try
            {
                d.cn.Open();
                SqlTransaction T = d.cn.BeginTransaction();


                commande = new
                      Commande(Authentication.getId(false), model.adresse.adresse, model.adresse.ville, model.adresse.codePostal, model.adresse.pays);
                req1 = string.Format("insert into commande values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',null)",
                  commande.IdCommande, commande.idUtilisateur, commande.Adresse_Com,
                  commande.Ville_Com, commande.CodePostal_Com, commande.Pays_Com, commande.Date_Com,
                  commande.isDelivered);
                SqlCommand cmd1 = new SqlCommand(req1, d.cn);
                cmd1.Transaction = T;
                cmd1.ExecuteNonQuery();
                foreach (var produit in model.produits)
                {
                    string req2 = string.Format("insert into ligneCommande values('{0}','{1}',{2},'{3}')", commande.IdCommande, produit.idProduit, produit.qte, produit.idTaille);
                    string req3 = string.Format("update QteStock_Taille set qteStock = qteStock - {0} where idProduit='{1}' and idTaille = '{2}'", produit.qte, produit.idProduit,produit.idTaille);

                    SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                    SqlCommand cmd3 = new SqlCommand(req3, d.cn);

                    cmd2.Transaction = T;
                    cmd3.Transaction = T;
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();
                }
                T.Commit();
                d.cn.Close();
                GrProduit.SupprimerPanier();
                GrProduit.SupprimerAdresseCommande();
                return Json(new { done = true, commande.IdCommande }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { done = false, ex.Message ,model}, JsonRequestBehavior.AllowGet);

            }
        }
    }
}