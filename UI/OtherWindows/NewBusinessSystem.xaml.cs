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
using Data;

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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            BusinessSystem newBS = new BusinessSystem();

            newBS.Title = TitleTextbox.Text;
            newBS.Vision = VisionTextbox.Text;
            newBS.Mission = MissionTextbox.Text;
            newBS.GlobalGoal = GlobalGoalTextbox.Text;

            // TODO Finalize BS creation


            MainWindow.currentBS = newBS;
        }

        private void FuncZoneDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var defaultItems = new List<string>();

            defaultItems.Add("Производство");
            defaultItems.Add("Материально-техническая снабжение");
            defaultItems.Add("Материально-техническая база");
            defaultItems.Add("Финансы");
            defaultItems.Add("Менеджмент");
            defaultItems.Add("Персонал");
            defaultItems.Add("Маркетинг");
            defaultItems.Add("Имидж");

            var grid = sender as DataGrid;
            grid.ItemsSource = defaultItems;
        }

        private void KeyAreasDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var defaultItems = new List<string>();

            defaultItems.Add("Положение на рынке");
            defaultItems.Add("Инновации");
            defaultItems.Add("Производительность");
            defaultItems.Add("Ресурсы");
            defaultItems.Add("Доходность (прибыльность)");
            defaultItems.Add("Управленческие аспекты");
            defaultItems.Add("Персонал");
            defaultItems.Add("Социальная ответственность");

            var grid = sender as DataGrid;
            grid.ItemsSource = defaultItems;
        }
    }
}
