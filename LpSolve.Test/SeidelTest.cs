using System;
using System.Collections.Generic;
using LpSolve.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LpSolve.Test
{
	[TestClass]
	public class CalculationTest
	{
		[TestMethod]
		public void Seidel_SimpleTest_9()
		{
			//5x+3y>=30
			var halfSpace1 = new HalfSpace(
					new Plane(
							new Point(new double[] { 6.0, 0.0 }),
							new Vector(new double[] { 5.0, 3.0 })
						),
					true
				);
			//x-y<=3
			var halfSpace2 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, -3.0 }),
							new Vector(new double[] { 1.0, -1.0 })
						),
					false
				);

			//-3x+5y<=15
			var halfSpace3 = new HalfSpace(
					new Plane(
							new Point(new double[] { 5.0, 6.0 }),
							new Vector(new double[] { -3.0, 5.0 })
						),
					false
				);

			Assert.AreEqual(-30.0, halfSpace1.Plane.D);
			Assert.AreEqual(-3.0, halfSpace2.Plane.D);
			Assert.AreEqual(-15.0, halfSpace3.Plane.D);


			var solver = new SeidelSolver(new List<HalfSpace> { halfSpace1, halfSpace2, halfSpace3 }, new Vector(new double[] { 1.0, -2.0 }));
			solver.Run();

			Assert.AreEqual(SeidelResultEnum.Minimum, solver.ResultType);
			Assert.AreEqual(15.0, solver.ResultPoint.X);
			Assert.AreEqual(12.0, solver.ResultPoint.Y);
		}
	}
}
