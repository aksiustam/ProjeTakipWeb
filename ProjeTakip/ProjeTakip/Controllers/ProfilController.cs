using ProjeTakip.Models;
using ProjeTakip.Models.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

namespace ProjeTakip.Controllers
{
    public class ProfilController : Controller
    {

        private WebProjeDBEntities db = new WebProjeDBEntities();
        // GET: Profil
        //from ogrpro in db.ogrpro
        //                 join dt in
        //                     (
        //                         from proje in db.proje
        //                         where proje.proje_mail == data.kul_mail
        //                         select proje
        //                         )
        //                 on new { ogrpro.ogrpro_proje }
        //                equals new { dt.proje_id }
        //                 select ogrpro;
        public class ProfilModel
        {
            public IEnumerable<ogrpro> list { get; set; }

            public kullanici kul { get; set; }
        }

        [Authorize(Roles = "1")]
        public ActionResult Kul(string id , string message)
        {
            ViewBag.Message = message;
            var data = db.kullanici.Where(x => x.kul_isim == id).SingleOrDefault();
            var list = db.ogrpro.Include(o => o.bolum).Include(o => o.ogrenci).Include(o => o.proje).Include(o => o.tur).Where(x => x.proje.proje_mail == data.kul_mail).ToList();



            ProfilModel pm = new ProfilModel();
            pm.kul = data;
            pm.list = list;
            return View(pm);
        }

        [Authorize(Roles = "0")]
        public ActionResult Ogr(string id)
        {
            var data = db.ogrenci.Where(x => x.ogr_kul == id).SingleOrDefault();
            return View(data);
            
        }

        [Authorize(Roles = "0")]
        [HttpPost]
        public ActionResult ForgotOgrPassword(ogrenci ogr)
        {
            string KulID = ogr.ogr_kul;
            string message = "";
            var data = db.ogrenci.Where(a => a.ogr_kul == KulID).FirstOrDefault(); 
            if (data != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                SendPassEmail(data.ogr_mail, resetCode);

                data.ogr_code = resetCode;
                db.SaveChanges();
                message = "Parola reset linki mailinize gelmiştir!";

            }
            else
            {
                message = " Öğrenci Numarası Bulunamadı!";
            }

            ViewBag.Message = message;
            return View("Ogr", data);
            //return RedirectToAction("Ogr", "Profil", new { id = data.kul_isim , message = message });
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult ForgotPassword(kullanici kul)
        {
            string EmailID = kul.kul_mail;
            string message = "";
            var data = db.kullanici.Where(a => a.kul_mail == EmailID).FirstOrDefault();
            if(data != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                SendPassEmail(data.kul_mail, resetCode);

                data.kul_Code = resetCode;  
                db.SaveChanges();
                message = "Parola reset linki mailinize gelmiştir!";
               
            }
            else
            {
                message = " Mail Bulunamadı!";
            }

            ViewBag.Message = message;
            return View("Kul", data);
            //return RedirectToAction("Kul", "Profil", new { id = data.kul_isim , message = message });
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string id)
        {
            //var action = RouteData.Values["action"].ToString();
            

            var data = db.kullanici.Where(a => a.kul_Code == id).FirstOrDefault();
            var ogrdata = db.ogrenci.Where(a => a.ogr_code == id).FirstOrDefault();

            if(data != null){
                PassResetModel pass = new PassResetModel();
                pass.ResetCode = id;
                return View(pass);
            }
            else if (ogrdata != null)
            {
                PassResetModel pass = new PassResetModel();
                pass.ResetCode = id;
                return View(pass);
            }
            else
            {
                return HttpNotFound();
            }

            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(PassResetModel model)
        {
            var message = "";
            var data = db.kullanici.Where(a => a.kul_Code == model.ResetCode).FirstOrDefault();
            var data2 = db.ogrenci.Where(a => a.ogr_code == model.ResetCode).FirstOrDefault();
            if (data != null)
            {
                data.kul_sifre = Crypto.Hash(model.NewPass);
                data.kul_Code = "";
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                message = "Yeni Parola oluşturuldu!";

            }
            else if(data2 != null)
            {
                data2.ogr_sifre = Crypto.Hash(model.NewPass);
                data2.ogr_code = "";
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                message = "Yeni Parola oluşturuldu!";
            }
            else
            {
                message = "Bulunamadı!";
            }

            ViewBag.Message = message;
            //return View("Index","Index");
            return RedirectToAction("Index", "Anasayfa", null);
        }


        [NonAction]
        public void SendPassEmail(string emailID,string activationCode)
        {
            var verifyUrl = "/ResetPassword/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("aksiustam@gmail.com", "ProjeTakip!");
            var toEmail = new MailAddress(emailID);
            var fromEmailPass = "************";
            string subject = "ProjeTakip Parola resetleme! ";

            string body = "Merhaba,<br/><br/>Parolanızı resetlemek için bu linke tıklayınız<br/><br/><a href="+link+">Reset Password link </a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPass)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })
                smtp.Send(message);
        }
    }
}