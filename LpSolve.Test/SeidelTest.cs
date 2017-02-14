using System;
using System.Collections.Generic;
using LpSolve.Elements;
using LpSolve.Result;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LpSolve.Test
{
	//http://www.matburo.ru/ex_mp.php?p1=mpgr

	[TestClass]
	public class CalculationTest
	{
		[TestMethod]
		public void Seidel_SimpleTest()
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
							new Point(new double[] { 3.0, 0.0 }),
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

			//x >= 0
			var halfSpace4 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, 0.0 }),
							new Vector(new double[] { 1.0, 0.0 })
						),
					true
				);

			//y >= 0
			var halfSpace5 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, 0.0 }),
							new Vector(new double[] { 0.0, 1.0 })
						),
					true
				);

			Assert.AreEqual(30.0, halfSpace1.Plane.D);
			Assert.AreEqual(-3.0, halfSpace2.Plane.D);
			Assert.AreEqual(-15.0, halfSpace3.Plane.D);

			//-x+2y -> min
			var solver = new SeidelSolver(new List<HalfSpace> { halfSpace1, halfSpace2, halfSpace3, halfSpace4, halfSpace5 }, new Vector(new double[] { -1.0, 2.0 }));
			solver.Run();

			Assert.IsInstanceOfType(solver.Result, typeof(MinimumSeidelResult));
			Assert.AreEqual(4.875, solver.Result.Point.X);
			Assert.AreEqual(1.875, solver.Result.Point.Y);
		}

		[TestMethod]
		public void Seidel_SimpleTest_2()
		{
			//x<=3
			var halfSpace1 = new HalfSpace(
					new Plane(
							new Point(new double[] { 3.0, 0.0 }),
							new Vector(new double[] { -1.0, 0.0 })
						),
					true
				);
			//x>=-1
			var halfSpace2 = new HalfSpace(
					new Plane(
							new Point(new double[] { -1.0, 0.0 }),
							new Vector(new double[] { 1.0, 0.0 })
						),
					true
				);

			//-2x-3y<=6
			var halfSpace3 = new HalfSpace(
					new Plane(
							new Point(new double[] { -3.0, 0.0 }),
							new Vector(new double[] { -2.0, -3.0 })
						),
					false
				);

			//-x+2y<=6
			var halfSpace4 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0, 3.0 }),
							new Vector(new double[] { -1.0, 2.0 })
						),
					false
				);

			//-2x+y -> min
			var solver = new SeidelSolver(new List<HalfSpace> { halfSpace1, halfSpace2, halfSpace3, halfSpace4 }, new Vector(new double[] { -2.0, 1.0 }));
			solver.Run();

			Assert.IsInstanceOfType(solver.Result, typeof(MinimumSeidelResult));
			Assert.AreEqual(3.0, solver.Result.Point.X);
			Assert.AreEqual(-4.0, solver.Result.Point.Y);
		}

		[TestMethod]
		public void Seidel_EmptyResult()
		{
			//-x + 2y <= 30
			var halfSpace1 = new HalfSpace(
					new Plane(
							new Point(new double[] { -1.0, 0.0 }),
							new Vector(new double[] { -1.0, 0.0 })
						),
					true
				);
			//-5x + y <= 25
			var halfSpace2 = new HalfSpace(
					new Plane(
							new Point(new double[] { 3.0, 0.0 }),
							new Vector(new double[] { 1.0, 0.0 })
						),
					true
				);

			//x <= 0
			var halfSpace4 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, 0.0 }),
							new Vector(new double[] { -1.0, 0.0 })
						),
					true
				);

			//y <= 0
			var halfSpace5 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, 0.0 }),
							new Vector(new double[] { 0.0, -1.0 })
						),
					true
				);

			//x + y -> min
			var solver = new SeidelSolver(new List<HalfSpace> { halfSpace1, halfSpace2, halfSpace4, halfSpace5 }, new Vector(new double[] { 1.0, 1.0 }));
			solver.Run();

			Assert.IsInstanceOfType(solver.Result, typeof(InfeasibleSeidelResult));
		}

		[TestMethod]
		public void Seidel_3D()
		{
			//x + y - z >= 8
			var halfSpace1 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, 0.0, -8.0 }),
							new Vector(new double[] { 1.0, 1.0, -1.0 })
						),
					false
				);
			//x - y + 2z >=2
			var halfSpace2 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, 0.0, 1 }),
							new Vector(new double[] { 1.0, -1.0, 2.0 })
						),
					true
				);

			//-2x-8y+3z >= 1
			var halfSpace4 = new HalfSpace(
					new Plane(
							new Point(new double[] { 4.0, 0.0, 3.0 }),
							new Vector(new double[] { -2.0, -8.0, 3.0 })
						),
					true
				);

			//x >= 0
			var halfSpace5 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, 0.0, 0.0 }),
							new Vector(new double[] { 1.0, 0.0, 0.0 })
						),
					true
				);

			//2x + y - 2z -> min
			var solver = new SeidelSolver(new List<HalfSpace> { halfSpace1, halfSpace2, halfSpace4, halfSpace5 }, new Vector(new double[] { 2.0, 1.0, -2.0 }));
			solver.Run();

			Assert.IsInstanceOfType(solver.Result, typeof(UnboundedSeidelResult));
		}

		[TestMethod]
		public void Seidel_3D_2()
		{
			//y + z >= 4
			var halfSpace1 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, 0.0, 4.0 }),
							new Vector(new double[] { 0.0, 1.0, 1.0 })
						),
					true
				);
			//2x + y + 2z >=6
			var halfSpace2 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, 0.0, -3.0 }),
							new Vector(new double[] { 2.0, 1.0, 2.0 })
						),
					true
				);

			//2x-y+2z >= 2
			var halfSpace4 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, 0.0, 1.0 }),
							new Vector(new double[] { 2.0, -1.0, 2.0 })
						),
					true
				);

			//x >= 0
			var halfSpace5 = new HalfSpace(
					new Plane(
							new Point(new double[] { 0.0, 0.0, 0.0 }),
							new Vector(new double[] { 1.0, 0.0, 0.0 })
						),
					true
				);

			//3x + 2y + z -> min
			var solver = new SeidelSolver(new List<HalfSpace> { halfSpace1, halfSpace2, halfSpace4, halfSpace5 }, new Vector(new double[] { 3.0, 2.0, 1.0 }));
			solver.Run();

			Assert.IsInstanceOfType(solver.Result, typeof(MinimumSeidelResult));
			Assert.AreEqual(0.0, solver.Result.Point.X);
			Assert.AreEqual(0.0, solver.Result.Point.Y);
			Assert.AreEqual(4.0, solver.Result.Point.Z);
		}

	}
}
