using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Matrixes
{
    [Serializable]
    public class PairwiseComparisonsMatrix<TVariable> : NamedMatrix<TVariable, double>
    {
        public PairwiseComparisonsMatrix(HashSet<TVariable> variables) : base(variables) { }

        public PairwiseComparisonsMatrix(double[,] data, HashSet<TVariable> variables) : base(data, variables)
        {
            CheckDataConsistency(data);
        }

        private void CheckDataConsistency(double[,] data)
        {
            int length = data.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                for (int j = i; j < length; j++)
                {
                    if (!(data[i, j] * data[j, i]).EqualsWithError(1.0))
                    {
                        throw new ArithmeticException();
                    }
                }
            }
        }

        public override double this[TVariable row, TVariable column]
        {
            get
            {
                return base[row, column];
            }

            set
            {
                base[row, column] = value;
                base[column, row] = 1.0 / value;
            }
        }
    }
}
