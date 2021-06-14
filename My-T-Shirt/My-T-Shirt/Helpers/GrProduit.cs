using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using My_T_Shirt.Models;
using My_T_Shirt.ViewModel;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace My_T_Shirt.Helpers
{
    public class GrProduit
    {
        public static void AjouterAuPanier(ProduitPanier produit)
        {
            HttpCookie Panier = HttpContext.Current.Request.Cookies["Panier"];
            if(Panier == null)
            {
                Panier = new HttpCookie("Panier");
              
                List<ProduitPanier> lstprods = new List<ProduitPanier>();
                lstprods.Add(produit);
                Panier.Value = JsonConvert.SerializeObject(lstprods);
                Panier.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.SetCookie(Panier);
            }
            else
            {
                if(!inPanier(produit.idProduit,produit.idTaille))
                {
                    List<ProduitPanier> lstProduits = JsonConvert.DeserializeObject<List<ProduitPanier>>(Panier.Value);
                    lstProduits.Add(produit);
                    Panier.Value = JsonConvert.SerializeObject(lstProduits);
                    HttpContext.Current.Response.SetCookie(Panier);
                }
            }
        }

        public static List<ProduitPanier> GetPanier()
        {
            HttpCookie Panier = HttpContext.Current.Request.Cookies["Panier"];
            List<ProduitPanier> lstProduits = new List<ProduitPanier>();
            if (Panier != null)
            {
                lstProduits = JsonConvert.DeserializeObject<List<ProduitPanier>>(Panier.Value);
            }
            return lstProduits;
        }

        public static bool inPanier(string idProduit,string idTaille)
        {
            bool exsite = false;
            HttpCookie Panier = HttpContext.Current.Request.Cookies["Panier"];
            if (Panier != null)
            {
                List<ProduitPanier> lstProduits = JsonConvert.DeserializeObject<List<ProduitPanier>>(Panier.Value);
                foreach(ProduitPanier produit in lstProduits)
                {
                    if(produit.idProduit == idProduit && produit.idTaille == idTaille)
                    {
                        exsite = true;
                        break;
                    }
                }
            }
            else
            {
                exsite = false;
            }
            return exsite;
        }

        public static void AugmenterQte(string idProduit,string idTaille)
        {
            if (inPanier(idProduit, idTaille))
            {
                HttpCookie Panier = HttpContext.Current.Request.Cookies["Panier"];
                List<ProduitPanier> lstProduits = JsonConvert.DeserializeObject<List<ProduitPanier>>(Panier.Value);
                foreach (ProduitPanier produit in lstProduits)
                {
                    if (produit.idProduit == idProduit && produit.idTaille == idTaille)
                    {
                        produit.qte = produit.qte+1;
                        break;
                    }
                }
                Panier.Value = JsonConvert.SerializeObject(lstProduits);
                HttpContext.Current.Response.SetCookie(Panier);

            }
        }

        public static void DiminuerQte(string idProduit,string idTaille)
        {
            if (inPanier(idProduit, idTaille))
            {
                HttpCookie Panier = HttpContext.Current.Request.Cookies["Panier"];
                List<ProduitPanier> lstProduits = JsonConvert.DeserializeObject<List<ProduitPanier>>(Panier.Value);
                foreach (ProduitPanier produit in lstProduits)
                {
                    if (produit.idProduit == idProduit && produit.idTaille == idTaille)
                    {
                        produit.qte = produit.qte-1;
                        break;
                    }
                }
                Panier.Value = JsonConvert.SerializeObject(lstProduits);
                HttpContext.Current.Response.SetCookie(Panier);
            }
        }


        public static void supprimerDuPanier(string idProduit,string idTaille)
        {
            if (inPanier(idProduit, idTaille))
            {
                HttpCookie Panier = HttpContext.Current.Request.Cookies["Panier"];
                List<ProduitPanier> lstProduits = JsonConvert.DeserializeObject<List<ProduitPanier>>(Panier.Value);
                foreach (ProduitPanier produit in lstProduits)
                {
                    if (produit.idProduit == idProduit && produit.idTaille == idTaille)
                    {
                        lstProduits.Remove(produit);
                        break;
                    }
                }
                Panier.Value = JsonConvert.SerializeObject(lstProduits);
                HttpContext.Current.Response.SetCookie(Panier);
            }
        }

        public static void SupprimerPanier()
        {
            HttpCookie Panier = HttpContext.Current.Request.Cookies["Panier"];
            if (Panier != null)
            {
                Panier.Expires = DateTime.Now.AddHours(-1);
                HttpContext.Current.Response.SetCookie(Panier);
            }
        }

        public static void setAdresseCommande(AdresseCommande adresse)
        {
            HttpCookie Adresse = HttpContext.Current.Request.Cookies["AdresseCommande"];
            if (Adresse == null)
            {
                Adresse = new HttpCookie("AdresseCommande");
                if (adresse != null)
                {
                    Adresse.Value = JsonConvert.SerializeObject(adresse);
                    Adresse.Expires = DateTime.Now.AddMinutes(15);
                    HttpContext.Current.Response.SetCookie(Adresse);
                }
            }
            else
            {
                if (adresse != null)
                {
                    Adresse.Value = JsonConvert.SerializeObject(adresse);
                    Adresse.Expires = DateTime.Now.AddMinutes(15);
                    HttpContext.Current.Response.SetCookie(Adresse);
                }
            }
        }



        public static AdresseCommande GetAdresseCommande()
        {
            AdresseCommande adresse = new AdresseCommande();
            HttpCookie Cookie_Adresse = HttpContext.Current.Request.Cookies["AdresseCommande"];
            if (Cookie_Adresse != null)
            {
                AdresseCommande valCookie_Adress = JsonConvert.DeserializeObject<AdresseCommande>(Cookie_Adresse.Value);
                adresse.pays = valCookie_Adress.pays;
                adresse.ville = valCookie_Adress.ville;
                adresse.codePostal = valCookie_Adress.codePostal;
                adresse.adresse = valCookie_Adress.adresse;
            }

            return adresse;
        }


        public static bool isAdr_Com_Existe()
        {
            HttpCookie Cookie_Adresse = HttpContext.Current.Request.Cookies["AdresseCommande"];
            bool existe = false;
            if (Cookie_Adresse != null)
            {
                if(Cookie_Adresse.Value != null &&  Cookie_Adresse.Value != "")
                {
                    existe = true;
                }
            }
            return existe;
        }

        public static void SupprimerAdresseCommande()
        {
            HttpCookie AdresseCommande = HttpContext.Current.Request.Cookies["AdresseCommande"];
            if (AdresseCommande != null)
            {
                AdresseCommande.Expires = DateTime.Now.AddHours(-1);
                HttpContext.Current.Response.SetCookie(AdresseCommande);
            }
        }


    }
}