using Data.BusinessStructures;
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

namespace UI.Windows.GoalWindows
{
    using Models;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for GoalEditWindow.xaml
    /// </summary>
    public partial class GoalEditWindow : Window
    {
        ObservableCollection<GoalModel> _goals;

        Goal _goal;

        public Goal Goal => _goal;

        public IEnumerable<GoalModel> ProvidedFor => _goals;

        private GoalEditWindow()
        {
            InitializeComponent();
        }

        public GoalEditWindow(IEnumerable<GoalModel> goals) : this()
        {
            _goals = new ObservableCollection<GoalModel>(goals);
            dataGridGoalsTable.ItemsSource = _goals;
        }

        public GoalEditWindow(Goal goal, IEnumerable<GoalModel> goals) : this(goals)
        {
            _goal = goal;
            textBoxIndex.Text = _goal.Index.ToString();
            textBoxContent.Text = _goal.Content;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            uint buff;
            if (uint.TryParse(textBoxIndex.Text, out buff))
            {
                if (_goal == null)
                {
                    _goal = new Goal() { Index = buff, Content = textBoxContent.Text };
                }
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Index should be non-negative integer.");
            }

        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
