using LinqToExcel;
using LinqToExcel.Domain;
using ProjeTakip.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjeTakip.Controllers
{
    public class ExcelController : Controller
    {
        // GET: Excel
        private WebProjeDBEntities db = new WebProjeDBEntities();

        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            
            return View();
        }

        [Authorize(Roles = "1")]
        public ActionResult Ogr()
        {
            
            var products = db.ogrenci.ToList();

            var grid = new GridView();
            grid.DataSource = from p in products
                              select p;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Ogr.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());



            Response.Charset = "UTF-8";
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
            Response.Write("şŞğĞİıiçÇöÖüÜ");
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }
        


        [Authorize(Roles = "1")]
        public ActionResult Kul()
        {
            var products = db.kullanici.ToList();

            var grid = new GridView();
            grid.DataSource = from p in products
                              select p;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Kul.xls");
            Response.ContentType = "application/vnd.ms-excel";


            Response.Charset = "UTF-8";
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
            Response.Write("şŞğĞİıiçÇöÖüÜ");
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }


        [Authorize(Roles = "1")]
        public ActionResult Pro()
        {
            var products = db.proje.ToList();

            var grid = new GridView();
            grid.DataSource = from p in products
                              select p;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Proje.xls");
            Response.ContentType = "application/vnd.ms-excel";

            Response.Charset = "UTF-8";
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
            Response.Write("şŞğĞİıiçÇöÖüÜ");
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "1")]
        public ActionResult Ekle(HttpPostedFileBase FileUpload, FormCollection fc)
        {

            List<string> message = new List<string>();
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {

                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Doc/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);

                    var workNames = excelFile.GetWorksheetNames().ToList();
                    var workName = workNames.FirstOrDefault();


                    //var value = from a in excelFile.WorksheetNoHeader(workName) select a;
                    //return Json(data.ToList(), JsonRequestBehavior.AllowGet);

                    string key = fc["btn"];

                    message.Add(key);
                    switch (key)
                    {
                        case "ogrenci":
                            {
                                var data = from a in excelFile.Worksheet<ogrenci>(workName) select a;
                                #region ogr
                                foreach (var a in data)
                                {
                                    try
                                    {

                                        if (a.ogr_kul != null && a.ogr_sifre != null)
                                        {
                                            var user = db.ogrenci.Where(s => s.ogr_kul == a.ogr_kul).FirstOrDefault();
                                            if (user != null)
                                            {

                                                message.Add(a.ogr_kul + " - adı kullanıldı!");

                                            }
                                            else
                                            {
                                                ogrenci TU = new ogrenci();
                                                TU = a;
                                                TU.ogr_yetki = 0;
                                                db.ogrenci.Add(TU);
                                                db.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            string not = "";
                                            if (a.ogr_kul == "" || a.ogr_kul == null) not += "Öğrenci No ";
                                            if (a.ogr_sifre == "" || a.ogr_sifre == null) not += "Şifre ";
                                            not += "Gereklidir.";
                                            message.Add(not);
                                            message.ToArray();

                                        }
                                    }

                                    catch (DbEntityValidationException ex)
                                    {
                                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                        {

                                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                                            {

                                                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                            }

                                        }
                                    }
                                }
                                #endregion

                                break;
                            }
                        case "kullanici":
                            {
                                var data = from a in excelFile.Worksheet<kullanici>(workName) select a;

                                #region kul
                                foreach (var a in data)
                                {
                                    try
                                    {

                                        if (a.kul_mail != null && a.kul_sifre != null)
                                        {
                                            var user = db.kullanici.Where(s => s.kul_mail == a.kul_mail).FirstOrDefault();

                                            if (user != null)
                                            {

                                                message.Add(a.kul_mail + " - adı kullanıldı!");

                                            }
                                            else
                                            {
                                                kullanici TU = new kullanici();
                                                TU = a;
                                                TU.kul_yetki = 1;
                                                db.kullanici.Add(TU);
                                                db.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            string not = "";
                                            if (a.kul_mail == "" || a.kul_mail == null) not += "Kullanıcı Mail ";
                                            if (a.kul_sifre == "" || a.kul_sifre == null) not += "Şifre ";
                                            not += "Gereklidir.";
                                            message.Add(not);
                                            message.ToArray();

                                        }
                                    }

                                    catch (DbEntityValidationException ex)
                                    {
                                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                        {

                                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                                            {

                                                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                            }

                                        }
                                    }
                                }
                                #endregion
                                break;
                            }
                        case "proje":
                            {
                                var data = from a in excelFile.Worksheet<proje>(workName) select a;

                                #region proje
                                foreach (var a in data)
                                {
                                    try
                                    {

                                        if (a.proje_isim != null && a.proje_mail != null)
                                        {
                                            var user = db.proje.Where(s => s.proje_isim == a.proje_isim).FirstOrDefault();


                                            if (user != null)
                                            {

                                                message.Add(a.proje_isim + " - adı kullanıldı!");

                                            }
                                            else
                                            {
                                                proje TU = new proje();
                                                TU = a;
                                                db.proje.Add(TU);
                                                db.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            string not = "";
                                            if (a.proje_isim == "" || a.proje_isim == null) not += "Proje İsmi ";
                                            if (a.proje_mail == "" || a.proje_mail == null) not += "Proje Mail ";
                                            not += "Gereklidir.";
                                            message.Add(not);
                                            message.ToArray();

                                        }
                                    }

                                    catch (DbEntityValidationException ex)
                                    {
                                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                        {

                                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                                            {

                                                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                            }

                                        }
                                    }
                                }
                                #endregion

                                break;
                            }



                    }

                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }


                    message.Add("Başarılı!");
                    message.ToArray();

                    return PartialView("Sonuc", message);

                    //return Content(message.ToList());
                    //return Json(message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //alert message for invalid file format  
                    message.Add("Only Excel file format is allowed");
                    message.ToArray();
                    return PartialView("Sonuc", message);
                }
            }
            else
            {

                if (FileUpload == null) message.Add("Please choose Excel file");
                message.ToArray();
                return PartialView("Sonuc", message);
            }
        }


        
    }
}