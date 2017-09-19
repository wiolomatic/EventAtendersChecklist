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
                        ActionNames_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActionNames", t => t.ActionNames_Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.ActionNames_Id);
            
            CreateTable(
                "dbo.ActionNames",
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
                        ActionNames_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActionNames", t => t.ActionNames_Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.EventId)
                .Index(t => t.ActionNames_Id);
            
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
            DropForeignKey("dbo.EmployeeEventAssignments", "ActionNames_Id", "dbo.ActionNames");
            DropForeignKey("dbo.ActionGroups", "ActionNames_Id", "dbo.ActionNames");
            DropIndex("dbo.EmployeeEventAssignments", new[] { "ActionNames_Id" });
            DropIndex("dbo.EmployeeEventAssignments", new[] { "EventId" });
            DropIndex("dbo.EmployeeEventAssignments", new[] { "EmployeeId" });
            DropIndex("dbo.ActionGroups", new[] { "ActionNames_Id" });
            DropIndex("dbo.ActionGroups", new[] { "EventId" });
            DropTable("dbo.Events");
            DropTable("dbo.Employees");
            DropTable("dbo.EmployeeEventAssignments");
            DropTable("dbo.ActionNames");
            DropTable("dbo.ActionGroups");
        }
    }
}
