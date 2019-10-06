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
        public List<string> comboBoxItems = new List<string>();
        private bool comboBoxSelectedIndexChanged = false;


        public MainWindow()
        {
            InitializeComponent();


            SetUpWindow();
            new Thread(new ThreadStart(InitializeComboBox)).Start();

            new Thread(new ThreadStart(getExcelFile)).Start();
        }

        private void SetUpWindow()
        {
            this.Closed += MainWindow_Closed;
            this.BringIntoView();
            this.Show();
        }

        private void InitializeComboBox()
        {
            this.Dispatcher.Invoke(() =>
            {
                cbTestType.SelectedItem = "Choose a Test Type!";
            });
            this.Dispatcher.Invoke(() =>
            {
                cbTestType.Foreground = Brushes.Red;
            });
            this.Dispatcher.Invoke(() =>
            {
                cbTestType.SelectionChanged += CbTestType_SelectionChanged;
            });
        }


        public void CbTestType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBoxSelectedIndexChanged = true;
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
            while (!comboBoxSelectedIndexChanged)
            {
                this.Dispatcher.Invoke(() => { tbAction.Text = "Choose a Test Type!"; });
                Thread.Sleep(100);
            }
            
            this.Dispatcher.Invoke(() => { tbAction.Text = string.Empty; });

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\lazlo\Desktop\smokeTest.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            int row = 3;
            if (xlRange.Cells[2, 1] != null && xlRange.Cells[2, 1].Value2 != null)
                this.Dispatcher.Invoke(() =>
                {
                    tbAction.Text = (xlRange.Cells[2, 1].Value2.ToString());
                });
            if (xlRange.Cells[2, 2] != null && xlRange.Cells[2, 2].Value2 != null)
                this.Dispatcher.Invoke(() =>
                {
                    tbExpected.Text = (xlRange.Cells[2, 2].Value2.ToString());
                });
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
                                this.Dispatcher.Invoke(() =>
                                {
                                    tbAction.Text = (xlRange.Cells[row, col].Value2.ToString());
                                });

                            break;
                        case 2:
                            if (xlRange.Cells[row, col] != null && xlRange.Cells[row, col].Value2 != null)
                                this.Dispatcher.Invoke(() =>
                                {
                                    tbExpected.Text = (xlRange.Cells[row, col].Value2.ToString());
                                });

                            break;
                        default:
                            break;
                    }

                }
                while (!ButtonPressed)
                {
                    Thread.Sleep(100);
                }
                ButtonPressed = false;
                row++;
            }



            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Show();
        }
    }
}
