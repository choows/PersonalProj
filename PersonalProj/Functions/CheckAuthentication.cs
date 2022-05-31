using PersonalProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProj.Functions
{
    public class CheckAuthentication
    {
        public bool VerificationByToken(string input_Token)
        {
            string Token = System.Configuration.ConfigurationManager.AppSettings["Token"];
            if (string.Compare(input_Token, Token) == 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckAuthorizationBySession()
        {
            try
            {
                CustomDbContext customDbContext = new CustomDbContext();
                int CurrentLoginUserID = int.Parse(HttpContext.Current.Session["U"].ToString());
                var user = customDbContext.Users.Where(usr => usr.ID == CurrentLoginUserID).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                if (user.UserName == System.Configuration.ConfigurationManager.AppSettings["AdminUserName"])
                {
                    return true;
                }
                return false;
            }
            catch (Exception exp)
            {
                return false;
            }
        }
    }
}