using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Matrixes
{
    [Serializable]
    public class NamedSquareMatrix<TVariable, TDataType>
    {

        protected TDataType[,] _data;
        public TDataType[,] GetDataArray() => _data;

        protected IDictionary<TVariable, int> _variables;
        public TVariable[] Variables => _variables.Keys.ToArray();

        public NamedSquareMatrix(HashSet<TVariable> variables)
        {
            _data = new TDataType[variables.Count, variables.Count];
            InitVariables(variables.ToArray());
        }
        public NamedSquareMatrix(TDataType[,] data, HashSet<TVariable> variables)
        {
            CheckInitData(data.GetLength(0), data.GetLength(1), variables.Count);
            _data = data;
            InitVariables(variables.ToArray());
        }

        private void CheckInitData(int dataRowLength, int dataColumnLength, int variablesLength)
        {
            if (dataRowLength != dataColumnLength || dataRowLength != variablesLength)
            {
                throw new ArgumentException();
            }
        }

        private void InitVariables(TVariable[] variables)
        {
            _variables = new Dictionary<TVariable, int>(variables.Length);
            for (int i = 0; i < variables.Length; i++)
            {
                _variables.Add(variables[i], i);
            }
        }

        public virtual TDataType this[TVariable row, TVariable column]
        {
            get
            {
                if (CheckVariables(row, column))
                {
                    throw new ArgumentException();
                }
                return _data[_variables[row], _variables[column]];
            }
            set
            {             
                if(CheckVariables(row, column))
                {
                    throw new ArgumentException();
                }
                _data[_variables[row], _variables[column]] = value;
            }
        }

        public virtual TDataType this[int row, int column]
        {
            get
            {
                if(CheckIndexes(row, column))
                {
                    throw new IndexOutOfRangeException();
                }
                return _data[row, column];
            }
            set
            {
                if (CheckIndexes(row, column))
                {
                    throw new IndexOutOfRangeException();
                }
                _data[row, column] = value;
            }
        }

        public void AddVariable(TVariable variable)
        {
            _variables.Add(variable, _variables.Count);
            var newData = new TDataType[_data.GetLength(0) + 1, _data.GetLength(1) + 1];
            for(int i = 0; i < _data.GetLength(0); i++)
            {
                for(int j = 0; j < _data.GetLength(1); j++)
                {
                    newData[i, j] = _data[i, j];
                }
            }
            _data = newData;
            InitializeNewVariableValuesInMatrix();
        }

        protected virtual void InitializeNewVariableValuesInMatrix(TDataType defaultValue = default(TDataType))
        {
            int length = _data.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                _data[i, length - 1] = defaultValue;
                _data[length - 1, i] = defaultValue;
            }
        }

        protected bool CheckVariable(TVariable variable)
        {
            return _variables.Keys.Contains(variable);
        }
        protected bool CheckVariables(params TVariable[] variables)
        {
            return !variables.Any(variable => !CheckVariable(variable));
        }
        protected bool CheckIndex(int index)
        {
            return _variables.Values.Contains(index);
        }
        protected bool CheckIndexes(params int[] indexes)
        {
            return !indexes.Any(index => !CheckIndex(index));
        } 

        public void RemoveVariable(TVariable variable)
        {
            if(CheckVariable(variable))
            {
                return;
            }
            int varIndex = _variables[variable];
            _variables.Remove(variable);
            foreach(var element in _variables.Keys)
            {
                if(_variables[element] > varIndex)
                {
                    _variables[element]--;
                }
            }
            int oldLength = _data.GetLength(0);
            int newLength = oldLength - 1;
            var newData = new TDataType[newLength, newLength];
            
            for (int i = 0; i < varIndex; i++)
            {
                for (int j = 0; j < varIndex; j++)
                {
                    newData[i, j] = _data[i, j];
                }
                for (int j = varIndex; j < newLength; j++)
                {
                    newData[i, j] = _data[i, j + 1];
                }
            }
            for (int i = varIndex; i < newLength; i++)
            {
                for (int j = 0; j < varIndex; j++)
                {
                    newData[i, j] = _data[i + 1, j];
                }
                for (int j = varIndex; j < newLength; j++)
                {
                    newData[i, j] = _data[i + 1, j + 1];
                }
            }
            _data = newData;
        }


        
    }
}
