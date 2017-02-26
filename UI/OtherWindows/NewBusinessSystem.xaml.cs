using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI.OtherWindows
{
    /// <summary>
    /// Interaction logic for NewBusinessSystem.xaml
    /// </summary>
    public partial class NewBusinessSystem : Window
    {
        public NewBusinessSystem()
        {
            InitializeComponent();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel spreadsheets (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    GoalsTable.ItemsSource = Data.DataHelper.ReadFromXLSX(openFileDialog.FileName);
                }
                catch (System.IO.IOException exception)
                {
                    MessageBox.Show(
                        this, 
                        exception.Message + "\n" + "Try to close all windows using this file and try again.", 
                        "Cannot access the file", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Information);
                }              
            }
        }
    }
}
