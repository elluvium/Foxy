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
    /// <summary>
    /// Interaction logic for AspectEditWindow.xaml
    /// </summary>
    public partial class AspectEditWindow : Window
    {
        public AspectEditWindow()
        {
            InitializeComponent();
        }

        public AspectEditWindow(Aspect aspect)
        {
            InitializeComponent();
            textBoxAspectName.Text = aspect.Name;
            textBoxIntensityUpBorder.Text = aspect.Intensity.UpperBorder.ToString();
            textBoxIntensityCurrentValue.Text = aspect.Intensity.NominalValue.ToString();
            textBoxIntensityDownBorder.Text = aspect.Intensity.LowerBorder.ToString();
        }

        public string AspectName => textBoxAspectName.Text;

        public Intensity Intensity => new Intensity()
        {
            UpperBorder = int.Parse(textBoxIntensityUpBorder.Text),
            NominalValue = int.Parse(textBoxIntensityCurrentValue.Text),
            LowerBorder = int.Parse(textBoxIntensityDownBorder.Text)
        };


        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            int buff;
            if(
                int.TryParse(textBoxIntensityUpBorder.Text, out buff) &&
                int.TryParse(textBoxIntensityCurrentValue.Text, out buff) &&
                int.TryParse(textBoxIntensityDownBorder.Text, out buff)
                )
            {
                DialogResult = true;
                Close();
                return;
            }
            else
            {
                MessageBox.Show("Invalid intensity values.", "Cast Error");
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
