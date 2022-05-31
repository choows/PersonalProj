using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalProj.Models;
namespace PersonalProj.App_Code
{
    public class Functions
    {
        public List<Media> GetMedias(string Path)
        {
            CustomDbContext customDbContext = new CustomDbContext();
            return customDbContext.Medias.Where(x => x.LocalPath == Path).ToList<Media>();
        }
    }
}