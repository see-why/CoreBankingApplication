namespace CBA.CBAMigration
{
    using CBA.Core;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CBA.Models.CBAContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"CBAMigration";
        }

        protected override void Seed(CBA.Models.CBAContext context)

        {
            context.Branches.Add(
               new Branch
               {
                   Name = "Opebi",
                   DateCreated = DateTime.Now,
                   DateModified = DateTime.Now
               }
               );
            context.SaveChanges();
        }
    }
}
