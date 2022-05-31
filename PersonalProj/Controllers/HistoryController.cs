using PersonalProj.Functions;
using PersonalProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalProj.Controllers
{
    public class HistoryController : Controller
    {
        CheckAuthentication checkAuthentication = new CheckAuthentication();

        // GET: History
        [AuthorizationFilter]
        public ActionResult Index()
        {
            bool isAuthorized = checkAuthentication.CheckAuthorizationBySession();
            if (!isAuthorized)
            {
                return View("../Home/Index");
            }
            return View();
        }

        [AuthorizationFilter]
        public ActionResult RmbDays()
        {
            if (!checkAuthentication.CheckAuthorizationBySession())
            {
                return View("../Home/Index");
            }

            DateTime firstday = new DateTime(2019, 05, 13);
            DateTime today = DateTime.Now;
            double days = (today - firstday).TotalDays;
            ViewBag.TotalDays = Math.Round(days);

            //calculating the next 100 days anniversary
            string daysStr = Math.Round(days).ToString();
            int strLength = daysStr.Length - 1;
            string temp = daysStr[strLength - 1].ToString() + daysStr[strLength].ToString();
            int diff = 100 - int.Parse(temp);
            DateTime NextHundAnni = DateTime.Now.AddDays(diff);
            ViewBag.NextHundDay = Math.Round((NextHundAnni - firstday).TotalDays);      //500 or 600 days
            ViewBag.NextHundDayDiff = diff;     //different days to that day 

            //Next Annual Anniversary
            DateTime NextAnnualAni = DateTime.Now;

            if (today.Month > firstday.Month)
            {
                NextAnnualAni = new DateTime(today.Year + 1, 05, 13);
            }
            else
            {
                NextAnnualAni = new DateTime(today.Year, 05, 13);
            }

            int year = NextAnnualAni.Year - firstday.Year;
            if(year == 1)
            {
                ViewBag.NextAnnualDay = "1st";
            }else if(year == 2)
            {
                ViewBag.NextAnnualDay = "2nd";
            }else if(year == 3)
            {
                ViewBag.NextAnnualDay = "3rd";
            }
            else
            {
                ViewBag.NextAnnualDay = year + "th";
            }

            ViewBag.NextAnnualDay = ViewBag.NextAnnualDay + " Year";
            ViewBag.NextAnnualDiff = Math.Round((NextAnnualAni - today).TotalDays);
            return View();
        }
        [AuthorizationFilter]
        [HttpPost]
        public JsonResult GetHistory(int Page , int PerPage)
        {
            try
            {
                if (!checkAuthentication.CheckAuthorizationBySession())
                {
                    throw new Exception("No Authorize");
                }
                CustomDbContext customDbContext = new CustomDbContext();
                int Bypassed = (Page - 1) * PerPage;
                var histories = customDbContext.Histories.Select(x => new {x.ID, x.Date , x.Details}).ToList().Skip(Bypassed).Take(PerPage);
                return Json(histories, JsonRequestBehavior.AllowGet);
            }
            catch(Exception exp)
            {
                return Json(new List<History>(), JsonRequestBehavior.AllowGet);
            }
        }

     
    }
}