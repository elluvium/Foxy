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
            textBoxIntensityUpBorder.Text = aspect.Intensity.UpperBound.ToString();
            textBoxIntensityCurrentValue.Text = aspect.Intensity.NominalValue.ToString();
            textBoxIntensityDownBorder.Text = aspect.Intensity.LowerBound.ToString();
        }

        public string AspectName => textBoxAspectName.Text;

        public Intensity Intensity { get; private set; }


        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            int buffUp, buffCurr, buffLow;
            if(
                int.TryParse(textBoxIntensityUpBorder.Text, out buffUp) &&
                int.TryParse(textBoxIntensityCurrentValue.Text, out buffCurr) &&
                int.TryParse(textBoxIntensityDownBorder.Text, out buffLow)
                )
            {
                try
                {
                    Intensity = new Intensity(buffLow, buffCurr, buffUp);
                }
                catch(ArgumentException argExc)
                {
                    MessageBox.Show(argExc.Message);
                    return;
                }
                DialogResult = true;
                Close();
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
