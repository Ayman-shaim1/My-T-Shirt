using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using My_T_Shirt.Models;
using My_T_Shirt.Helpers;

namespace My_T_Shirt.Controllers
{
    public class InscriptionController : Controller
    {
        // GET: Inscription
        public ActionResult Index()
        {
            if(Authentication.isConnected())
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();
                return RedirectToAction("", "Accueil");
            }
            else
            {
                ViewBag.error = "";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(Utilisateur utilisateur)
        {
            ViewBag.error = "";
            bool isAdded = utilisateur.Ajouter();
            if(isAdded)
            {
                Authentication.SeConnecter(utilisateur.Email, utilisateur.MotDePasse, false);
                return RedirectToAction("", "Accueil");
            }
            else
            {
                ViewBag.error = "Email existe déjà !";
                return View();
            }
        }
    }
}