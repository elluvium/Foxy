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
    using Data.BusinessStructures;
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
            var windowDialog = new DialogWindow("Do you really want to delete this goal?");
            if (windowDialog.ShowDialog() == true)
            {
                currentBS.GoalsIncidenceMatrix.RemoveVariable(goal);
            }
            dataGridGoalsTable.ItemsSource = Goals;
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

        private void menuEvaluate_Click(object sender, RoutedEventArgs e)
        {
            FillDataGridFullMatrix();
            FillAllMatrixes();
        }

        private void FillDataGridFullMatrix()
        {
            dataGridFullMatrix.ItemsSource = IncidenceMatrixWithCalcuationsToDataArray(currentBS.GoalsIncidenceMatrix).Data.DefaultView;
        }

        private static DataArray<string> IncidenceMatrixWithCalcuationsToDataArray(IncidenceMatrix<Goal> matrix)
        {
            var matrixLength = matrix.Variables.Length;
            var dataArray = new DataArray<string>(matrixLength + 3, matrixLength + 3);
            var data = matrix.ToInt32Array();
            var numberOfAncestors = matrix.GetNumberOfAncestorsForEachVariable().Values.ToArray();
            var numberOfDescendants = matrix.GetNumberOfDescendantsForEachVariable().Values.ToArray();
            var prioritiesOfAncestors = matrix.GetPrioritiesByAncestorsForEachVariable().Values.ToArray();
            var prioritiesOfDescendants = matrix.GetPrioritiesByDescendantsForEachVariable().Values.ToArray();
            for (int i = 1; i < matrixLength + 1; i++)
            {
                dataArray[i][0] = dataArray[0][i] = matrix.Variables[i - 1].Index.ToString();
                for (int j = 1; j < matrixLength + 1; j++)
                {
                    dataArray[i][j] = data[i - 1, j - 1].ToString();
                }
                dataArray[matrixLength + 1][i] = numberOfDescendants[i - 1].ToString();
                dataArray[i][matrixLength + 1] = numberOfAncestors[i - 1].ToString();
                dataArray[matrixLength + 1][matrixLength + 1] = numberOfDescendants.Sum().ToString();
                dataArray[matrixLength + 2][i] = prioritiesOfDescendants[i - 1].ToString();
                dataArray[i][matrixLength + 2] = prioritiesOfAncestors[i - 1].ToString();
            }
            return dataArray;

        }

        private void FillAllMatrixes()
        {
            int[][,] allMatrixes = Logic.FoxMath.ExponentiateTillZero(currentBS.GoalsIncidenceMatrix.ToInt32Array());
            for(int index = 0; index < allMatrixes.Length; index++)
            {
                tabControlMatrixPowers.Items.Add(new TabItem()
                {
                    Content = new DataGrid()
                    {
                        ItemsSource = MatrixToDataArray(allMatrixes[index]).Data.DefaultView
                    },
                    Header = string.Format("A^{0}", index + 1)
                });
            }
        }

        public static DataArray<int> MatrixToDataArray(int[,] matrix)
        {
            var dataArray = new DataArray<int>(matrix.GetLength(0), matrix.GetLength(1));
            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    dataArray[i][j] = matrix[i, j];
                }
            }
            return dataArray;
        }

        private void menuFileLoadFromExcel_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<GoalExcel> GoalsExcel = new List<GoalExcel>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    GoalsExcel = DataHelper.ReadFromXLSX(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Loading was unsuccessful.");
                    MessageBox.Show(ex.Message);
                    return;
                }
                try
                {
                    currentBS.GoalsIncidenceMatrix = DataHelper.Convert(GoalsExcel); 
                }
                catch(InvalidCastException castExc)
                {
                    MessageBox.Show(castExc.Message);
                    return;
                }
                catch(ArithmeticException ariExc)
                {
                    MessageBox.Show(ariExc.Message);
                    return;
                }
                catch(Exception)
                {
                    MessageBox.Show("Error occured during convertation.");
                    return;
                }
                dataGridGoalsTable.ItemsSource = Goals;
            }
        }

    }
}
