using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalProj.Models
{
    public class Expenses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public DateTime ExpensesDateTime { get; set; }
        public Place Place { get; set; }
        public decimal Sum { get; set; }
        public decimal? Leow { get; set; }
        public decimal? Choo { get; set; }
        public decimal? LeowPaid { get; set; }
        public decimal? ChooPaid { get; set; }
        public string ImagePath { get; set; }
        public string description_head { get; set; }
        public string description_tail { get; set; }
        public virtual User UploadUser { get; set; }
    }
}