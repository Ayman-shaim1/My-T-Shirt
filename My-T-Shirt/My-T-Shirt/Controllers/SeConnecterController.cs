using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using My_T_Shirt.Helpers;
using My_T_Shirt.Models;

namespace My_T_Shirt.Controllers
{
    public class SeConnecterController : Controller
    {
        // GET: SeConnecter
        public ActionResult Index()
        {
            if (Authentication.isConnected())
            {
                return RedirectToAction("", "Accueil");
            }
            else
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();
                ViewBag.error = "";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(Utilisateur utilisateur)
        {


            bool isConnected = Authentication.SeConnecter(utilisateur.Email, utilisateur.MotDePasse, false);
            if (isConnected)
            {
                return RedirectToAction("", "Accueil");
            }
            else
            {
                ViewBag.error = "Email ou mot de passe incorrecte !";
                return View();
            }

        }
    }
}