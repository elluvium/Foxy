using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Tests
{
    [TestClass()]
    public class FoxMathTests
    {
        // TODO Implement data-driven unit test for ExponentiateMatrix
        //i can't understand what test should do exactly(boolship), below i give you my variant of implementation 
        [TestMethod()]
        public void ExponentiateMatrixTest_ReturnMultiplicateMatrix()
        {
            double[,] matrix = { {1, 1, 1}, {0, 0, 1}, {0, 0, 1} };
            double[,] expectedMatrix = { { 1, 1, 3 }, { 0, 0, 1 }, { 0, 0, 1 } };
            
            var actualMatrix = FoxMath.ExponentiateMatrix(matrix);
            bool equal = actualMatrix.Rank == expectedMatrix.Rank && Enumerable.Range(0, actualMatrix.Rank).All(dimension => actualMatrix.GetLength(dimension) == expectedMatrix.GetLength(dimension)) && 
                actualMatrix.Cast<double>().SequenceEqual(expectedMatrix.Cast<double>());

            Assert.AreEqual(equal, true);
        }

        [TestMethod()]
        public void ExponentiateMatrix_ReturnMultiplicateMatrix_IfInitDataValid()
        {
            double[,] initialMatrix = 
            {
                { 1, 1, 1 },
                { 0, 0, 1 },
                { 0, 0, 1 }
            };

            double[,] expectedMatrix = 
            { 
                { 1, 1, 3 },
                { 0, 0, 1 },
                { 0, 0, 1 }
            };

            var matrix = FoxMath.ExponentiateMatrix(initialMatrix);
            var matrixToArray = matrix.Cast<double>().ToArray();
            var expectedMatrixToArray = expectedMatrix.Cast<double>().ToArray();

            var isEquals = matrixToArray.SequenceEqual(expectedMatrixToArray);

            Assert.IsTrue(isEquals);
        }
    }
}