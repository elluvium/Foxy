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

namespace UI.Windows.ScopeWindows
{

    public class MyAspect
    {
        public Aspect Aspect;

        public string Name => Aspect.Name;

        public MyAspect(Aspect aspect)
        {
            Aspect = aspect;
        }

        public static implicit operator Aspect(MyAspect aspect)
        {
            return aspect.Aspect;
        }
    }

    /// <summary>
    /// Interaction logic for AspectsWindow.xaml
    /// </summary>
    public partial class AspectsWindow : Window
    {
        private Area _area;
        private Side _side;

        public AspectsWindow(Area area, Side side)
        {
            InitializeComponent();
            _area = area;
            _side = side;
            Title = side.ToString() + " aspects";
            dataGridAspects.Items.Clear();
            UpdateAll();
        }

        private void UpdateAll()
        {
            dataGridAspects.ItemsSource = _area.Aspects[_side].Select(x => new MyAspect(x));
            UpdateExperts();
        }

        private void UpdateExperts()
        {
            tabControlExperts.Items.Clear();
            int i = 0;
            foreach(var matrix in _area.ExpertsComparisons[_side])
            {
                addExpertTab(matrix, ++i);
            }
            
        }

        private void buttonAddAspect_Click(object sender, RoutedEventArgs e)
        {
            var windowAddAspect = new AspectEditWindow();
            if(windowAddAspect.ShowDialog() == true)
            {
                _area.AddAspect(new Aspect(windowAddAspect.AspectName, windowAddAspect.Intensity), _side);
            }
            UpdateAll();
        }

        private void dataGridAreaMenuEditAspect_Click(object sender, RoutedEventArgs e)
        {
            var selected = dataGridAspects.SelectedItem as MyAspect;
            if (selected == null)
            {
                return;
            }
            var windowAddAspect = new AspectEditWindow(selected);
            if (windowAddAspect.ShowDialog() == true)
            {
                if (selected.Name != windowAddAspect.AspectName)
                {
                    selected.Aspect.Name = windowAddAspect.AspectName;
                    UpdateAll();
                }
                selected.Aspect.Intensity = windowAddAspect.Intensity;
            }
        }

        private void dataGridAspectMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dataGridAspects.SelectedItem as MyAspect;
            if (selected == null)
            {
                return;
            }
            if(new Windows.HelpWindows.DialogWindow("Do you really want to delete this aspect?").ShowDialog() == true)
            {
                _area.RemoveAspect(selected, _side);
                UpdateAll();
            }
        }

        private void buttonAddExpert_Click(object sender, RoutedEventArgs e)
        {
            _area.AddExpertOpinion(_side);
            var matrixes = _area.ExpertsComparisons[_side];
            addExpertTab(matrixes.Last(), matrixes.Count()+1);

        }

        public void addExpertTab(Data.Matrixes.PairwiseComparisonsMatrix<Aspect> matrix, int number)
        {
            TabItem tabItem = new TabItem() { Header = "Expertise" };
            Button deleteExpertOpinion = new Button()
            {
                Height = 30,
                Content = "Delete these comparisons",
                VerticalAlignment = VerticalAlignment.Top,
            };
            deleteExpertOpinion.Click += (x, y) =>
            {
                if (new HelpWindows.DialogWindow("Do you really want to delete these comparisons?").ShowDialog() == true)
                {
                    _area.RemoveExpertOpinion(matrix, _side);
                    tabControlExperts.Items.Remove(tabItem);
                }
            };
            Controls.PairwiseComparisonsDataGrid dataGridComparisons = new Controls.PairwiseComparisonsDataGrid(matrix)
            {
                Margin = new Thickness(0, 30, 0, -0.333),
                
            };
            Grid proxyGrid = new Grid();
            proxyGrid.Children.Add(deleteExpertOpinion);
            proxyGrid.Children.Add(dataGridComparisons);
            tabItem.Content = proxyGrid;
            tabControlExperts.Items.Add(tabItem);
        }

        private void dataGridAspects_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
