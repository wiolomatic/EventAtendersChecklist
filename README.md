# EventAtendersChecklist


Make sure that in Config "connectionStrings" is specified database
<br />
In Package Manager type: 

		Enable-Migrations -ContextTypeName EventAtendersChecklist.Models.ApplicationDbContext -MigrationsDirectory Migrations\ApplicationDbContext
		Enable-Migrations -ContextTypeName EventAtendersChecklist.DAL.eacContext -MigrationsDirectory Migrations\eacContext 
		Add-Migration -ConfigurationTypeName EventAtendersChecklist.Migrations.ApplicationDbContext.Configuration "InitialDatabaseCreation"
		Add-Migration -ConfigurationTypeName EventAtendersChecklist.Migrations.eacContext.Configuration "InitialDatabaseCreation"
		Update-Database -ConfigurationTypeName EventAtendersChecklist.Migrations.ApplicationDbContext.Configuration
		Update-Database -ConfigurationTypeName EventAtendersChecklist.Migrations.eacContext.Configuration