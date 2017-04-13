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
        public int _lowerBound;
        public int _nominalValue;
        public int _upperBound;


        public int LowerBound
        {
            get
            {
                return _lowerBound;
            }
            private set
            {
                //if(_lowerBound > _nominalValue || _lowerBound == _upperBound)
                //{
                //    throw new ArgumentException("LowerBound < NominalValue < UpperBound");
                //}
                _lowerBound = value;
            }
        }

        public int UpperBound
        {
            get
            {
                return _upperBound;
            }
            private set
            {
                //if (_upperBound < _nominalValue || _lowerBound == _upperBound)
                //{
                //    throw new ArgumentException();
                //}
                _upperBound = value;
            }
        }

        public int NominalValue
        {
            get
            {
                return _nominalValue;
            }
            private set
            {
                //if (_lowerBound > _nominalValue || _nominalValue > _upperBound)
                //{
                //    throw new ArgumentException();
                //}
                _nominalValue = value;
            }
        }


        public Intensity(int lowerBorder, int nominalValue, int upperBorder)
        {
            UpperBound = upperBorder;
            LowerBound = lowerBorder;
            NominalValue = nominalValue; 
        }

        public double GetNormalizedValue()
        {
            return (_nominalValue - _lowerBound) / (double) (_upperBound - _lowerBound);
        }
    }
}
