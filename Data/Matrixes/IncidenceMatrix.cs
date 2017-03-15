using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeLib;

namespace Data.Matrixes
{
    [Serializable]
    public class IncidenceMatrix<TVariable> : NamedMatrix<TVariable, bool>
    {
        public IncidenceMatrix(HashSet<TVariable> variables) : base(variables)
        {
        }

        public IncidenceMatrix(bool[,] matrix, HashSet<TVariable> variables) : base(matrix, variables)
        {
            if(checkMatrixForCycles(matrix))
            {
                throw new ArithmeticException("There cannot be cycles in tree graph.");
            }
        }
        private bool checkMatrixForCycles(bool[,] matrix)
        {
            int end = matrix.GetLength(0);
            for (int index = 0; index < end; index++)
            {
                if(matrix[index,index])
                {
                    return true;
                }
            }
            for(int row = 0; row < end; row++)
            {
                int start = 1;
                for (int col = start; col < end; col++)
                {
                    if(matrix[row, col] && matrix[col, row])
                    {
                        return true;
                    }
                }
                start++;
            }
            return false;
        }


        public IncidenceMatrix(Tree<TVariable> tree) : base(new HashSet<TVariable>(tree.Select(x => x.Value)))
        {
            foreach(var row in tree)
            {
                foreach (var col in tree)
                {
                    if (row.Descendants.Contains(col))
                    {
                        this[row.Value, col.Value] = true;
                    }
                    else
                    {
                        this[row.Value, col.Value] = false;
                    }
                }

            }
        }

        public override bool this[TVariable row,TVariable column]
        {
            get
            {
                return base[row, column];
            }
            set
            {
                if (value && base[column, row])
                {
                    throw new ArithmeticException();
                }
                base[row, column] = value;
            }
        }

            
        public int[,] ToInt32Array()
        {
            var data = GetDataArray();
            int length = data.GetLength(0);
            int[,] result = new int[length, length];
            for (int row = 0; row < length; row++)
            {
                for (int col = 0; col < length; col++)
                {
                    if (data[row, col])
                    {
                        result[row, col] = 1;
                    }
                }
            }
            return result;
        }

        public Dictionary<TVariable, int> GetNumberOfAncestorsForEachVariable()
        {
            var data = GetDataArray();
            var variables = Variables;
            var result = new Dictionary<TVariable, int>();
            for(int i = 0; i < data.GetLength(0); i++)
            {
                int sum = 0;
                for(int j = 0; j < data.GetLength(1); j++)
                {
                    if(data[i, j])
                    {
                        sum++;
                    }
                }
                result.Add(variables[i], sum);
            }
            return result;
        }

        public Dictionary<TVariable, int> GetNumberOfDescendantsForEachVariable()
        {
            var data = GetDataArray();
            var variables = Variables;
            var result = new Dictionary<TVariable, int>();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                int sum = 0;
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if (data[j, i])
                    {
                        sum++;
                    }
                }
                result.Add(variables[i], sum);
            }
            return result;
        }

        public IEnumerable<TVariable> GetIndependentVariables()
        {
            return GetNumberOfAncestorsForEachVariable().Where(x => x.Value == 0).Select(x => x.Key);
        }

        public int GetNumberOfConnections()
        {
            return GetNumberOfAncestorsForEachVariable().Values.Sum();
        }

        public Dictionary<TVariable, int> GetPrioritiesByAncestorsForEachVariable()
        {
            var dict = GetNumberOfAncestorsForEachVariable();
            var r = GetNumberOfConnections();
            foreach (var key in dict.Keys)
            {
                dict[key] /= r;
            }
            return dict;
        }

        public Dictionary<TVariable, int> GetPrioritiesByDescendantsForEachVariable()
        {
            var dict = GetNumberOfDescendantsForEachVariable();
            var r = GetNumberOfConnections();
            foreach (var key in dict.Keys)
            {
                dict[key] /= r;
            }
            return dict;
        }

    }

   
}
