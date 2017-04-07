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
using Data.BusinessStructures;

namespace UI
{
    public class MyArea 
    {
        public Area Area;

        public string Name => Area.Name;

        public MyArea(Area area)
        {
            Area = area;
        }

        public static implicit operator Area(MyArea area)
        {
            return area.Area;
        }
    }


    /// <summary>
    /// Interaction logic for ScopeWindow.xaml
    /// </summary>
    public partial class ScopeWindow : Window
    {
        private Scope _scope;

        public ScopeWindow(Scope scope, string nameOfScope)
        {
            InitializeComponent();
            Title = nameOfScope;
            _scope = scope;
            UpdateDataGridArea();
        }

        private void UpdateDataGridArea()
        {
            dataGridArea.ItemsSource =  _scope.Areas.Select(x => new MyArea(x)); 
        }

        private void menuFileReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void menuEvaluate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonAddArea_Click(object sender, RoutedEventArgs e)
        {
            var windowAddArea = new Windows.ScopeWindows.EditNameWindow();
            if(windowAddArea.ShowDialog() == true)
            {
                _scope.Add(new Area() { Name = windowAddArea.AreaName });
                UpdateDataGridArea();
            }
        }

        private void dataGridMenyAreaEditName_Click(object sender, RoutedEventArgs e)
        {
            var selected = dataGridArea.SelectedItem as MyArea;
            if (selected == null)
            {
                return;
            }
            var windowAddArea = new Windows.ScopeWindows.EditNameWindow(selected.Name);
            if (windowAddArea.ShowDialog() == true)
            {
                selected.Area.Name = windowAddArea.AreaName;
                UpdateDataGridArea();
            }

        }

        private void dataGridMenyAreaEditStrongAspects_Click(object sender, RoutedEventArgs e)
        {
            var selected = dataGridArea.SelectedItem as MyArea;
            if (selected == null)
            {
                return;
            }
            new Windows.ScopeWindows.AspectsWindow(selected, Side.Strong).ShowDialog();
        }

        private void dataGridMenyAreaEditWeakAspects_Click(object sender, RoutedEventArgs e)
        {
            var selected = dataGridArea.SelectedItem as MyArea;
            if (selected == null)
            {
                return;
            }
            new Windows.ScopeWindows.AspectsWindow(selected, Side.Weak).ShowDialog();
        }

        private void dataGridAreaMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dataGridArea.SelectedItem as MyArea;
            if (selected == null)
            {
                return;
            }
            if (new Windows.HelpWindows.DialogWindow("Do you want to delete this area?").ShowDialog() == true)
            {
                _scope.Remove(selected);
            }
        }

        private void dataGridArea_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
