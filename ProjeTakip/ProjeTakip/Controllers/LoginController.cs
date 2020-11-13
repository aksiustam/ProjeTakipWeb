using ProjeTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ProjeTakip.Models.Extended;
using System.Data.Entity;

namespace ProjeTakip.Controllers
{
    public class LoginController : Controller
    {

        private WebProjeDBEntities db = new WebProjeDBEntities();
  
        [HttpGet]
        [Authorize]
        public ActionResult Register()
        {
           
            return View();
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "kul_id,kul_isim,kul_mail,kul_sifre,kul_telefon")] kullanici kullanici)
        {
            bool Status = false;
            string Message = "";

            if (ModelState.IsValid)
            {
                #region //Email
                
                
                var isExist = IsEmailExist(kullanici.kul_mail);
                if(isExist)
                {
                    ModelState.AddModelError("Email", "Email var");
                    return View(kullanici);
                }
                #endregion


                //CODE
                var code = Guid.NewGuid();


                //HASH
                kullanici.kul_sifre = Crypto.Hash(kullanici.kul_sifre);

                //Mail Verify
                


                db.kullanici.Add(kullanici);
                db.SaveChanges();

                

                Status = true;
                ViewBag.Message = Message;
                ViewBag.Status = Status;
                    

            }
            else
            {
                Message = "Invalid Request";
            }




            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Anasayfa");
            }
            else
            {
                return View();
            }
            
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult OgrLogin()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Anasayfa");
            }
            else
            {
                return View();
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin Login , string returnUrl)
        {

            string message = "";


            var v = db.kullanici.Where(a => a.kul_mail == Login.Mail).FirstOrDefault();

            if(v != null)
            {


                if(String.Compare(Crypto.Hash(Login.Pass),v.kul_sifre) == 0)
                {
                    int timeout = Login.RememberMe ? 525600 : 50;

                    var ticket = new FormsAuthenticationTicket(v.kul_isim, Login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);



                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                   && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Anasayfa");
                    }
                    

                }
                else
                {
                    message = "Yanlış Pass";
                }
            }
            else
            {
                message = "Yanlış";
            }

            ViewBag.Message = message;
            
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OgrLogin(UserLogin Login, string returnUrl)
        {

            string message = "";

            var v = db.ogrenci.Where(a => a.ogr_kul == Login.Mail).FirstOrDefault();


            if (v != null)
            {
                if (String.Compare(Crypto.Hash(Login.Pass), v.ogr_sifre) == 0)
                {
                    int timeout = Login.RememberMe ? 525600 : 50;

                    var ticket = new FormsAuthenticationTicket(v.ogr_kul, Login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    if(v.ogr_mail == null || v.ogr_mail == "")
                    {
                        MailEkle me = new MailEkle();
                        me.id = v.ogr_id;
                        me.Mail = "";
                        return View("MailReset", me);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Anasayfa");
                    }

                } 
                else
                {
                    message = "Yanlış Pass";
                }
            }
            else
            {
                message = "Yanlış";
            }



            ViewBag.Message = message;
            return View();
        }
        [Authorize]
        public ActionResult Cikis()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Anasayfa");
        }

        [AllowAnonymous]
        public ActionResult OgrPassReset()
        {

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult OgrPassReset(string OgrKul)
        {

            string message = "";
            

            var data = db.ogrenci.Where(a => a.ogr_kul == OgrKul).FirstOrDefault();
            if (data != null)
            {
                string resetCode = Guid.NewGuid().ToString();

                var controller = DependencyResolver.Current.GetService<ProfilController>();
                controller.ControllerContext = new ControllerContext(Request.RequestContext, controller);

                controller.SendPassEmail(data.ogr_mail, resetCode);

                data.ogr_code = resetCode;
                db.SaveChanges();
                message = "Parola reset linki mailinize gelmiştir!";

            }
            else
            {
                message = "Öğrenci Bulunamadı!";
            }

            ViewBag.Message = message;
            return View();

        }

        [AllowAnonymous]
        public ActionResult KulPassReset()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult KulPassReset(string EmailID)
        {
            

            string message = "";
            

            var data = db.kullanici.Where(a => a.kul_mail == EmailID).FirstOrDefault();
            if (data != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                
                var controller = DependencyResolver.Current.GetService<ProfilController>();
                controller.ControllerContext = new ControllerContext(Request.RequestContext, controller);

                controller.SendPassEmail(data.kul_mail, resetCode);

                data.kul_Code = resetCode;
                db.SaveChanges();
                message = "Parola reset linki mailinize gelmiştir!";

            }
            else
            {
                message = " Mail Bulunamadı!";
            }

            ViewBag.Message = message;
            return View();
            //return RedirectToAction("Kul", "Profil", new { id = data.kul_isim , message = message });


            //controller.SendPassEmail("", "");
        }

        [NonAction]
        public bool IsEmailExist(string id)
        {

            var v = db.kullanici.Where(a => a.kul_mail == id).FirstOrDefault();
            return v != null;
        }

        [HttpPost]
        [Authorize(Roles = "0")]
        public ActionResult MailEkle(MailEkle Me)
        {

            var name = Me.id;
            
            var data = db.ogrenci.Where(x => x.ogr_id == name).FirstOrDefault();
            data.ogr_mail = Me.Mail.ToString();

            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
            
            
           
            return RedirectToAction("Index", "Anasayfa");
            
        }
    }
}