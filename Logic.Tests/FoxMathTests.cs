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
        [TestMethod()]
        public void ExponentiateMatrixTest()
        {
            double[,] matrix = { {1, 1, 1}, {0, 0, 1}, {0, 0, 1} };
            double[,] expectedMatrix = { { 1, 1, 3 }, { 0, 0, 1 }, { 0, 0, 1 } };
            
            var actualMatrix = FoxMath.ExponentiateMatrix(matrix);
            bool equal = actualMatrix.Rank == expectedMatrix.Rank && Enumerable.Range(0, actualMatrix.Rank).All(dimension => actualMatrix.GetLength(dimension) == expectedMatrix.GetLength(dimension)) && 
                actualMatrix.Cast<double>().SequenceEqual(expectedMatrix.Cast<double>());

            Assert.AreEqual(equal, true);
        }
    }
}