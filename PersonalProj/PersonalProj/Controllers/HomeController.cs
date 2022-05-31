using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using PersonalProj.Models;

namespace PersonalProj.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string UserName , string Password)
        {
            string a = "asd";
            string tempUserID = "1";
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                return Json(new { success = false, responseText = "Invalid Login" });
            }
            Session["UserID"] = tempUserID;
            return Json(new { success = true, responseText = "Login Success" });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Logout()
        {
            Session["UserID"] = null;
            return Json(new { success = true, responseText = "Logout Success" });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files)
        {
            List<string> SuccessUpload = new List<string>();
            List<string> FailedUpload = new List<string>();
            if (files.Length == 0)
            {
                return View("Index");
            }
            foreach (var file in files)
            {
                try
                {
                    string[] fileformat = file.ContentType.Split('/');
                    DateTime dateTime = DateTime.Now;
                    string FileName = dateTime.Year.ToString() + "-" + dateTime.Month.ToString() + "-" + dateTime.Day.ToString() + "T" + dateTime.Hour.ToString() + "-" + dateTime.Minute.ToString() + "-" + dateTime.Second.ToString();
                    string[] old_f_name = file.FileName.Split('.');
                    int fileformatidx = old_f_name.Length - 1;
                    FileName = FileName + "." + old_f_name[fileformatidx];

                    string foldername = Server.MapPath("~/MediaUploaded/" + fileformat[0]);
                    bool path_exist = Directory.Exists(foldername);
                    if (!path_exist)
                    {
                        Directory.CreateDirectory(foldername);
                    }
                    string path = Path.Combine(foldername, FileName);
                    file.SaveAs(path);
                    SuccessUpload.Add(file.FileName);
                }
                catch (Exception exp)
                {
                    FailedUpload.Add(file.FileName);
                }
            }
            ViewData["Success"] = SuccessUpload;
            ViewData["Failed"] = FailedUpload;

            return View("Index");

        }
    }
}