using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using My_T_Shirt.Models;
using My_T_Shirt.ViewModel;
using My_T_Shirt.Helpers;
using System.IO;
using PagedList;
using System.Data.SqlClient;

namespace My_T_Shirt.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Authentication.isAdmin_Connected())
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                return View();
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
        }

        public ActionResult Profile()
        {
            if (Authentication.isAdmin_Connected())
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                return View();
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
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
                                                model.nom, model.prenom, Authentication.getId(true));

                    SqlCommand cmd = new SqlCommand(req, d.cn);
                    d.cn.Open();
                    cmd.ExecuteNonQuery();
                    d.cn.Close();
                    if (Request.Cookies["Admin_Login"] != null)
                    {
                        HttpCookie cookie = Request.Cookies["Admin_Login"];
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
                                              Authentication.getId(true));

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
                    string req2 = string.Format("update Utilisateur set motdepasse='{0}' where idUtilisateur='{1}'", Cryptage.Encrypt(model.NouveauMotDePasse), Authentication.getId(true));
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
    

    [HttpGet]
        public ActionResult SeConnecter()
        {
           if(!Authentication.isAdmin_Connected())
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();
                ViewBag.error = "";
                return View();
            }
            else
            {
                return RedirectToAction("", "Admin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeConnecter(Utilisateur utilisateur)
        {
            string Email = utilisateur.Email;
            string Motdepasse = utilisateur.MotDePasse;
            bool resultat = Authentication.SeConnecter(Email, Motdepasse, true);
            if (resultat)
            {
                return RedirectToAction("", "Admin");
            }
            else
            {
                ViewBag.error = "Email ou mot de passe incorrect !";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeDeconnecter()
        {
            Authentication.seDeconnecter(true);
            return RedirectToAction("SeConnecter", "Admin");
        }

        [HttpGet]
        public ActionResult AjouterProduit()
        {
            if (Authentication.isAdmin_Connected())
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();


                ViewData["marques"] = Marque.Afficher();
                ViewBag.Exception = "";
                return View();
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjouterProduit(Produit produit, HttpPostedFileBase upload)
        {
            try
            {

                Produit produitToAdd = new Produit()
                {
                    Nom = produit.Nom,
                    Description = produit.Description,
                    
                    Genre = produit.Genre,
                    Prix = produit.Prix,
                    Image = "/Uploads/" + upload.FileName,
                    idMarque = produit.idMarque
                };

                string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                upload.SaveAs(path);
                produitToAdd.Ajouter();
                return RedirectToAction("Produits", "Admin");
            }
            catch(Exception ex)
            {
                if(d.cn.State == System.Data.ConnectionState.Open)
                {
                    d.cn.Close();
                }
                ViewBag.Exception = ex.Message;
                return View();
            }
        }


        [HttpGet]
        public ActionResult Produits(int? page)
        {
            if (Authentication.isAdmin_Connected())
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                const int pageSize = 9;
                int pageNumbre = page ?? 1;
                return View(Produit.Affcher().ToPagedList(pageNumbre,pageSize));
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
        }

        [HttpPost]
        public ActionResult SupprimerProduit(string idProduit)
        {
            try
            {
                bool isDeleted = Produit.supprimer(idProduit);
                if (!isDeleted)
                {
                    Response.StatusCode = 404;
                    return Json(new { done = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = 200;
                    return Json(new { done = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                Response.StatusCode = 500;
                return Json(new { done = false,ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DetailsProduit(string idProduit)
        {
            if (Produit.Existe(idProduit))
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();


                ViewData["marques"] = Marque.Afficher();
                ViewData["tailles"] = Taille.Afficher();
                return View(Produit.Recherche(idProduit));
            }
            else
            {
                return RedirectToAction("Produits","Admin");
            }
        }

        [HttpPost]
        public JsonResult getTailleQteStock(string idProduit,string idTaille)
        {
            try
            {
                int qteStock = Produit.getTailleQteStock(idProduit, idTaille);
                return Json(new
                {
                    done = true,
                    qteStock
                },
                JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                return Json(new
                {
                    done = false,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ModfierQteStock(string idProduit, string idTaille, int nouvelle_qteStock)
        {
            try
            {
                int nbr = 0;
                string req1 = string.Format(@"select count(*) from QteStock_Taille
                                                where idProduit = '{0}' and idTaille = '{1}' ", idProduit, idTaille);
              
              
                SqlCommand cmd1 = new SqlCommand(req1, d.cn);
                d.cn.Open();
                nbr = int.Parse(cmd1.ExecuteScalar().ToString());
                d.cn.Close();
                if(nbr == 0)
                {
                    string req2 = string.Format(@"insert into QteStock_Taille 
                                                values('{0}','{1}',{2})",  idTaille, idProduit, nouvelle_qteStock);
                    SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                    d.cn.Open();
                    cmd2.ExecuteNonQuery();
                    d.cn.Close();
                    return Json(new
                    {
                        done = true,
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string req3 = string.Format(@"update QteStock_Taille set qteStock = {0} 
                                                 where idProduit = '{1}' and idTaille = '{2}' ", nouvelle_qteStock, idProduit, idTaille);
                    SqlCommand cmd3 = new SqlCommand(req3, d.cn);
                    d.cn.Open();
                    cmd3.ExecuteNonQuery();
                    d.cn.Close();
                    return Json(new
                    {
                        done = true,
                    }, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                return Json(new
                {
                    done = false,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ModifierProduit(Produit produit)
        {
            try
            {
                bool isModified = produit.modifer();
                if (isModified)
                {
                    return Json(new { done = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = 404;
                    return Json(new { done = false,Message = "Produit introuvable !" }, JsonRequestBehavior.AllowGet);
                }
            }catch(Exception ex)
            {
                Response.StatusCode = 500;

                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                    return Json(new { done = false ,ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ModifierProduit_Image(string idProduit,HttpPostedFile upload)
        {
            try
            {
                if (upload != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                    upload.SaveAs(path);
                    string db_path = "/Uploads/" + upload.FileName;
                    string req = string.Format("update Produit set image='{0}' where idProduit='{1}' ", db_path, idProduit);
                    SqlCommand cmd = new SqlCommand(req, d.cn);
                    d.cn.Open();
                    cmd.ExecuteNonQuery();
                    d.cn.Close();
                    return Json(new { done = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = 404;
                    return Json(new { done = false,Message="donnees introuvable !", idProduit,upload }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                Response.StatusCode = 500;
                return Json(new { done = false, ex.Message,idProduit,upload },JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult AjouterAuSolde(Solde solde)
        {
            try
            {
                bool isAdded = solde.Ajouter();
                if (isAdded)
                {
                    Response.StatusCode = 201;
                    return Json(new { done = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = 500;
                    return Json(new { done = false ,error = "ce produit est deja en solde !"}, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                {
                    d.cn.Close();
                }
                Response.StatusCode = 500;
                return Json(new { done = false, error = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult SupprimerSolde(string idProduit)
        {
            try
            {
                string req = string.Format("delete from solde where idproduit = '{0}' ", idProduit);
                SqlCommand cmd = new SqlCommand(req, d.cn);
                d.cn.Open();
                cmd.ExecuteNonQuery();
                d.cn.Close();
                return Json(new
                {
                    done = true
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                return Json(new
                {
                    done = false,
                    ex.Message,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Commandes(int? page,string type = "",DateTime? dateDebut = null,DateTime? dateFin = null)
        {
            if (Authentication.isAdmin_Connected())
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                List<Commande> commandes = new List<Commande>();
                ViewBag.type = type;

                if (type.ToLower() == "livree")
                {
                    ViewBag.Type = type.ToLower();
                    if (dateDebut != null && dateFin != null)
                    {
                        commandes = Commande.Afficher(true, dateDebut, dateFin);
                        ViewBag.dateDebut = DateTime.Parse(dateDebut.ToString()).ToString("yyyy-MM-dd");
                        ViewBag.dateFin = DateTime.Parse(dateFin.ToString()).ToString("yyyy-MM-dd");

                    }
                    else
                        commandes = Commande.Afficher(true, null, null);

                }
                else if (type.ToLower() == "nonlivree")
                {
                    ViewBag.Type = type.ToLower();

                    if (dateDebut != null && dateFin != null)
                    {
                        commandes = Commande.Afficher(false, dateDebut, dateFin);
                        ViewBag.dateDebut = DateTime.Parse(dateDebut.ToString()).ToString("yyyy-MM-dd");
                        ViewBag.dateFin = DateTime.Parse(dateFin.ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                        commandes = Commande.Afficher(false, null, null);
                }
                else if (type == "")
                {
                    if (dateDebut != null && dateFin != null)
                    {
                        commandes = Commande.Afficher(null, dateDebut, dateFin);
                        ViewBag.dateDebut = DateTime.Parse(dateDebut.ToString()).ToString("yyyy-MM-dd");
                        ViewBag.dateFin = DateTime.Parse(dateFin.ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                        commandes = Commande.Afficher(null, null, null);
                }
                const int pageSize = 9;
                int pageNumbre = page ?? 1;
                return View(commandes.ToPagedList(pageNumbre, pageSize));
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
        }


        [HttpGet]
        public ActionResult DetailsCommande(string idCommande)
        {


            if (Authentication.isAdmin_Connected())
            {
                if (idCommande != null && idCommande != "")
                {
                    if (d.cn.State == System.Data.ConnectionState.Open)
                        d.cn.Close();

                    ShowCommande commande = new ShowCommande();
                    string req1 = string.Format("select * from commande where idCommande = '{0}' order by Date_Com desc ", idCommande);
                    string req2 = string.Format(@"select p.idProduit,qte,idTaille,nom,Image ,prix
                                                from ligneCommande l inner join produit p 
                                                on l.idProduit = p.idProduit
                                                where idCommande = '{0}' ", idCommande);

                    SqlCommand cmd1 = new SqlCommand(req1, d.cn);
                    SqlCommand cmd2 = new SqlCommand(req2, d.cn);

                    d.cn.Open();
                    SqlDataReader datar1 = cmd1.ExecuteReader();


                    if (datar1.Read())
                    {
                        commande.IdCommande = datar1[0].ToString();
                        commande.idUtilisateur = datar1[1].ToString();
                        commande.Adresse_Com = datar1[2].ToString();
                        commande.Ville_Com = datar1[3].ToString();
                        commande.CodePostal_Com = datar1[4].ToString();
                        commande.Pays_Com = datar1[5].ToString();
                        commande.Date_Com = DateTime.Parse(datar1[6].ToString());
                        commande.isDelivered = bool.Parse(datar1[7].ToString());

                        if (datar1[8].ToString() != "")
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
                    return RedirectToAction("Commandes", "Admin");
                }
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
        }

        [HttpPost]
        public JsonResult livreCommande(string idCommande)
        {
            try
            {
                DateTime dateLivraison = DateTime.Now;
                string req = string.Format(@"update commande set isDelivered = 'True',dateLivraison='{0}' 
                                            where idCommande = '{1}' ", dateLivraison,idCommande);

                SqlCommand cmd = new SqlCommand(req, d.cn);
                d.cn.Open();
                  cmd.ExecuteNonQuery();
                d.cn.Close();

                return Json(new
                {
                    done = true,
                    dateLivraison
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                return Json(new
                {
                    done = false,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Marques()
        {

            if (Authentication.isAdmin_Connected())
            {
                return View(Marque.Afficher());
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }

        }


        [HttpGet]
        public ActionResult AjouterMarque()
        {
            if(Authentication.isAdmin_Connected())
            {
                ViewBag.error = "";
                return View();
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjouterMarque(Marque marque)
        {
            try
            {
                ViewBag.error = "";
                bool isAdded = marque.AjouterMarque();
                if (isAdded)
                {
                    return RedirectToAction("Marques", "Admin");

                }
                else
                {
                    ViewBag.error = "cette marque exsite deja !";
                    return View();
                }
            }
            catch(Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public JsonResult SupprimerMarque(string idMarque)
        {
            try
            { 
                bool isDeleted = Marque.Supprimer(idMarque);
                if (isDeleted == true)
                {
                    return Json(new
                    {
                        done = true,
                        idMarque,
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = 404;
                    return Json(new
                    {
                        done = false,
                        idMarque,
                        Message = "Marque introuvable !!"
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new
                {
                    done = false,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DetailsMarque(string idMarque)
        {
            if(Authentication.isAdmin_Connected())
            {
                if (idMarque != null)
                {
                    return View(Marque.Recherche(idMarque));
                }
                else
                {
                    return RedirectToAction("Marques", "Admin");

                }
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
        }

        [HttpPost]
        public JsonResult ModifierMarque(Marque marque)
        {
            try
            {
                bool isModified = Marque.Modifier(marque);
                if (isModified)
                {
                    return Json(new
                    {
                        done = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = 404;
                    return Json(new
                    {
                        done = false,
                        Message = "Marque introuvable !"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new
                {
                    done = false,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AjouterTaille()
        {
            if (Authentication.isAdmin_Connected())
            {
                ViewBag.error = "";
                return View();
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjouterTaille(Taille taille)
        {

            try
            {
                bool isAdded = taille.Ajouter();
                if (isAdded)
                {
                    return RedirectToAction("Tailles", "Admin");
                }
                else
                {
                    ViewBag.error = "Cette taille existe deja !";
                    return View();
                }
                
            }
            catch(Exception ex)
            {
                 ViewBag.error = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Tailles()
        {
            if (Authentication.isAdmin_Connected())
            {
                return View(Taille.Afficher());
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
        }

        [HttpGet]
        public ActionResult DetailsTaille(string idTaille)
        {
            if (Authentication.isAdmin_Connected())
            {
                return View(Taille.Recherche(idTaille));
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
        }


        [HttpPost]
        public JsonResult ModifierTaille(Taille taille)
        {
            try
            {
                bool isModified = Taille.Modifier(taille);
                if (isModified)
                {
                    return Json(new
                    {
                        done = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = 404;
                    return Json(new
                    {
                        done = false,
                        Message = "taille introuvable !"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode =500;
                return Json(new
                {
                    done = false,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SupprimerTaille(string idTaille)
        {
            try
            {
                bool isDeleted = Taille.Supprimer(idTaille);
                if(isDeleted)
                {
                  
                    return Json(new
                    {
                        done = true,
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = 404;
                    return Json(new
                    {
                        done = false,
                        Message = "taille introuvable !"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 404;

                return Json(new
                {
                    done = false,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AjouterAdmin()
        {
            if(Authentication.isAdmin_Connected())
            {
                ViewBag.error = "";
                return View();
            }
            else
            {
                return RedirectToAction("SeConnecter", "Admin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjouterAdmin(Utilisateur utilisateur)
        {
            try
            {
                int nbr = 0;
                string req1 = string.Format("select count(*) from Utilisateur where email = '{0}' ", utilisateur.Email);
                SqlCommand cmd = new SqlCommand(req1, d.cn);
                d.cn.Open();
                nbr = int.Parse(cmd.ExecuteScalar().ToString());
                d.cn.Close();
                if (nbr == 0)
                {
                    string req2 = string.Format("insert into Utilisateur values ('{0}','{1}','{2}','{3}','{4}','True')", utilisateur.IdUtilisateur, utilisateur.Nom, utilisateur.Prenom, utilisateur.Email, Cryptage.Encrypt(utilisateur.MotDePasse));
                    SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                    d.cn.Open();
                    cmd2.ExecuteNonQuery();
                    d.cn.Close();
                    return RedirectToAction("","Admin");
                }
                else
                {
                    ViewBag.error = "ce compte existe deja !!";
                    return View();
                }
                    


            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }
    }
}