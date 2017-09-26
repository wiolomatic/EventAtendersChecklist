namespace EventAtendersChecklist.Migrations.eacContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabaseCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionDictionaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ActionGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionDictionaryId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActionDictionaries", t => t.ActionDictionaryId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.ActionDictionaryId)
                .Index(t => t.EventId);

            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.EmployeeEventAssignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionDictionaryId = c.Int(nullable: false),
                        ActionValue = c.Boolean(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActionDictionaries", t => t.ActionDictionaryId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.ActionDictionaryId)
                .Index(t => t.EmployeeId)
                .Index(t => t.EventId);

            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeEventAssignments", "EventId", "dbo.Events");
            DropForeignKey("dbo.EmployeeEventAssignments", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeEventAssignments", "ActionDictionaryId", "dbo.ActionDictionaries");
            DropForeignKey("dbo.ActionGroups", "EventId", "dbo.Events");
            DropForeignKey("dbo.ActionGroups", "ActionDictionaryId", "dbo.ActionDictionaries");
            DropIndex("dbo.EmployeeEventAssignments", new[] { "EventId" });
            DropIndex("dbo.EmployeeEventAssignments", new[] { "EmployeeId" });
            DropIndex("dbo.EmployeeEventAssignments", new[] { "ActionDictionaryId" });
            DropIndex("dbo.ActionGroups", new[] { "EventId" });
            DropIndex("dbo.ActionGroups", new[] { "ActionDictionaryId" });
            DropTable("dbo.Employees");
            DropTable("dbo.EmployeeEventAssignments");
            DropTable("dbo.Events");
            DropTable("dbo.ActionGroups");
            DropTable("dbo.ActionDictionaries");
        }
    }
}
