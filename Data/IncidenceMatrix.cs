﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeLib;

namespace Data
{
    public class IncidenceMatrix<T>
    {
        int[,] _data;

        public int[,] ToArray() => _data;

        public IEnumerable<T> Variables => _variables;

        private IList<T> _variables;

        public IncidenceMatrix(Tree<T> tree)
        {
            _variables = tree.Select(x => x.Value).ToList();
            _data = new int[tree.Count(), tree.Count()];
            foreach(var row in tree)
            {
                foreach (var col in tree)
                {
                    if (row.Descendants.Contains(col) || row == col)
                    {
                        this[row.Value, col.Value] = 1;
                    }
                    else
                    {
                        this[row.Value, col.Value] = 0;
                    }
                }

            }
        }

        public int this[T row,T column]
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
                _data[rowIndex, colIndex] = value;
            }
        }
            

            
    }
}
