using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;




using System.Data.Entity;
using EnCoOrszag.Models.DataAccess.Entities;

namespace EnCoOrszag.Models.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false){

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

       // public DbSet<Army> Armies { get; set; }
        public DbSet<Blueprints> Blueprints { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Construction> Constructions { get; set; }
        public DbSet<Country> Countries { get; set; }
        //public DbSet<Group> Groups { get; set; }
        public DbSet<Research> Researches { get; set; }
        public DbSet<Researching> Researching { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<Game> Game { get; set; }
    }
}