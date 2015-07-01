using System.Data.Entity;
using System.Security.Claims;
using System.Collections.Generic;

using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


using EnCoOrszag.Models.DataAccess.Entities;

namespace EnCoOrszag.Models
{
    //TODO: application user kulon osztaly entitiesben

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //TODO: DB context a data accesben
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
          
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }



        
        public DbSet<Army> Armies { get; set; }
        public DbSet<Blueprints> Blueprints { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Construction> Constructions { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Research> Researches { get; set; }
        public DbSet<Researching> Researching { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; } 
    }
}