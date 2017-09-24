namespace EventAtendersChecklist.ModelsView
{
    using EventAtendersChecklist.Models;
    using OfficeOpenXml;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="EmployeeToDatabase" />
    /// </summary>
    public static class EmployeeToDatabase
    {
        /// <summary>
        /// The ToDataBase
        /// </summary>
        /// <param name="package">The <see cref="ExcelPackage"/></param>
        /// <returns>The <see cref="List{Employee}"/></returns>
        public static ListOfAttendeesFromExcel ToDataBase(this ExcelPackage package)
        {
            ListOfAttendeesFromExcel loafe = new ListOfAttendeesFromExcel();
            List<Employee> listOfEmployee = new List<Employee>();
            List<ActionDictionary> listOfCheckBoxNames = new List<ActionDictionary>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

            var start = workSheet.Dimension.Start;
            var end = workSheet.Dimension.End;
            for (int row = start.Row; row <= start.Row; row++)
            {
                for (int col = start.Column + 3; col <= end.Column; col++)
                { // ... Cell by cell...
                    string cellValue = workSheet.Cells[row, col].Text; // This got me the actual value I needed.
                    listOfCheckBoxNames.Add(new ActionDictionary { Name = cellValue });
                }
            }

            for (int row = start.Row + 1; row <= end.Row; row++)
            { // Row by row...
                Employee employee = new Employee();
                for (int col = start.Column; col <= end.Column; col++)
                { // ... Cell by cell...
                    string cellValue = workSheet.Cells[row, col].Text; // This got me the actual value I needed.
                    if (row != 1)
                    {
                        switch (col)
                        {
                            case 1:
                                employee.FirstName = cellValue;
                                break;
                            case 2:
                                employee.LastName = cellValue;
                                break;
                            case 3:
                                employee.Email = cellValue;
                                break;
                            default:
                                break;
                        }
                    }
                }
                listOfEmployee.Add(employee);
            }
            loafe.ListOfEmployee = listOfEmployee;
            loafe.ListOfActionDictionary = listOfCheckBoxNames;
            return loafe;
        }
    }
}
