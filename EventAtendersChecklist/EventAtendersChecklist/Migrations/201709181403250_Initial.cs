namespace EventAtendersChecklist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Actions", t => t.ActionId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.ActionId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Actions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeEventAssignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionId = c.Int(nullable: false),
                        ActionValue = c.Boolean(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Actions", t => t.ActionId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.ActionId)
                .Index(t => t.EmployeeId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeEventAssignments", "EventId", "dbo.Events");
            DropForeignKey("dbo.ActionGroups", "EventId", "dbo.Events");
            DropForeignKey("dbo.EmployeeEventAssignments", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeEventAssignments", "ActionId", "dbo.Actions");
            DropForeignKey("dbo.ActionGroups", "ActionId", "dbo.Actions");
            DropIndex("dbo.EmployeeEventAssignments", new[] { "EventId" });
            DropIndex("dbo.EmployeeEventAssignments", new[] { "EmployeeId" });
            DropIndex("dbo.EmployeeEventAssignments", new[] { "ActionId" });
            DropIndex("dbo.ActionGroups", new[] { "EventId" });
            DropIndex("dbo.ActionGroups", new[] { "ActionId" });
            DropTable("dbo.Events");
            DropTable("dbo.Employees");
            DropTable("dbo.EmployeeEventAssignments");
            DropTable("dbo.Actions");
            DropTable("dbo.ActionGroups");
        }
    }
}
