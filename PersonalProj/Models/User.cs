using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProj.Models
{
    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime LastLogin { get; set; }
        public string UserIpAddress { get; set; }

        //relationship 
        public virtual ICollection<Media> Medias { get; set; }

    }
}