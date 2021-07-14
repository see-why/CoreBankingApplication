namespace CBA.CBAMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sucker : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Branches", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Branches", "Name", c => c.String(nullable: false));
        }
    }
}
