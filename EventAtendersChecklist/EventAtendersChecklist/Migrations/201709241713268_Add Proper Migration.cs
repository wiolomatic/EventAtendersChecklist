namespace EventAtendersChecklist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProperMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActionDictionaries", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Email", c => c.String());
            AlterColumn("dbo.Employees", "LastName", c => c.String());
            AlterColumn("dbo.Employees", "FirstName", c => c.String());
            AlterColumn("dbo.Events", "Name", c => c.String());
            AlterColumn("dbo.ActionDictionaries", "Name", c => c.String());
        }
    }
}
