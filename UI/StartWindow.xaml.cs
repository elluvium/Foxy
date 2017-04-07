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
using Data.BusinessStructures;

namespace UI
{
    using Microsoft.Win32;
    using Windows;
    using Windows.BSWindows;

    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        BusinessSystem currentBS;
        public string currentBSfullPath;


        public StartWindow()
        {
            InitializeComponent();
        }

        private void menuFileCreate_Click(object sender, RoutedEventArgs e)
        {
            var windowBS = new BusinessSystemEditWindow();
            if(windowBS.ShowDialog() == true)
            {
                currentBS = windowBS.BusinessSystem;
                UpdateBSView(currentBS);
                EnableUIElements();
            }          
        }

        private void EnableUIElements()
        {
            menuEdit.IsEnabled = true;
            buttonGoals.IsEnabled = true;
            buttonInnerScope.IsEnabled = true;
            buttonAmbient.IsEnabled = true;
        }

        private void UpdateBSView(BusinessSystem bs)
        {
            TitleTextbox.Text = bs.Title;
            VisionTextbox.Text = bs.Vision;
            MissionTextbox.Text = bs.Mission;
            GlobalGoalTextbox.Text = bs.GlobalGoal;
        }

        private void menuFileLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Business system (*.bs)|*.bs|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                currentBSfullPath = openFileDialog.FileName;
                try
                {
                    currentBS = DataHelper.DeserializeBS(currentBSfullPath);              
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Loading was unsuccessful.");
                    MessageBox.Show(ex.Message);
                    return;
                }
                UpdateBSView(currentBS);
                EnableUIElements();
            }
        }

        private void Save()
        {
            try
            {
                DataHelper.SerializeBS(currentBS, currentBSfullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Saving was unsuccessful.");
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Successfully saved.");
        }

        private void menuFileSave_Click(object sender, RoutedEventArgs e)
        {
            if (currentBS != null)
            {
                Save();
            }
        }

        private void menuFileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (currentBS != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "My business system";
                saveFileDialog.DefaultExt = ".bs";
                saveFileDialog.Filter = "Business system (*.bs)|*.bs|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    currentBSfullPath = saveFileDialog.FileName;
                    Save();
                }
            }
        }

        private void menuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void menuEdit_Click(object sender, RoutedEventArgs e)
        {
            var windowBS = new BusinessSystemEditWindow(currentBS);
            if (windowBS.ShowDialog() == true)
            {
                currentBS = windowBS.BusinessSystem;
                UpdateBSView(currentBS);
                EnableUIElements();
            }
        }

        private void buttonGoals_Click(object sender, RoutedEventArgs e)
        {
            new GoalWindow(currentBS).ShowDialog();
        }

        private void menuHelpAbout_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void buttonInnerScope_Click(object sender, RoutedEventArgs e)
        {
            new ScopeWindow(currentBS.FunctionalAreas, "Inner scope").ShowDialog();
        }

        private void buttonAmbient_Click(object sender, RoutedEventArgs e)
        {
            new ScopeWindow(currentBS.AmbientAreas, "Ambient").ShowDialog();
        }
    }
}
