using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonalProj.Models;

namespace PersonalProj
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                {
                    // Don't check for authorization as AllowAnonymous filter is applied to the action or controller  
                    return;
                }
                // Check for authorization  
                if (HttpContext.Current.Session["UserID"] == null || HttpContext.Current.Session["U"] == null)
                {
                    throw new Exception();
                    
                }
                CustomDbContext customDbContext = new CustomDbContext();
                string UserId = HttpContext.Current.Session["U"].ToString();
                int iUserId;
                if (!int.TryParse(UserId, out iUserId))
                {
                    throw new Exception();
                }

                var user = customDbContext.Users.Where(usr => usr.ID == iUserId).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception exp)
            {
                HttpContext.Current.Session["U"] = null;
                HttpContext.Current.Session["UserID"] = null;
                filterContext.Result = new RedirectResult("~/Home/LoginPage");
            }
            
        }
    }
}