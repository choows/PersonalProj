using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonalProj.Models;
using PersonalProj.Functions;

namespace PersonalProj.Controllers
{
    public class SettingController : Controller
    {
        // GET: Setting
        //public ActionResult Index()
        //{
        //    return View();
        //}
        CheckAuthentication checkAuthentication = new CheckAuthentication();
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SetupNewUser(string Token , string UserName , string UserPassword)
        {
            try
            {
                if (!checkAuthentication.VerificationByToken(Token))
                {
                    throw new Exception("Invalid Action");
                }
                Md5Encrypt md5Encrypt = new Md5Encrypt();
                CustomDbContext customDbContext = new CustomDbContext();
                User user = new User();
                user.LastLogin = DateTime.Now;
                user.UserIpAddress = "0.0.0.0";
                user.UserName = UserName;
                user.Password = md5Encrypt.EncryptPassword(UserPassword);
                var old_user = customDbContext.Users.Where(usr => usr.UserName == UserName).FirstOrDefault();
                if(old_user == null)
                {
                    customDbContext.Users.Add(user);
                    customDbContext.SaveChanges();
                    return Json(new { status = 200, textResult = "Done Add New User" });
                }
                return Json(new {status = 202 , textResult = "Failed" });

            }catch(Exception exp)
            {
                return Json(new { status = 202, textResult = "failed" , Excep = exp });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult UpdatePassword(string Token , string Old_Pass , string New_Pass , string User_Name)
        {
            try
            {
                CustomDbContext customDbContext = new CustomDbContext();
                Md5Encrypt md5Encrypt = new Md5Encrypt();
                if (!checkAuthentication.VerificationByToken(Token))
                {
                    throw new Exception("Invalid Action");
                }

                var selected_user = customDbContext.Users.Where(usr => usr.UserName == User_Name).FirstOrDefault();
                if(selected_user == null)
                {
                    throw new Exception("User Not Found");
                }
                string new_pass_enc = md5Encrypt.EncryptPassword(New_Pass);
                if(selected_user.Password != md5Encrypt.EncryptPassword(Old_Pass))
                {
                    throw new Exception("Incorrect Password");
                }
                selected_user.Password = new_pass_enc;
                customDbContext.SaveChanges();
                return Json(new { status = 200, textResult = "Done Update Password" });
            }
            catch(Exception exp)
            {
                return Json(new { status = 202, textResult = "failed", Excep = exp });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult RemoveUser(string Token , string UserName)
        {
            try
            {
                CustomDbContext customDbContext = new CustomDbContext();
                if (!checkAuthentication.VerificationByToken(Token))
                {
                    throw new Exception("Invalid Action");
                }
                var selected_usr = customDbContext.Users.Where(usr => usr.UserName == UserName && usr.UserName != System.Configuration.ConfigurationManager.AppSettings["AdminUserName"]).FirstOrDefault();
                if(selected_usr == null)
                {
                    throw new Exception("User Not Found");
                }
                customDbContext.Users.Remove(selected_usr);
                customDbContext.SaveChanges();
                return Json(new { status = 200, textResult = "Done Remove User" + UserName });
            }
            catch(Exception exp)
            {
                return Json(new { status = 202, textResult = "failed", Excep = exp });
            }
        }
    }
}