using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalProj.Models;
using PersonalProj.Functions;

namespace PersonalProj.DAL
{
    public class CustomDbInitializer: System.Data.Entity.DropCreateDatabaseIfModelChanges<CustomDbContext>
    {
        Md5Encrypt Md5Encrypt = new Md5Encrypt();
        protected override void Seed(CustomDbContext context)
        {
            var users = new List<User>
            {
                new User{UserName=System.Configuration.ConfigurationManager.AppSettings["AdminUserName"], Password =Md5Encrypt.EncryptPassword("526822311129") , UserIpAddress="0.0.0.0" , LastLogin = DateTime.Now }
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }
}