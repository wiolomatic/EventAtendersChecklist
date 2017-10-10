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
        /// Defines the ListOfReservedWords
        /// </summary>
        public static List<string> ListOfReservedWords = new List<string>()
        {
            "FirstName",
            "LastName",
            "Email"
        };

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

            // Set startPoint and endPoint
            var start = workSheet.Dimension.Start;
            var end = workSheet.Dimension.End;
            for (int row = start.Row; row <= start.Row; row++)
            {
                for (int col = start.Column + 3; col <= end.Column; col++)
                { // ... Cell by cell...
                    string cellValue = workSheet.Cells[row, col].Text; // This got me the actual value I needed.
                    if (!ListOfReservedWords.Any(x => x.Contains(cellValue))) // check if checkbox is not Name or email.
                    {
                        listOfCheckBoxNames.Add(new ActionDictionary { Name = cellValue });
                    }
                }
            }

            for (int row = start.Row + 1; row <= end.Row; row++)
            { // Row by row...
                try
                {
                    Employee employee = new Employee();
                    for (int col = start.Column; col <= end.Column; col++)
                    { // ... Cell by cell...
                        string cellValue = workSheet.Cells[row, col].Text; // This got me the actual value I needed.
                        if (cellValue == null)
                        {
                            break;
                        }
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
                catch (System.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
            }
            loafe.ListOfEmployee = listOfEmployee;
            loafe.ListOfActionDictionary = listOfCheckBoxNames;
            return loafe;
        }
    }
}
