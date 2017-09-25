# EventAtendersChecklist


Make sure that in Config "connectionStrings" is specified database
<br />
In Package Manager type: 

Enable-Migrations -ContextTypeName EventAtendersChecklist.Models.ApplicationDbContext -MigrationsDirectory Migrations\ApplicationDbContext
		Enable-Migrations -ContextTypeName EventAtendersChecklist.DAL.aecContext -MigrationsDirectory Migrations\eacContext
		Add-Migration -ConfigurationTypeName EventAtendersChecklist.Migrations.ApplicationDbContext.Configuration2 "InitialDatabaseCreation"
		Add-Migration -ConfigurationTypeName EventAtendersChecklist.Migrations.eacContext.Configuration "InitialDatabaseCreation"
		Update-Database -ConfigurationTypeName EventAtendersChecklist.Migrations.ApplicationDbContext.Configuration2
		Update-Database -ConfigurationTypeName EventAtendersChecklist.Migrations.eacContext.Configuration