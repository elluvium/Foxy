using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Data.Matrixes
{


    [Serializable]
    public class PairwiseComparisonsMatrix<TVariable> : NamedSquareMatrix<TVariable, double>
    {
        [NonSerialized]
        static readonly int minimumPairwiseValue = 1;
        [NonSerialized]
        static readonly int maximumPairwiseValue = 9;

        public PairwiseComparisonsMatrix(HashSet<TVariable> variables) : base(variables, 1)
        {
            InitializeNewVariableValuesInMatrix();
        }

        public PairwiseComparisonsMatrix(double[,] data, HashSet<TVariable> variables) : base(data, variables, 1)
        {
            CheckDataConsistency(data);
        }

        internal PairwiseComparisonsMatrix(IEnumerable<TVariable> variables) : base(variables, 1)
        {
            InitializeNewVariableValuesInMatrix();
        }

        private void CheckDataConsistency(double[,] data)
        {
            int length = data.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                for (int j = i; j < length; j++)
                {
                    if (!(data[i, j] * data[j, i]).EqualsWithError(DefaultValue))
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

        public static bool CheckValidity(int value)
        {
            return minimumPairwiseValue <= value && value <= maximumPairwiseValue;
        }

        public void SetPreference(TVariable source, TVariable target, int value)
        {

            if (!CheckVariables(source, target))
            {
                throw new ArgumentException();
            }
            if(!CheckValidity(value))
            {
                throw new ArgumentOutOfRangeException("Pairwise value must be in range [1,9]");
            }
            if(source.Equals(target))
            {
                return;
            }
            base[source, target] = value;
            base[target, source] = DefaultValue / value;
        }

        public void SetPreference(int source, int target, int value)
        {
            if (!CheckValidity(value))
            {
                throw new ArgumentOutOfRangeException("Pairwise value must be in range [1,9]");
            }
            if(source == target)
            {
                return;
            }
            base[source, target] = value;
            base[target, source] = DefaultValue / value;
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
