using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProj.Models
{
    public class Memo
    {
        public int ID { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string Memo_Details { get; set; }
        public string Status { get; set; }
        public string TextColor { get; set; }
        public string BackgroundColro { get; set; }
        public int Priority { get; set; }
        //1 to 10 
    }
}