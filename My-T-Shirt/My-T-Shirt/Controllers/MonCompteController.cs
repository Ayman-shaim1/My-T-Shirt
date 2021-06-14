using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using My_T_Shirt.Helpers;
using My_T_Shirt.Models;
using My_T_Shirt.ViewModel;
using System.Data.SqlClient;

namespace My_T_Shirt.Controllers
{
    public class MonCompteController : Controller
    {
        // GET: MonCompte
        public ActionResult Index()
        {
            if(Authentication.isConnected())
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                List<Commande> lstcommandes = new List<Commande>();
                string req1 = string.Format("select * from commande where idUtilisateur = '{0}' order by Date_Com desc ", Authentication.getId(false));
                SqlCommand cmd1 = new SqlCommand(req1, d.cn);
                d.cn.Open();
                SqlDataReader dataR1 = cmd1.ExecuteReader();
                while (dataR1.Read())
                {
                    Commande commande = new Commande();
                    commande.IdCommande = dataR1[0].ToString();
                    commande.idUtilisateur = dataR1[1].ToString();
                    commande.Adresse_Com = dataR1[2].ToString();
                    commande.Ville_Com = dataR1[3].ToString();
                    commande.CodePostal_Com = dataR1[4].ToString();
                    commande.Pays_Com = dataR1[5].ToString();
                    commande.Date_Com = DateTime.Parse(dataR1[6].ToString());
                    commande.isDelivered = bool.Parse(dataR1[7].ToString());

                    if (dataR1[8].ToString() != "")
                    {
                        commande.Date_Livraison = DateTime.Parse(dataR1[8].ToString());
                    }
                    lstcommandes.Add(commande);
                }
                dataR1.Close();
                d.cn.Close();
                return View(lstcommandes);
            }
            else
            {
                return RedirectToAction("", "SeConnecter");
            }
        }

        public ActionResult SeDeconnecter()
        {
            Authentication.seDeconnecter(false);
            GrProduit.SupprimerPanier();
            GrProduit.SupprimerAdresseCommande();
            return RedirectToAction("","Accueil");
        }

        [HttpPost]
        public JsonResult ModifierInfo(ChangerNomPrenom model)
        {
            try
            {
                if (model != null)
                {
                    string req = string.Format(@"update utilisateur set nom = '{0}', prenom = '{1}' 
                                                where idUtilisateur='{2}' ",
                                                model.nom, model.prenom, Authentication.getId(false));

                    SqlCommand cmd = new SqlCommand(req, d.cn);
                    d.cn.Open();
                    cmd.ExecuteNonQuery();
                    d.cn.Close();
                    if (Request.Cookies["Login"] != null)
                    {
                        HttpCookie cookie = Request.Cookies["Login"];
                        cookie.Values["Nom"] = model.nom;
                        cookie.Values["Prenom"] = model.prenom;
                        cookie.Expires = DateTime.Now.AddDays(1);
                        Response.SetCookie(cookie);
                    }
                    return Json(new { done = true });
                }
                else
                {
                    Response.StatusCode = 500;
                    return Json(new { done = false, Message = "Model null !!" });
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { done = false, ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ModfierMotDePasse(ChangerMotDePasse model)
        {
            try
            {
                bool existe = false;
                string motdepasse = "";
                string req1 = string.Format(@"select motdepasse from Utilisateur
                                              where idUtilisateur = '{0}' ",
                                              Authentication.getId(false));

                SqlCommand cmd1 = new SqlCommand(req1, d.cn);
                d.cn.Open();
                if (cmd1.ExecuteScalar() != null)
                {
                    motdepasse = Cryptage.Decrypt(cmd1.ExecuteScalar().ToString());
                    if (motdepasse == model.MotDePasseActuel)
                        existe = true;

                }
                d.cn.Close();
                if (existe)
                {
                    string req2 = string.Format("update Utilisateur set motdepasse='{0}' where idUtilisateur='{1}'",Cryptage.Encrypt(model.NouveauMotDePasse), Authentication.getId(false));
                    SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                    d.cn.Open();
                    cmd2.ExecuteNonQuery();
                    d.cn.Close();

                    return Json(new
                    {
                        done = true,
                    });

                }
                else
                {
                    Response.StatusCode = 404;
                    return Json(new
                    {
                        done = false,
                        Message = "Mot de passe actuel incorrect !"
                    });
                }

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new
                {
                    done = false,
                    ex.Message,
                    model
                });
            }
        }
    }
}