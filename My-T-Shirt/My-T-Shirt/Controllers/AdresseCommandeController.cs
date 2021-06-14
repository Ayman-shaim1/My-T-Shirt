using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using My_T_Shirt.Models;
using My_T_Shirt.Helpers;
using My_T_Shirt.ViewModel;

namespace My_T_Shirt.Controllers
{
    public class AdresseCommandeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Authentication.isConnected())
            {
                if (GrProduit.GetPanier().Count != 0)
                {
                    if (d.cn.State == System.Data.ConnectionState.Open)
                        d.cn.Close();

                    ViewBag.error = "";
                    return View(GrProduit.GetAdresseCommande());
                }
                else
                {
                    return RedirectToAction("", "Panier");
                }
            }
            else
            {
                return RedirectToAction("", "SeConnecter");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AdresseCommande adresse)
        {
            try
            {
                ViewBag.error = "";
                if (adresse != null)
                {

                    GrProduit.setAdresseCommande(adresse);
                    return RedirectToAction("", "ValiderCommande");
                }
                else
                {
                    ViewBag.error = "Erreur adresse null !";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult setAdresseCommande(AdresseCommande adresse)
        {
            try
            {
                ViewBag.error = "";
                if (adresse != null)
                {

                    GrProduit.setAdresseCommande(adresse);
                    return Json(new { done = true, adresse },JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = 404;

                    return Json(new { done = false,Message="adresse introuvable !" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                ViewBag.error = ex.Message;
                return Json(new { done = false ,ex.Message}, JsonRequestBehavior.AllowGet);

            }
        }
    }
}