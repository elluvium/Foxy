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
using UI.OtherWindows;
using Data;
using Microsoft.Win32;
using TreeLib;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static BusinessSystem currentBS;
        public static string currentBSfullPath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewBusinessSystemWindow_Click(object sender, RoutedEventArgs e)
        {
            NewBusinessSystem newBusinessSystem = new NewBusinessSystem();
            bool? dialogResult = newBusinessSystem.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow newAboutWindow = new AboutWindow();
            bool? dialogResult = newAboutWindow.ShowDialog();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Business system (*.bs)|*.bs|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                currentBSfullPath = openFileDialog.FileName;

                currentBS = DataHelper.DeserializeBS(currentBSfullPath);

                LoadGoalsTable();
                LoadAdjacencyMatrices();
                LoadGoalTree();
            }
        }

        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // for test
            TestInitBS();

            if (currentBS != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "My business system";
                saveFileDialog.DefaultExt = ".bs";
                saveFileDialog.Filter = "Business system (*.bs)|*.bs|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    currentBSfullPath = saveFileDialog.FileName;

                    DataHelper.SerializeBS(currentBS, currentBSfullPath);
                }
            }
        }

        // TODO delete this method after functioning NewBS window.
        //for test
        private void TestInitBS()
        {
            Tree<Goal> testGoalTree = new Tree<Goal>(new Goal() { Index = 1, Content = "Goal content 1" });
            testGoalTree.Add(new Goal() { Index = 2, Content = "Goal content 2" }, new List<TreeNode<Goal>>() { testGoalTree.Root });
            testGoalTree.Add(new Goal() { Index = 3, Content = "Goal content 3" }, new List<TreeNode<Goal>>() { testGoalTree.Root });

            currentBS = new BusinessSystem()
            {
                Title = "Some title",
                Vision = "Some vision",
                Mission = "Some mission",
                GlobalGoal = "Some global goal",
                FunctionalZones = new List<string>() { "fz1", "fz2", "fz3" },
                KeyAreas = new List<string>() { "ka1", "ka2", "ka3" },
                GoalsIncidenceMatrix = new IncidenceMatrix<Goal>(testGoalTree)
            };
        }

        // TODO LoadGoalsTable
        private void LoadGoalsTable()
        {
            throw new NotImplementedException();
        }

        // TODO LoadAdjacencyMatrices
        private void LoadAdjacencyMatrices()
        {
            throw new NotImplementedException();
        }

        // TODO LoadGoalTree
        private void LoadGoalTree()
        {
            throw new NotImplementedException();
        }
    }
}
