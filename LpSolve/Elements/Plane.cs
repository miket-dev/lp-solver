using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Elements;
using LpSolve.Interface;
using MathExt;

namespace LpSolve.Elements
{
	public class Plane : IElement<Plane>
	{
		private double? _d;
		private Point _point;
		private Vector _vector;

		public Point Point { get { return this._point; } }
		public Vector Vector { get { return this._vector; } }

		public Plane(Point point, Vector vector)
		{
			if (point.GetDimension() != vector.GetDimension())
			{
				throw new ArgumentException(string.Format("Point{{0}} and Vector{{1}} are of different dimensions!", point.GetDimension(), vector.GetDimension()));
			}

			this._point = point;
			this._vector = vector;
		}

		public double D
		{
			get
			{
				if (this._d == null)
				{
					this._d = 0;

					var size = this.Point.GetDimension();
					for (int i = 0; i < size; i++)
					{
						this._d += this.Point.GetAt(i) * this.Vector.GetAt(i);
					}
				}

				return this._d.Value;
			}
		}

		public Plane MoveDown(Plane plane)
		{
			if (this.GetDimension() == 2)
			{

				var crossProduct = this._vector.CrossProduct(plane._vector);

				if (crossProduct.Length == 0)
				{
					return null;
				}

				var point = this._point.MoveDown(plane, this._vector);
				var vec = this._vector.MoveDown(plane);

				return new Plane(point, vec);
			}

			if (this.GetDimension() == 3)
			{
				//plane1 Ax+By+Cz-D = 0
				//plane2 A1x+B1y+C1z-D1 = 0

				var solver = new LESolver();

				//set x == 0
				var x = 0.0;
				var matrixA = Matrix.Create(new double[][] { 
						new double[] {this._vector.Y, this._vector.Z},
						new double[] {plane.Vector.Y, plane.Vector.Z}
					});

				var matrixB = Matrix.Create(new double[][] {
						new double[] {this.D},
						new double[] {plane.D}
					});

				var result = solver.Solve(matrixA, matrixB);

				var y = result[0];
				var z = result[1];

				//if smth of this is infinity, set y to 0
				if (double.IsInfinity(y) || double.IsInfinity(z) ||
					double.IsNaN(y) || double.IsNaN(z))
				{
					y = 0.0;
					matrixA = Matrix.Create(new double[][] { 
						new double[] {this._vector.X, this._vector.Z},
						new double[] {plane.Vector.X, plane.Vector.Z}
					});

					matrixB = Matrix.Create(new double[][] {
						new double[] {this.D},
						new double[] {plane.D}
					});

					result = solver.Solve(matrixA, matrixB);

					x = result[0];
					z = result[1];
				}

				if (double.IsInfinity(x) || double.IsInfinity(z) ||
					double.IsNaN(x) || double.IsNaN(z))
				{
					//set z to 0
					z = 0.0;
					matrixA = Matrix.Create(new double[][] { 
						new double[] {this._vector.X, this._vector.Y},
						new double[] {plane.Vector.X, plane.Vector.Y}
					});

					matrixB = Matrix.Create(new double[][] {
						new double[] {this.D},
						new double[] {plane.D}
					});

					result = solver.Solve(matrixA, matrixB);

					x = result[0];
					y = result[1];
				}

				if (double.IsInfinity(x) || double.IsInfinity(y) || double.IsInfinity(z) ||
					double.IsNaN(x) || double.IsNaN(y) || double.IsNaN(z))
				{
					//there is no intersection
					return null;
				}

				return new Plane(
						new Point(new double[] { x, y }, new Point(new double[] { x, y, z })),
						new Vector(new double[] { this.Vector.X, this.Vector.Y })
					);
			}

			return null;
		}

		public int GetDimension()
		{
			return this._point.GetDimension();
		}
	}
}
