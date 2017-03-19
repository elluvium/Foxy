using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Matrixes
{


    [Serializable]
    public class PairwiseComparisonsMatrix<TVariable> : NamedSquareMatrix<TVariable, double>
    {
        const int defaultValue = 1;
        readonly int[] pairwiseScale = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

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
                    if (!(data[i, j] * data[j, i]).EqualsWithError(defaultValue))
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
            }
        }

        public override double this[int row, int column]
        {
            get
            {
                return base[row, column];
            }

            set
            {
            }
        }

        public void SetPreference(TVariable source, TVariable target, int value)
        {

            if (CheckVariables(source, target))
            {
                throw new ArgumentException();
            }
            if(!pairwiseScale.Contains(value))
            {
                throw new ArgumentOutOfRangeException("value is in range [1,9]");
            }
            base[source, target] = value;
            base[target, source] = defaultValue / value;
        }

        protected override void InitializeNewVariableValuesInMatrix(double defaultValue = default(double))
        {
            base.InitializeNewVariableValuesInMatrix(defaultValue);
        }

        public IDictionary<TVariable, double> CalculateLocalPriorities()
        {
            var result = new Dictionary<TVariable, double>(Variables.Count());
            var variables = Variables;
            int length = variables.Length;
            double sum = 0;
            foreach (var variableRow in variables)
            {
                double prod = 1.0;
                foreach(var variableColumn in variables)
                {
                    prod *= Math.Pow(this[variableRow, variableColumn], 1.0 / length);
                }
                result.Add(variableRow, prod);
                sum += prod;
            }
            foreach(var variable in variables)
            {
                result[variable] /= sum;
            }
            return result;
        }


    }
}
