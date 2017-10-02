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


Visual Studio Community (https://www.visualstudio.com/pl/downloads/?rr=https%3A%2F%2Fwww.google.pl%2F)
MS Sql Server express edition (https://www.microsoft.com/pl-pl/sql-server/sql-server-editions-express)
SQL server management studio (https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) 