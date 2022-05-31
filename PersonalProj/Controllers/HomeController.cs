using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using PersonalProj.Models;
using PersonalProj.Functions;


namespace PersonalProj.Controllers
{
    [AuthorizationFilter]
    public class HomeController : Controller
    {
        CustomDbContext customDbContext = new CustomDbContext();
        [AuthorizationFilter]
        public ActionResult Index()
        {

            List<Expenses> expenses = customDbContext.Expenses.Take(30).ToList();
            ViewBag.Expenses = expenses;
            ViewBag.Places = customDbContext.places.ToList();
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string UserName , string Password)
        {
            Md5Encrypt md5Encrypt = new Md5Encrypt();
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                ViewBag.LoginErr = "Invalid Login";
                return View("LoginPage");
            }

            var user = customDbContext.Users.Where(x => x.UserName == UserName).FirstOrDefault();
            string PasswordEncoded = md5Encrypt.EncryptPassword(Password);
            if (user == null || PasswordEncoded != user.Password)
            {
                ViewBag.LoginErr = "incorrect UserName or Password";
                return View("LoginPage");
            }
            string UserNameEncrypted = md5Encrypt.EncryptUserName(UserName);

            HttpContext.Session["UserID"] = UserNameEncrypted;
            HttpContext.Session["U"] = user.ID;
            user.LastLogin = DateTime.Now;
            user.UserIpAddress = Request.UserHostAddress;
            History history = new History()
            {
                Date = DateTime.Now,
                Details = "User " + user.UserName + " had logged in. "
            };
            customDbContext.Histories.Add(history);

            customDbContext.SaveChanges();

            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult LoginPage()
        {
            ViewBag.LoginErr = string.Empty;
            return View();
        }

        [AuthorizationFilter]
        public ActionResult Logout()
        {
            try
            {
                string UIDString = HttpContext.Session["U"].ToString();
                int UIDint = int.Parse(UIDString);
                var user = customDbContext.Users.Where(usr => usr.ID == UIDint).FirstOrDefault();
                if(user != null)
                {
                    customDbContext.Histories.Add(new History
                    {
                        Date = DateTime.Now,
                        Details = "User " + user.UserName + "had logged out."
                    });
                    customDbContext.SaveChanges();
                }
                HttpContext.Session["UserID"] = null;
                HttpContext.Session["U"] = null;
            }
            catch (Exception exp)
            {

            }

            return RedirectToAction("LoginPage");
        }

        [AuthorizationFilter]
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