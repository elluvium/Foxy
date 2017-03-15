using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Matrixes
{
    [Serializable]
    public class NamedMatrix<TVariable, TDataType>
    {
        private TDataType[,] _data;
        public TDataType[,] GetDataArray() => _data;

        private IDictionary<TVariable, int> _variables;
        public TVariable[] Variables => _variables.Keys.ToArray();

        public NamedMatrix(HashSet<TVariable> variables)
        {
            _data = new TDataType[variables.Count, variables.Count];
            InitVariables(variables.ToArray());
        }
        public NamedMatrix(TDataType[,] data, HashSet<TVariable> variables)
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
                return _data[_variables[row], _variables[column]];
            }
            set
            {
                _data[_variables[row], _variables[column]] = value;
            }
        }

    }
}
