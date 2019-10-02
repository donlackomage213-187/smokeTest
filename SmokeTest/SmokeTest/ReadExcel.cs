using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;       //microsoft Excel 14 object in references-> COM tab

namespace SmokeTest
{


    public class Read_From_Excel
    {
        public static MainWindow mainWinInstance;

        public Read_From_Excel(MainWindow main)
        {
            mainWinInstance = main;
        }
        public static void getExcelFile()
        {

            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\lazlo\Desktop\smokeTest.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            int row = 1;

            for (int col = 1; col <= colCount; col++)
            {
                switch (col)
                {
                    case 1:
                        if (xlRange.Cells[row, col] != null && xlRange.Cells[row, col].Value2 != null)
                            mainWinInstance.tbAction.Text=(xlRange.Cells[row, col].Value2.ToString());
                        break;
                    case 2:
                        if (xlRange.Cells[row, col] != null && xlRange.Cells[row, col].Value2 != null)
                            mainWinInstance.tbExpected.Text = (xlRange.Cells[row, col].Value2.ToString());
                        break;
                    default:
                        break;
                }

                //write the value to the console

            }


            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
    }
}
}
}
