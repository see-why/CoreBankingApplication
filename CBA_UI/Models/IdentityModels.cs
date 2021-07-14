using CBA.Core;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace CBA.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    //{
    //    public virtual string FirstName { get; set; }
    //    public virtual string LastName { get; set; }       
    //    public virtual string OtherNames { get; set; }      
    //    public virtual string PhoneNumber { get; set; }
    //    public virtual string Email { get; set; }
    //    public virtual Branch Branch { get; set; }
    //    public virtual bool IsSuperAdmin { get; set; }

    //}

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("connectionString")
        {
        }
        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
           // Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }   

    }
    public class CBAContext : DbContext
    {
        public CBAContext()
            : base("connectionString")
        {
        }
        public DbSet<Branch> Branches { get; set; }
        static CBAContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            //Database.SetInitializer<CBAContext>(new CBADbInitializer());
        }

        public static CBAContext Create()
        {
            return new CBAContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }

    }
}