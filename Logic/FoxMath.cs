﻿using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    /// Contains static methods to be used to calculate all math in lab tasks.
    /// </summary>
    public static class FoxMath
    {
        /// <summary>
        /// Multiplies given matrix by itself one time.
        /// </summary>
        /// <param name="matrix">Square matrix of Int32's</param>
        /// <returns>(input matrix) ^ 2</returns>
        public static int[,] ExponentiateMatrix(int[,] matrix) // TODO Add data-driven unit-tests
        {
            var temp = Matrix<int>.Build.DenseOfArray(matrix);
            temp.Multiply(temp);

            return temp.ToArray();
        }
    }
}