using PersonalProj.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalProj.Controllers
{
    [AuthorizationFilter]
    public class UploadController : Controller
    {
        // GET: Upload
        [AuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizationFilter]
        public ActionResult UploadNewMedia()
        {
            return View("UploadNewMedia");
        }

        [AuthorizationFilter]
        [HttpPost]
        public ActionResult SingleUpload(HttpPostedFileBase file , string U_ID , string path)
        {
            CustomDbContext customDbContext = new CustomDbContext();
            try
            {
                
                string[] fileformat = file.ContentType.Split('/');
                int User_ID = int.Parse(U_ID);
                DateTime dateTime = DateTime.Now;

                /*
                 * check for user exist first 
                 */

                var user = customDbContext.Users.Where(u => u.ID == User_ID).FirstOrDefault();
                if(user == null)
                {
                    throw new Exception("User Not Found Exception ");
                }


                string foldername = Server.MapPath("~/MediaUploaded/" + path + "/");
                bool path_exist = Directory.Exists(foldername);
                if (!path_exist)
                {
                    Directory.CreateDirectory(foldername);
                }
               // string path = Path.Combine(foldername, file.FileName);
                file.SaveAs(foldername+ file.FileName);
                Media media = new Media()
                {
                    FileName = file.FileName,
                    LocalPath = path,
                    Type = fileformat[0],
                    UploadDate = DateTime.Now,
                    UploadUserID = User_ID
                };
                customDbContext.Medias.Add(media);

                History history = new History()
                {
                    Date = DateTime.Now,
                    Details = user.UserName + " had added new file => " + media.FileName + " , on path => " + media.LocalPath
                };
                customDbContext.Histories.Add(history);
                customDbContext.SaveChanges();
                return Json(new { success = true, responseText = "Upload Success" });
            }
            catch (Exception exp)
            {
                return Json(new { success = false, responseText = exp });
            }
        }

        [AuthorizationFilter]
        [HttpPost]
        public JsonResult CreateNewFolder(string FolderName , string ParentPath)
        {
            try
            {
                CustomDbContext customDbContext = new CustomDbContext();
                var file_name = customDbContext.Medias.Where(x => x.FileName == FolderName && x.Type == "Folder").FirstOrDefault();
                string userID = Session["UserID"].ToString();
                string UserName = Session["U"].ToString();
                var user = customDbContext.Users.FirstOrDefault(x => x.ID.ToString() == UserName);
                if (file_name == null)
                {
                    Media media = new Media()
                    {
                        FileName = FolderName,
                        LocalPath = ParentPath,
                        Type = "Folder",
                        UploadDate = DateTime.Now,
                        UploadUserID = user.ID
                    };
                    customDbContext.Medias.Add(media);
                    customDbContext.SaveChanges();
                    return Json(new { success = true, responseText = "Folder Created." });
                }
                else
                {
                    return Json(new { success = false, responseText = "Folder existed." });
                }
            }
            catch(Exception exp)
            {
                return Json(new { success = false, responseText = "Upload Failed : " + exp });
            }
        }

        [AuthorizationFilter]
        [HttpPost]
        public JsonResult RemoveFile(string filepath , string FileName)
        {
            try
            {
                CustomDbContext customDbContext = new CustomDbContext();
                var file = customDbContext.Medias.Where(x => x.FileName == FileName).FirstOrDefault();
                string path = Request.MapPath(filepath);
                if (file != null && System.IO.File.Exists(path))
                {
                    customDbContext.Medias.Remove(file);
                    customDbContext.SaveChanges();
                    System.IO.File.Delete(path);
                    return Json(new { success = true, responseText = "file removed." });
                }
                return Json(new { success = false, responseText = "file not found." });
            }catch(Exception exp)
            {
                return Json(new { success = false , responseText="error removing file." });
            }
            
        }

        [AuthorizationFilter]
        [HttpGet]
        public JsonResult GetMediaType()
        {
            CustomDbContext customDbContext = new CustomDbContext();

            var result = (from m in customDbContext.Medias group m by m.Type).Select(x => x.Key).ToList();
            var groupedres = (from m in customDbContext.Medias group m by m.Type).ToList();
            List<string> MediaType = new List<string>();
            foreach (var g in groupedres)
            {
                MediaType.Add(g.Key);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthorizationFilter]
        [HttpPost]
        public JsonResult GetMedias(string MediaType , int Page , int PerPage)
        {
            int Bypassed = (Page - 1) * PerPage;
            CustomDbContext customDbContext = new CustomDbContext();
            if(MediaType.ToLower() == "image" || MediaType.ToLower() == "video" || MediaType.ToLower() == "application")
            {
                var result = customDbContext.Medias.Where(x => x.Type.ToLower() == MediaType.ToLower()).Select(x => new { x.FileName, x.LocalPath, x.UploadDate }).ToList().Skip(Bypassed).Take(PerPage);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = customDbContext.Medias.Where(x => x.Type.ToLower() != "image" &&  x.Type.ToLower() != "video" && x.Type != "application").Select(x => new { x.FileName, x.LocalPath, x.UploadDate }).ToList().Skip(Bypassed).Take(PerPage);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizationFilter]
        public ActionResult ShowAllContent(string id)
        {
            id = id.Trim().ToLower();
            if(id != "image" && id != "video" && id != "application" && id != "others")
            {
                return View("Index");
            }
            ViewBag.MediaType = id;
            return View();
        }

        [AuthorizationFilter]
        [HttpPost]
        public JsonResult AddNewExpenses(string Datetime, string L, string C, string L_Paid, string C_Paid, string place , string head_detail , string tail_detail)
        {
            try
            {
                //2021-06-16
                if (string.IsNullOrEmpty(head_detail) || string.IsNullOrEmpty(tail_detail) || string.IsNullOrEmpty(Datetime) || string.IsNullOrEmpty(L) || string.IsNullOrEmpty(C) || string.IsNullOrEmpty(L_Paid) || string.IsNullOrEmpty(C_Paid) || string.IsNullOrEmpty(place))
                {
                    return Json(new { status = "failed", text = "Invalid input" });
                }

                CustomDbContext customDbContext = new CustomDbContext();
                var place_found = customDbContext.places.Where(x => x.Name == place).FirstOrDefault();
                if (place_found == null)
                {
                    customDbContext.places.Add(new Place()
                    {
                        ID = new Guid(),
                        Name = place
                    });
                    customDbContext.SaveChanges();
                    place_found = customDbContext.places.Where(x => x.Name == place).FirstOrDefault();
                }
                string UserName = Session["U"].ToString();
                var user = customDbContext.Users.FirstOrDefault(x => x.ID.ToString() == UserName);
                customDbContext.Expenses.Add(new Expenses()
                {
                    Sum = decimal.Parse(L_Paid) + decimal.Parse(C_Paid),
                    Choo = decimal.Parse(C),
                    Leow = decimal.Parse(L),
                    LeowPaid = decimal.Parse(L_Paid),
                    ChooPaid = decimal.Parse(C_Paid),
                    ExpensesDateTime = DateTime.Parse(Datetime),
                    Place = place_found,
                    UpdateDateTime = DateTime.Now,
                    UploadUser = user,
                    description_head = head_detail,
                    description_tail = tail_detail
                });
                customDbContext.SaveChanges();
                return Json(new { status = 200, text = "success" });
            }
            catch(Exception exp)
            {
                return Json(new { status = 400, text = "failed" });
            }
            }
        
    }
}