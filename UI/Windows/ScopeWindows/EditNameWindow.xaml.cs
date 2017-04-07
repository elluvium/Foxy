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

namespace UI.Windows.ScopeWindows
{
    /// <summary>
    /// Interaction logic for AreaEditNameWindow.xaml
    /// </summary>
    public partial class EditNameWindow : Window
    {
        public EditNameWindow()
        {
            InitializeComponent();
        }

        public EditNameWindow(string name)
        {
            InitializeComponent();
            textBoxAreaName.Text = name;
        }

        public string AreaName => textBoxAreaName.Text;

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
