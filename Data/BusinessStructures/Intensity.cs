using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BusinessStructures
{
    [Serializable]
    public class Intensity
    {
        public double LowerBorder { get; set; }
        public double UpperBorder { get; set; }
        public double NominalValue { get; set; }

        public Intensity(double lowerBorder, double nominalValue, double upperBorder)
        {
            LowerBorder = lowerBorder;
            NominalValue = nominalValue;
            UpperBorder = upperBorder;
        }
    }
}
