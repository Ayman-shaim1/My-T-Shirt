using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using My_T_Shirt.Models;
using My_T_Shirt.ViewModel;

namespace My_T_Shirt.Controllers
{
    public class DetailsProduitController : Controller
    {
        // GET: DetailsProduit
        public ActionResult Index(string idProduit)
        {
            if(Produit.Existe(idProduit))
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                Produit modelProduit = Produit.Recherche(idProduit);

                List<ShowReviews> reviews = new List<ShowReviews>();
                List<Produit> relatedProducts = new List<Produit>();

                string req1 = string.Format("select idReview,r.idUtilisateur,idProduit,Nom,Prenom,Rating,Commentaire,dateReview from Review r inner join Utilisateur u on u.idUtilisateur = r.idUtilisateur where idProduit = '{0}' order by dateReview desc", idProduit);
                SqlCommand cmd1 = new SqlCommand(req1, d.cn);

                string req2 = string.Format("select top 7 * from produit where idMarque = '{0}' and genre = '{1}' and idProduit != '{2}' ", modelProduit.idMarque, modelProduit.Genre,idProduit);
                SqlCommand cmd2 = new SqlCommand(req2, d.cn);

                d.cn.Open();
                SqlDataReader dataR1 = cmd1.ExecuteReader();
                while (dataR1.Read())
                {
                    ShowReviews review = new ShowReviews();
                    review.IdReview = dataR1[0].ToString();
                    review.IdUtilisateur = dataR1[1].ToString();
                    review.IdProduit = dataR1[2].ToString();
                    review.Nom = dataR1[3].ToString();
                    review.Prenom = dataR1[4].ToString();
                    review.Rating = int.Parse(dataR1[5].ToString());
                    review.Commentaire = dataR1[6].ToString();
                    review.dateReview = DateTime.Parse(dataR1[7].ToString());
                    reviews.Add(review);
                }
                dataR1.Close();



          


                SqlDataReader dataR2 = cmd2.ExecuteReader();
                while (dataR2.Read())
                {
                    Produit produit = new Produit();
                    produit.IdProduit = dataR2[0].ToString();
                    produit.Nom = dataR2[1].ToString();
                    produit.Description = dataR2[2].ToString();
                    produit.Genre = dataR2[3].ToString();
                    produit.Prix = float.Parse(dataR2[4].ToString());
                    produit.Image = dataR2[5].ToString();
                    produit.idMarque = dataR2[6].ToString();

                    relatedProducts.Add(produit);

                }
                dataR2.Close();
                d.cn.Close();


                ViewData["reviews"] = reviews;
                ViewData["tailles"] = Taille.Afficher();
                ViewData["relatedProducts"] = relatedProducts;


                return View(modelProduit);
            }
            else
            {
                return RedirectToAction("","Produits");
            }
        }
        [HttpPost]
        public ActionResult MakeAReview(Review review)
        {
            bool isAdded = review.Ajouter();
            if(isAdded)
            {
                return Json(new { done = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { done = false }, JsonRequestBehavior.AllowGet);

            }
        }


        [HttpPost]
        public JsonResult getTailleQteStock(string idProduit, string idTaille)
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

   
  

    }
}