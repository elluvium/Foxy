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

        private void menuEvaluate_Click(object sender, RoutedEventArgs e)
        {
            // ------------------------------------------------------------------------------------------------------
            //                                          CALCULATIONS
            // ------------------------------------------------------------------------------------------------------
            var localAspectsByAreasStrong = Scope.CalculateLocalPrioritiesOfAspectsByAreas(_scope.Areas, Side.Strong);
            var localAspectsByAreasWeak = Scope.CalculateLocalPrioritiesOfAspectsByAreas(_scope.Areas, Side.Weak);
            var areasStrong = Scope.CalculatePrioritiesOfAreas(localAspectsByAreasStrong, Side.Strong);
            var areasWeak = Scope.CalculatePrioritiesOfAreas(localAspectsByAreasWeak, Side.Weak);
            var globalAspectsStrong = Scope.CalculateGlobalPrioritiesOfAspects(Scope.CalculateGlobalPrioritiesOfAspectsByAreas(areasStrong, localAspectsByAreasStrong, Side.Strong));
            var globalAspectsWeak = Scope.CalculateGlobalPrioritiesOfAspects(Scope.CalculateGlobalPrioritiesOfAspectsByAreas(areasWeak, localAspectsByAreasWeak, Side.Weak));
            var bestAspectsStrong = Scope.ExtractTheMostValueableGlobalPrioritiesOfAspects(globalAspectsStrong);
            var bestAspectsWeak = Scope.ExtractTheMostValueableGlobalPrioritiesOfAspects(globalAspectsWeak);
            double positiveMark = Scope.CalculateStrategicEstimation(bestAspectsStrong);
            double negativeMark = 1 - Scope.CalculateStrategicEstimation(bestAspectsWeak);
            double systemParameter = Scope.CalculateSystemParameter(positiveMark, negativeMark);
            // ------------------------------------------------------------------------------------------------------
            //                                  Filling datagrids and textboxes
            // ------------------------------------------------------------------------------------------------------
            dataGridPrioritiesOfAreas.ItemsSource = areasStrong.Join(areasWeak, x => x.Key, y => y.Key, (x1, y1) => new Models.AreaModel(x1.Key) { StrongPriority = x1.Value, WeakPriority = y1.Value }).ToList();
            dataGridLocalStrongPrioritiesOfAspects.ItemsSource = createItemsSourceForGroupedDataGrid(localAspectsByAreasStrong);
            dataGridLocalWeakPrioritiesOfAspects.ItemsSource = createItemsSourceForGroupedDataGrid(localAspectsByAreasWeak);
            dataGridGlobalStrongPrioritiesOfAspects.ItemsSource = createItemsSourceForDataGridWithGlobalAspects(globalAspectsStrong);
            dataGridGlobalWeakPrioritiesOfAspects.ItemsSource = createItemsSourceForDataGridWithGlobalAspects(globalAspectsWeak);
            dataGridBestGlobalStrongPrioritiesOfAspects.ItemsSource = createItemsSourceForDataGridWithGlobalAspects(bestAspectsStrong);
            dataGridBestGlobalWeakPrioritiesOfAspects.ItemsSource = createItemsSourceForDataGridWithGlobalAspects(bestAspectsWeak);
            fillSysParams(positiveMark, negativeMark, systemParameter);

        }

        private IEnumerable<Models.AspectModel> createItemsSourceForDataGridWithGlobalAspects(IDictionary<Aspect, double> data)
        {
            return data.Select(x => new Models.AspectModel(x.Key, x.Value));
        }

        private ListCollectionView createItemsSourceForGroupedDataGrid(IDictionary<Area, IDictionary<Aspect, double>> data)
        {
            var convertedData = data
                            .Select(x => x.Value.Select(y => new Models.AspectModelByArea(y.Key, x.Key, y.Value)))
                            .Aggregate((x, y) => x.Union(y))
                            .ToList();
            var listColView = new ListCollectionView(convertedData);
            listColView.GroupDescriptions.Add(new PropertyGroupDescription("Area"));
            return listColView;
        }

        private void fillSysParams(double positiveMark, double negativeMark, double systemParameter)
        {
            textBoxOptimisticalMark.Text = positiveMark.ToString();
            textBoxPessimisticalMark.Text = negativeMark.ToString();
            textBoxStrategicParameter.Text = systemParameter.ToString();
        }

        private void buttonCalcSysParams_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGridAspectsWeakBestPrioritiesGlobalMenuEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGridAspectsStrongBestPrioritesGlobalMenuEdit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
