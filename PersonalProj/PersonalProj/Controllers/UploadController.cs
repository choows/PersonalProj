using PersonalProj.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalProj.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UploadNewMedia()
        {
            return View("UploadNewMedia");
        }
        [HttpPost]
        public ActionResult SingleUpload(HttpPostedFileBase file)
        {
            CustomDbContext customDbContext = new CustomDbContext();
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
                Media media = new Media()
                {
                    FileName = FileName,
                    LocalPath = foldername,
                    Type = fileformat[0],
                    UploadDate = DateTime.Now,
                    UploadFileName = file.FileName,
                    UploadUserID = 1
                };
                customDbContext.Medias.Add(media);
                customDbContext.SaveChanges();
                return Json(new { success = true, responseText = "Upload Success" });
            }
            catch (Exception exp)
            {
                return Json(new { success = false, responseText = "Upload Failed" });
            }
        }

    }
}