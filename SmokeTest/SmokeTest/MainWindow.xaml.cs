using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;

namespace SmokeTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
                       
           // getExcelFile();
            new Thread(new ThreadStart(getExcelFile)).Start();

            this.Closed += MainWindow_Closed;
            this.BringIntoView();
            this.Show();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void ShowMainWindow()
        {
            this.BringIntoView();
            this.Show();
        }

        

        private bool buttonPressed;
        public bool ButtonPressed { get { return buttonPressed; } set { buttonPressed = value; } }

        private void btNext_Click(object sender, RoutedEventArgs e)
        {
            ButtonPressed = true;
        }

        public void getExcelFile()
        {
          
            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\lazlo\Desktop\smokeTest.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            int row = 3;
            if (xlRange.Cells[2, 1] != null && xlRange.Cells[2, 1].Value2 != null)
                this.Dispatcher.Invoke(() => {
                    tbAction.Text = (xlRange.Cells[2, 1].Value2.ToString());
                });
            //  UpdateActionTbText(xlRange.Cells[2, 1].Value2.ToString());
            if (xlRange.Cells[2, 2] != null && xlRange.Cells[2, 2].Value2 != null)
                this.Dispatcher.Invoke(() => {
                    tbExpected.Text = (xlRange.Cells[2, 2].Value2.ToString());
                });
            // UpdateExpectedTbText(xlRange.Cells[2, 2].Value2.ToString());
            while (!ButtonPressed)
            {
                Thread.Sleep(100);
            }
            ButtonPressed = false;

            while (row <= rowCount)
            {


                for (int col = 1; col <= colCount; col++)
                {
                    switch (col)
                    {
                        case 1:
                            if (xlRange.Cells[row, col] != null && xlRange.Cells[row, col].Value2 != null)
                                this.Dispatcher.Invoke(() => {
                                    tbAction.Text = (xlRange.Cells[row, col].Value2.ToString());
                                });
                            //tbAction.Dispatcher.Invoke(
                            //                new UpdateTextCallback(UpdateActionTbText),
                            //                new object[] { (xlRange.Cells[row, col].Value2.ToString()) }
                            //); 
                            break;
                        case 2:
                            if (xlRange.Cells[row, col] != null && xlRange.Cells[row, col].Value2 != null)
                                this.Dispatcher.Invoke(() => {
                                    tbExpected.Text = (xlRange.Cells[row, col].Value2.ToString());
                                });
                            //tbExpected.Dispatcher.Invoke(
                            //      new UpdateTextCallback(UpdateExpectedTbText),
                            //                new object[] { (xlRange.Cells[row, col].Value2.ToString()) }
                            //); 
                            break;
                        default:
                            break;
                    }

                    //write the value to the console

                }
                while (!ButtonPressed)
                {
                    Thread.Sleep(100);
                }
                ButtonPressed = false;
                row++;
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
        public delegate void UpdateTextCallback(string message);

        public void UpdateActionTbText(string text)
        {
            tbAction.Text = text;
        }


        public void UpdateExpectedTbText(string text)
        {
            tbExpected.Text = text;
        }


        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Show();
        }
    }
}
