using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProj.Models
{
    public class Media
    {
        public long ID { get; set; }
        public string Type { get; set; }
        public string LocalPath { get; set; }
        public string FileName { get; set; }
        public string UploadFileName { get; set; }
        public DateTime UploadDate { get; set; }
        public int UploadUserID { get; set; }

        public virtual User User { get; set; }
    }
}