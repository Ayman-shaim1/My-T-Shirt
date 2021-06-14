using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using My_T_Shirt.Models;
using PagedList;
namespace My_T_Shirt.Controllers
{
    public class ProduitsController : Controller
    {
        // GET: Produits
        public ActionResult Index(int? page, string recherche = "", string genre = "")
        {
            if (d.cn.State == System.Data.ConnectionState.Open)
                d.cn.Close();
            if (recherche != "")
            {
                ViewBag.recherche = recherche;
            }
            if (genre != "")
            {
                ViewBag.genre = genre;
            }
            const int pageSize = 16;
            int pageNumbre = page ?? 1;
            return View(Produit.Affcher(recherche, genre).ToPagedList(pageNumbre, pageSize));


        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult RenderSoldeProduits()
        {
            List<Produit> lstProduits = new List<Produit>();
            SqlCommand cmd = new SqlCommand("select top 10 p.* from produit p inner join solde s on p.idProduit = s.idProduit", d.cn);
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
            ViewData["soldeProduits"] = lstProduits;
            return PartialView("_SoldeProduits");
        }
       
    }
}