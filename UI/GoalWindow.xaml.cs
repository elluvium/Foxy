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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Data;
using Microsoft.Win32;
using TreeLib;
using Data.Matrixes;

namespace UI
{
    using Models;
    using Windows;
    using Windows.BSWindows;
    using Windows.GoalWindows;
    using Windows.HelpWindows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GoalWindow : Window
    {
        public BusinessSystem currentBS;
        public string currentBSfullPath;

        IEnumerable<Models.GoalModelWithProvidings> Goals => currentBS.GoalsIncidenceMatrix.Variables.Select(x => new GoalModelWithProvidings(x, currentBS.GoalsIncidenceMatrix.GetAncestors(x)));

        public GoalWindow(BusinessSystem businessSystem)
        {
            InitializeComponent();
            currentBS = businessSystem;
            dataGridGoalsTable.ItemsSource = Goals;
        }

     
        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void dataGridGoalEdit_Click(object sender, RoutedEventArgs e)
        {
            //int column = dataGridGoalsTable.Columns.ToList().IndexOf(dataGridGoalsTable.SelectedCells[0].Column);
            //int row = dataGridGoalsTable.Items.IndexOf(dataGridGoalsTable.SelectedCells[0].Item);
            var selected = dataGridGoalsTable.SelectedItem as GoalModelWithProvidings;
            if(selected == null)
            {
                return;
            }
            var goal = selected.ToGoal();
            var ancestors = currentBS.GoalsIncidenceMatrix.GetAncestors(goal);
            var windowEdit = new GoalEditWindow(goal, currentBS.GoalsIncidenceMatrix.GetAvailableForProvidingVariables(goal).Select(x => new GoalModel(x, ancestors.Contains(x))));
            if(windowEdit.ShowDialog() == true)
            {
                goal = windowEdit.Goal;
                foreach(var goalmodel in windowEdit.ProvidedFor)
                {
                    currentBS.GoalsIncidenceMatrix[goalmodel.ToGoal(), goal] = goalmodel.ProvidedBy;
                }
            }
            dataGridGoalsTable.ItemsSource = Goals;
        }

        private void dataGridGoalDeleteThis_Click(object sender, RoutedEventArgs e)
        {
            var selected = dataGridGoalsTable.SelectedItem as GoalModelWithProvidings;
            if (selected == null)
            {
                return;
            }
            var goal = selected.ToGoal();
            var windowDialog = new DialogWindow("");
            if (windowDialog.ShowDialog() == true)
            {
                currentBS.GoalsIncidenceMatrix.RemoveVariable(goal);
            }
        }
        private void dataGridGoalDeleteThisAndSubgoals_Click(object sender, RoutedEventArgs e)
        {
            var selected = dataGridGoalsTable.SelectedItem as GoalModelWithProvidings;
            if (selected == null)
            {
                return;
            }
            var goal = selected.ToGoal();
            var windowDialog = new DialogWindow("");
            if (windowDialog.ShowDialog() == true)
            {
                MessageBox.Show("Not implemented function, sorry, bro ;P ");
            }
        }

        private void dataGridGoalAdd_Click(object sender, RoutedEventArgs e)
        {
            var windowEdit = new GoalEditWindow(Goals.Select(x => new GoalModel(x.ToGoal(), false)));
            if (windowEdit.ShowDialog() == true)
            {
                var goal = windowEdit.Goal;
                currentBS.GoalsIncidenceMatrix.AddVariable(goal);
                foreach (var goalmodel in windowEdit.ProvidedFor)
                {
                    currentBS.GoalsIncidenceMatrix[goalmodel.ToGoal(), goal] = goalmodel.ProvidedBy;
                }
                dataGridGoalsTable.ItemsSource = Goals;
            }
        }
    }
}
