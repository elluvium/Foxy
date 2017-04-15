using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Matrixes
{
    [Serializable]
    public class IncidenceMatrix<TVariable> : NamedSquareMatrix<TVariable, bool>
    {
        public IncidenceMatrix(HashSet<TVariable> variables) : base(variables)
        {
        }

        public IncidenceMatrix(IEnumerable<TVariable> variables) : base(variables)
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

        public override bool this[TVariable row,TVariable column]
        {
            get
            {
                return base[row, column];
            }
            set
            {
                if (base[row, column] == value)
                {
                    return;
                }
                if (value && CheckForCycles(_variables[row], _variables[column]))
                {
                    throw new ArithmeticException("Tree structure graph: cycles are prohibited.");
                }
                base[row, column] = value;
            }
        }

        public override bool this[int row, int column]
        {
            get
            {
                return base[row, column];
            }

            set
            {
                if(base[row,column] == value)
                {
                    return;
                }
                if (value && CheckForCycles(row, column))
                {
                    throw new ArithmeticException("Tree structure graph: cycles are prohibited.");
                }
                base[row, column] = value;
            }
        }

        private bool CheckForCycles(int currentvariable, int addedVariable)
        {
            for (int checkedVariable = 0; checkedVariable < _variables.Count; checkedVariable++)
            {
                if (_data[currentvariable, checkedVariable])
                {
                    if(checkedVariable == addedVariable)
                    {
                        return true;
                    }
                    else
                    {
                        return CheckForCycles(checkedVariable, addedVariable);
                    }
                }
            }
            return false;
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

        public IEnumerable<TVariable> GetDescendants(TVariable variable)
        {
            HashSet<TVariable> variables = new HashSet<TVariable>();
            foreach(var element in _variables.Keys)
            {
                if(this[element, variable])
                {
                    variables.Add(element);
                }
            }
            return variables;
        }

        

        public IEnumerable<TVariable> GetAncestors(TVariable variable)
        {
            HashSet<TVariable> variables = new HashSet<TVariable>();
            foreach (var element in _variables.Keys)
            {
                if (this[variable, element])
                {
                    variables.Add(element);
                }
            }
            return variables;
        }

        public IEnumerable<TVariable> GetAllDescendants(TVariable variable)
        {
            IEnumerable<TVariable> variables = GetDescendants(variable);
            foreach(var element in variables)
            {
                variables = variables.Union(GetAllDescendants(element));
            }
            return variables;
        }

        public IEnumerable<TVariable> GetAvailableForProvidingVariables(TVariable variable)
        {
            return Variables
                .Except(GetAllDescendants(variable))
                .Except(new TVariable[] { variable });
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

        public Dictionary<TVariable, double> GetPrioritiesByAncestorsForEachVariable()
        {
            var r = GetNumberOfConnections();
            return GetNumberOfAncestorsForEachVariable().ToDictionary(x => x.Key, y => (double)y.Value / r);
        }

        public Dictionary<TVariable, double> GetPrioritiesByDescendantsForEachVariable()
        {
            var r = GetNumberOfConnections();
            return GetNumberOfDescendantsForEachVariable().ToDictionary(x => x.Key, y => (double)y.Value / r);
        }
    }

   
}
