using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using My_T_Shirt.Models;
using My_T_Shirt.ViewModel;
using My_T_Shirt.Helpers;

namespace My_T_Shirt.Controllers
{
    public class PanierController : Controller
    {
        // GET: Panier
        public ActionResult Index()
        {
            if (d.cn.State == System.Data.ConnectionState.Open)
                d.cn.Close();
            return View(GrProduit.GetPanier());
        }

        [HttpPost]
        public JsonResult AjouterAuPanier(ProduitPanier produit)
        {
            try
            {
                GrProduit.AjouterAuPanier(produit);
                return Json(new { done = true }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { done = false,ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult inPanier(string idProduit ,string idTaille)
        {
            try
            {
                bool inPanier = GrProduit.inPanier(idProduit,idTaille);
                if (inPanier)
                {
                    return Json(new
                    {
                        done = true,
                        existe = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        done = true,
                        existe = false
                    }, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { done = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AugmenterQte(string idProduit,string idTaille)
        {
            try
            {
                GrProduit.AugmenterQte(idProduit, idTaille);
                return Json(new { done = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;

                return Json(new { done = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DiminuerQte(string idProduit,string idTaille)
        {
            try
            {
                GrProduit.DiminuerQte(idProduit,idTaille);
                return Json(new { done = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;

                return Json(new { done = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult supprimerDuPanier(string idProduit, string idTaille)
        {
            try
            {
                GrProduit.supprimerDuPanier(idProduit,idTaille);
                return Json(new { done = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;

                return Json(new { done = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}