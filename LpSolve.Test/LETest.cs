using System;
using MathExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LpSolve.Test
{
	[TestClass]
	public class LETest
	{
		[TestMethod]
		public void LE_SimpleTest()
		{
			//|x + 3y = 3
			//|-x - 2y = 4

			//y == 7
			//x == -18

			var matrixA = Matrix.Create(new double[][] {
				new double[] { 1, 3 },
				new double[] { -1, -2}
			});

			var matrixB = Matrix.Create(new double[][] {
				new double[] {3},
				new double[] {4}
			});

			var solver = new LESolver();

			var result = solver.Solve(matrixA, matrixB);

			Assert.AreEqual(-18, result[0]);
			Assert.AreEqual(7, result[1]);
		}

		[TestMethod]
		public void LE_Seidel_Case1()
		{
			//|x - y = 3
			//|-3x +5y = 15

			//y == 12
			//x == 15

			var matrixA = Matrix.Create(new double[][] {
				new double[] { 1, -1 },
				new double[] { -3, +5 }
			});

			var matrixB = Matrix.Create(new double[][] {
				new double[] {3},
				new double[] {15}
			});

			var solver = new LESolver();

			var result = solver.Solve(matrixA, matrixB);

			Assert.AreEqual(15, result[0]);
			Assert.AreEqual(12, result[1]);
		}

		[TestMethod]
		public void LE_Seidel_Case2()
		{
			//|5x+3y=30
			//|x-y=3
			//x == 4.875
			//y == 1.875

			var matrixA = Matrix.Create(new double[][] {
				new double[] { 5, 3 },
				new double[] { 1, -1 }
			});

			var matrixB = Matrix.Create(new double[][] {
				new double[] {30},
				new double[] {3}
			});

			var solver = new LESolver();

			var result = solver.Solve(matrixA, matrixB);

			Assert.AreEqual(4.875, result[0]);
			Assert.AreEqual(1.875, result[1]);
		}
	}
}
