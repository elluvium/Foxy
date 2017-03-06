using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeLib;

namespace Data
{
    [Serializable]
    public class IncidenceMatrix<T>
    {
        bool[,] _data;

        public bool[,] ToArray() => _data;

        public T[] Variables => _variables.ToArray();

        private IList<T> _variables;

        public IncidenceMatrix(HashSet<T> variables)
        {
            _variables = variables.ToList();
        }

        public IncidenceMatrix(bool[,] matrix, HashSet<T> variables)
        {
            if(matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new ArgumentException();
            }
            if(matrix.GetLength(0) != variables.Count)
            {
                throw new ArgumentException();
            }
            if(checkMatrixForCycles(matrix))
            {
                throw new ArithmeticException();
            }
            _variables = variables.ToList();
            _data = matrix;
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


        public IncidenceMatrix(Tree<T> tree)
        {
            _variables = tree.Select(x => x.Value).ToArray();
            _data = new bool[tree.Count(), tree.Count()];
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

        public bool this[T row,T column]
        {
            get
            {
                int rowIndex = _variables.IndexOf(row);
                int colIndex = _variables.IndexOf(column);
                if(rowIndex == -1 || colIndex == -1)
                {
                    throw new ArgumentException();
                }
                return _data[rowIndex, colIndex];
            }
            private set
            {
                int rowIndex = _variables.IndexOf(row);
                int colIndex = _variables.IndexOf(column);
                if (rowIndex == -1 || colIndex == -1)
                {
                    throw new ArgumentException();
                }
                if (value && _data[colIndex, rowIndex])
                {
                    throw new ArithmeticException();
                }
                _data[rowIndex, colIndex] = value;
            }
        }

        public bool this[int rowIndex, int colIndex]
        {
            get
            {
                if(checkBorders(0, _variables.Count - 1, rowIndex, colIndex))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return _data[rowIndex, colIndex];
            }
            set
            {
                if (checkBorders(0, _variables.Count - 1, rowIndex, colIndex))
                {
                    throw new ArgumentOutOfRangeException();
                }
                if(value && _data[colIndex, rowIndex])
                {
                    throw new ArithmeticException();
                }
                _data[rowIndex, colIndex] = value;
            }
        }

        private bool checkBorders(int lower, int upper, params int[] values)
        {
            return !values.Any(value => lower <= value && value <= upper);
        }

            
        public int[,] ToInt32Array()
        {
            int length = _data.GetLength(0);
            int[,] result = new int[length, length];
            for (int row = 0; row < length; row++)
            {
                for (int col = 0; col < length; col++)
                {
                    if (_data[row, col])
                    {
                        result[row, col] = 1;
                    }
                }
            }
            return result;
        }

        public Dictionary<T, int> GetNumberOfAncestorsForEachVariable()
        {
            var result = new Dictionary<T, int>();
            for(int i = 0; i < _data.GetLength(0); i++)
            {
                int sum = 0;
                for(int j = 0; j < _data.GetLength(1); j++)
                {
                    if(_data[i, j])
                    {
                        sum++;
                    }
                }
                result.Add(_variables[i], sum);
            }
            return result;
        }

        public Dictionary<T, int> GetNumberOfDescendantsForEachVariable()
        {
            var result = new Dictionary<T, int>();
            for (int i = 0; i < _data.GetLength(0); i++)
            {
                int sum = 0;
                for (int j = 0; j < _data.GetLength(1); j++)
                {
                    if (_data[j, i])
                    {
                        sum++;
                    }
                }
                result.Add(_variables[i], sum);
            }
            return result;
        }

        public IEnumerable<T> GetIndependentVariables()
        {
            return GetNumberOfAncestorsForEachVariable().Where(x => x.Value == 0).Select(x => x.Key);
        }

        public int GetNumberOfConnections()
        {
            return GetNumberOfAncestorsForEachVariable().Values.Sum();
        }

        public Dictionary<T, int> GetPrioritiesByAncestorsForEachVariable()
        {
            var dict = GetNumberOfAncestorsForEachVariable();
            var r = GetNumberOfConnections();
            foreach (var key in dict.Keys)
            {
                dict[key] /= r;
            }
            return dict;
        }

        public Dictionary<T, int> GetPrioritiesByDescendantsForEachVariable()
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
