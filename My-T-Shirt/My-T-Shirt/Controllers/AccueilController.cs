using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
using My_T_Shirt.Models;

namespace My_T_Shirt.Controllers
{
    public class AccueilController : Controller
    {
        // GET: Accueil
        public ActionResult Index()
        {

            try
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                    d.cn.Close();

                ViewBag.Exception = "";
                List<Produit> lstProduits = new List<Produit>();
                SqlCommand cmd = new SqlCommand("select top 12 * from produit", d.cn);
                d.cn.Open();
                SqlDataReader dataR = cmd.ExecuteReader();
                while (dataR.Read())
                {
                    Produit produit = new Produit();
                    produit.IdProduit = dataR[0].ToString();
                    produit.Nom = dataR[1].ToString();
                    produit.Description = dataR[2].ToString();
                 
                    produit.Genre = dataR[3].ToString();
                    produit.Prix = float.Parse(dataR[4].ToString());
                    produit.Image = dataR[5].ToString();
                    
                    produit.idMarque = dataR[6].ToString();
                    lstProduits.Add(produit);
                }
                dataR.Close();
                d.cn.Close();
                ViewData["apercuRapide"] = lstProduits;
                return View();

            }
            catch (Exception ex)
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                {
                    d.cn.Close();
                    ViewBag.Exception = "";

                }
                else
                {
                    ViewBag.Exception = ex.Message;
                }
                return View();
            }
        }
    }
}