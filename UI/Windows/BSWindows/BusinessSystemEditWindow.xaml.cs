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

namespace UI.Windows.BSWindows
{
    /// <summary>
    /// Interaction logic for NewBusinessSystem.xaml
    /// </summary>
    public partial class BusinessSystemEditWindow : Window
    {
        public BusinessSystemEditWindow()
        {
            InitializeComponent();
        }

        public BusinessSystemEditWindow(BusinessSystem bs)
        {
            InitializeComponent();
            UpdateBSView(bs);
        }

        private void UpdateBSView(BusinessSystem bs)
        {
            TitleTextbox.Text = bs.Title;
            VisionTextbox.Text = bs.Vision;
            MissionTextbox.Text = bs.Mission;
            GlobalGoalTextbox.Text = bs.GlobalGoal;
        }

        public BusinessSystem BusinessSystem { get; private set; }

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
            DialogResult = false;
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            BusinessSystem newBS = new BusinessSystem();

            newBS.Title = TitleTextbox.Text;
            newBS.Vision = VisionTextbox.Text;
            newBS.Mission = MissionTextbox.Text;
            newBS.GlobalGoal = GlobalGoalTextbox.Text;
            BusinessSystem = newBS;

            DialogResult = true;
            Close();
        }

        private void FuncZoneDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var defaultItems = new[] 
            {
                new { funcZone = "Производство" },
                new { funcZone = "Материально-техническая снабжение" },
                new { funcZone = "Материально-техническая база" },
                new { funcZone = "Финансы" },
                new { funcZone = "Менеджмент" },
                new { funcZone = "Персонал" },
                new { funcZone = "Маркетинг" },
                new { funcZone = "Имидж" }
            };

            var grid = sender as DataGrid;
            grid.ItemsSource = defaultItems.ToList();
        }

        private void KeyAreasDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var defaultItems = new[]
           {
                new { keyArea = "Положение на рынке" },
                new { keyArea = "Инновации" },
                new { keyArea = "Производительность" },
                new { keyArea = "Ресурсы" },
                new { keyArea = "Доходность (прибыльность)" },
                new { keyArea = "Управленческие аспекты" },
                new { keyArea = "Персонал" },
                new { keyArea = "Социальная ответственность" }
            };

            var grid = sender as DataGrid;
            grid.ItemsSource = defaultItems.ToList();
        }
    }
}
