using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalProj.Models;

namespace PersonalProj.DAL
{
    public class CustomDbInitializer: System.Data.Entity.DropCreateDatabaseIfModelChanges<CustomDbContext>
    {
        protected override void Seed(CustomDbContext context)
        {
            
            var users = new List<User>
            {
                new User{UserName="chooweisung@gmail.com", Password="526822311129" , UserIpAddress="0.0.0.0" , LastLogin = DateTime.Now }
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }
}