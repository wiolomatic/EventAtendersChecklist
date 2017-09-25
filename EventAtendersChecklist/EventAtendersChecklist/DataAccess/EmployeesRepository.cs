using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EventAtendersChecklist.Hub;
using EventAtendersChecklist.Models;

namespace EventAtendersChecklist.DataAccess
{
    public class EmployeesRepository
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        public IEnumerable<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (var connection = new SqlConnection(_connString))
            {
            
            
                connection.Open();
                //using (var command = new SqlCommand(@"SELECT [Id], [FirstName], [LastName], [Email] FROM [eacDB2].[dbo].[Employees]", connection))
                using (var command = new SqlCommand(@"SELECT * FROM [Employees]", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        employees.Add(item: new Employee
                        {
                            Id = (int)reader["Id"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Email = (string)reader["Email"]
                        });
                    }
                }
            }
            return employees;
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {


            //EmployeesHub.SendEmployees();
            //var connection = new SqlConnection(_connString);
            //connection.Open();
            

            if (e.Type == SqlNotificationType.Subscribe)
            {
                
                EmployeesHub.SendEmployees();
            }
            
        }
    }
}