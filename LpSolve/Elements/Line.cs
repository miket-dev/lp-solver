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

			var x = (this.C * line.Vector.Y - this.Vector.Y * line.C) / den;
			var y = (this._vector.X * line.C - this.C * line.Vector.X) / den;

			return new Point(new double[] { x, y });
		}

		public static Line CreateFromPoints(Point p0, Point p1)
		{
			return new Line(p0, Vector.CreateFromPoints(p0, p1));
		}
	}
}
