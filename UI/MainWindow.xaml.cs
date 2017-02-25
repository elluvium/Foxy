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

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
    }
}
