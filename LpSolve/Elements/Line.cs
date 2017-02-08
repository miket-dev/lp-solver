using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Interface;

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
				return this._vector.X * this._point.X + this._vector.Y + this._point.Y;
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

		public Line MoveDown()
		{
			return new Line(this._point.MoveDown(), this._vector.MoveDown());
		}

		public int GetDimension()
		{
			return this._point.GetDimension();
		}

		/// <summary>
		/// Valid for 2 dimensions
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public Point Intersect(Line line)
		{
			var den = this._vector.X * line._vector.Y - this._vector.Y * line.Vector.X;

			if (den == 0)
				return null;

			var resultArray = new double[line.GetDimension()];

			var x = (this.C * line.Vector.Y - this.Vector.Y * line.C) / den;
			var y = (this._vector.X * line.C - this.C * line.Vector.X) / den;

			resultArray[0] = x;
			resultArray[1] = y;

			if (line.GetDimension() == 3)
			{
				var z1 = -(this._vector.X * x + this._vector.Y * y + this.C);
				var z2 = -(line._vector.X * x + line._vector.Y * y + line.C);

				if (z1 != z2)
					return null;

				resultArray[2] = z1;
			}

			return new Point(resultArray);
		}

		public static Line CreateFromPoints(Point p0, Point p1)
		{
            var lineVector = Vector.CreateFromPoints(p0, p1);

			return new Line(p0, lineVector.Rotate());
		}
	}
}
