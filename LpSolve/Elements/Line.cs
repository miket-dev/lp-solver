using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Interface;
using MathExt;

namespace LpSolve.Elements
{
	public class Line : IElement<Line>
	{
		private Point _point;
		private Vector _vector;

		public Point Point { get { return this._point; } }
		public Vector Vector { get { return this._vector; } }

		public double C
		{
			get
			{
				return this._vector.X * this._point.X + this._vector.Y * this._point.Y;
			}
		}

		public Line(Point point, Vector vector)
		{
			if (point.GetDimension() != vector.GetDimension())
			{
				throw new ArgumentException(string.Format("Point and Vector ({{0}} and {{1}}) are of different dimensions!", point.GetDimension(), vector.GetDimension()));
			}

			this._point = point;
			this._vector = vector;
		}

		public Line MoveDown(Plane plane)
		{
			var point = this._point.MoveDown(plane, this._vector);
			var vec = this._vector.MoveDown(plane);
			return new Line(point, vec);
		}

		public int GetDimension()
		{
			return this._point.GetDimension();
		}

		public Point IntersectPlane(Plane plane)
		{
			var fakeLinePoint = this._point;
			var fakeLineVector = this._vector;

			if (plane.GetDimension() == 2)
			{
				//it is the line
				//|ax + by - c = 0
				//|a1x + b1y - c1 = 0

				var matrixA = Matrix.Create(new double[][] { 
					new double[] { this._vector.X, this._vector.Y },
					new double[] { plane.Vector.X, plane.Vector.Y}
				});

				var matrixB = Matrix.Create(new double[][] {
					new double[] { this.C },
					new double[] { plane.D}
				});

				var solver = new LESolver();

				var result = solver.Solve(matrixA, matrixB);

				return new Point(result, this._point.ParentPoint);
			}

			if (plane.GetDimension() == 3)
			{

			}

			throw new NotImplementedException("Implemented only for 2d");
		}
	}
}
