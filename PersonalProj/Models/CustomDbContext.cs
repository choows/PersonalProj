using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;

namespace PersonalProj.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(): base("CustomDbContext")
        {
            Database.SetInitializer<CustomDbContext>(
            new DropCreateDatabaseAlways<CustomDbContext>());

        }

        public DbSet<Media> Medias { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Memo> Memos { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<Place> places { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}