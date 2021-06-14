using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace My_T_Shirt.Helpers
{
    public class Authentication
    {
        public static bool SeConnecter(string Email, string MotDePasse, bool Admin)
        {
            try
            {
                if (!Admin)
                {
                    string BD_motdepasse = "";
                    string req1 = string.Format("select motdepasse from utilisateur where Email = '{0}' and isAdmin = 'False'", Email.ToLower());
                    SqlCommand cmd = new SqlCommand(req1, d.cn);

                    d.cn.Open();
                    BD_motdepasse = cmd.ExecuteScalar().ToString();
                    d.cn.Close();

                    if (MotDePasse == Cryptage.Decrypt(BD_motdepasse))
                    {
                        string req2 = string.Format("select * from utilisateur where Email = '{0}' ", Email);
                        SqlCommand cmd2 = new SqlCommand(req2, d.cn);
                        d.cn.Open();
                        SqlDataReader dataR = cmd2.ExecuteReader();
                        if (dataR.Read())
                        {
                            HttpCookie cookie = new HttpCookie("Login");
                            cookie["idUtilisateur"] = dataR[0].ToString();
                            cookie["Nom"] = dataR[1].ToString();
                            cookie["Prenom"] = dataR[2].ToString();
                            cookie["Email"] = dataR[3].ToString();
                            cookie.Expires = DateTime.Now.AddDays(1);
                            HttpContext.Current.Response.SetCookie(cookie);
                        }
                        d.cn.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    bool mdp_correct = false;
                    string BD_motdepasse = "";
                    string req1 = string.Format("select motdepasse from Utilisateur where email = '{0}' and isAdmin = 'True' ", Email);
                    SqlCommand cmd = new SqlCommand(req1, d.cn);
                    d.cn.Open();
                    if (cmd.ExecuteScalar() != null)
                    {
                        BD_motdepasse = cmd.ExecuteScalar().ToString();


                        if (MotDePasse == Cryptage.Decrypt(BD_motdepasse))
                        {
                            mdp_correct = true;
                            string req2 = string.Format("select * from Utilisateur where email = '{0}' ", Email);
                            SqlCommand cmd2 = new SqlCommand(req2, d.cn);

                            SqlDataReader dataR = cmd2.ExecuteReader();
                            if (dataR.Read())
                            {
                                HttpCookie cookie = new HttpCookie("Admin_Login");
                                cookie["idUtilisateur"] = dataR[0].ToString();
                                cookie["Nom"] = dataR[1].ToString();
                                cookie["Prenom"] = dataR[2].ToString();
                                cookie["Email"] = dataR[3].ToString();
                                cookie.Expires = DateTime.Now.AddDays(4);
                                HttpContext.Current.Response.SetCookie(cookie);
                            }
                        }
                        d.cn.Close();
                        if (mdp_correct == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (d.cn.State == System.Data.ConnectionState.Open)
                {
                    d.cn.Close();
                }
                return false;
            }
        }
        public static bool isConnected()
        {
            if (HttpContext.Current.Request.Cookies["Login"] != null)
                return true;
            else
                return false;
        }
        public static bool isAdmin_Connected()
        {
            if (HttpContext.Current.Request.Cookies["Admin_Login"] != null)
                return true;
            else
                return false;
        }
        public static void seDeconnecter(bool admin)
        {
            if(admin)
            {
                if (HttpContext.Current.Request.Cookies["Admin_Login"] != null)
                {
                    HttpCookie cookie = new HttpCookie("Admin_Login");
                    cookie.Expires = DateTime.Now.AddHours(-1);
                    HttpContext.Current.Response.SetCookie(cookie);
                }
            }
            else
            {
                if (HttpContext.Current.Request.Cookies["Login"] != null)
                {
                    HttpCookie cookie = new HttpCookie("Login");
                    cookie.Expires = DateTime.Now.AddHours(-1);
                    HttpContext.Current.Response.SetCookie(cookie);
                }
            }
        }

        public static string getNom(bool admin)
        {
            if (admin)
            {
                if (HttpContext.Current.Request.Cookies["Admin_Login"] != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Admin_Login"];
                    return cookie.Values["Nom"].ToString();
                }
            }
            else
            {
                if (HttpContext.Current.Request.Cookies["Login"] != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Login"];
                    return cookie.Values["Nom"].ToString();
                }
            }
            return "";
        }

        public static string getPrenom(bool admin)
        {
            if (admin)
            {
                if (HttpContext.Current.Request.Cookies["Admin_Login"] != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Admin_Login"];
                    return cookie.Values["Prenom"].ToString();
                }
            }
            else
            {
                if (HttpContext.Current.Request.Cookies["Login"] != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Login"];
                    return cookie.Values["Prenom"].ToString();
                }
            }
            return "";
        }
        public static string getEmail(bool admin)
        {
            if (admin)
            {
                if (HttpContext.Current.Request.Cookies["Admin_Login"] != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Admin_Login"];
                    return cookie.Values["Email"].ToString();
                }
            }
            else
            {
                if (HttpContext.Current.Request.Cookies["Login"] != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Login"];
                    return cookie.Values["Email"].ToString();
                }
            }
            return "";
        }

        public static string getId(bool admin)
        {
            if (admin)
            {
                if (HttpContext.Current.Request.Cookies["Admin_Login"] != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Admin_Login"];
                    return cookie.Values["idUtilisateur"].ToString();
                }
            }
            else
            {
                if (HttpContext.Current.Request.Cookies["Login"] != null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Login"];
                    return cookie.Values["idUtilisateur"].ToString();
                }
            }
            return "";
        }
    }
}