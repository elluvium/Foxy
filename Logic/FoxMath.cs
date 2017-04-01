using MathNet.Numerics.LinearAlgebra;
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
        public static double[,] ExponentiateMatrix(double[,] matrix)
        {
            var temp = Matrix<double>.Build.DenseOfArray(matrix);
            temp.Multiply(temp, temp);

            return temp.ToArray();
        }


        public static int[][,] ExponentiateTillZero(int[,] matrix)
        {
            var result = new List<int[,]>();
            result.Add(matrix);
            result.AddRange(ExponentiateTillZero(matrix, matrix));
            return result.ToArray();
        }

        private static List<int[,]> ExponentiateTillZero(int[,] initial, int[,] current)
        {
            List<int[,]> result = new List<int[,]>();
            current = MultiplyWithoutCheckingDimensions(current, initial);
            result.Add(current);
            if (!current.IfMatrixIsZero())
            {
                result.AddRange(ExponentiateTillZero(initial, current));
            }
            return result;
        }

        private static int[,] MultiplyWithoutCheckingDimensions(int[,] M1, int[,] M2)
        {
            int[,] result = new int[M1.GetLength(0), M1.GetLength(1)];
            for (int i = 0; i < M1.GetLength(0); i++)
            {
                for (int j = 0; j < M2.GetLength(1); j++)
                {
                    int sum = 0;
                    for (int k = 0; k < M1.GetLength(1); k++)
                    {
                        sum += M1[i, k] * M2[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }

        public static int[,] Multiply(int[,] M1, int[,] M2)
        {
            if(M1.GetLength(1) != M2.GetLength(0))
            {
                throw new ArgumentException();
            }
            return MultiplyWithoutCheckingDimensions(M1, M2);
        }

        private static bool IfMatrixIsZero(this int[,] matrix)
        {
            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if(matrix[i,j] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }        
    }
}
