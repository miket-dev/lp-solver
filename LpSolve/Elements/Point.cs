using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Interface;

namespace LpSolve.Elements
{
	public class Point : IElement<Point>
	{
		public double X { get { return this.GetAt(0); } }
		public double Y { get { return this.GetAt(1); } }
		public double Z { get { return this.GetAt(2); } }

		private double[] _coordinates;

		public Point(double[] coordinates)
		{
			this._coordinates = coordinates;
		}

		public double GetAt(int index)
		{
			if (this._coordinates.Length < index)
			{
				throw new ArgumentException("Point does not contain data for dimension " + (index + 1));
			}

			return this._coordinates[index];
		}

#if DEBUG
		public override string ToString()
		{
			var format = "(";

			for (int i = 0; i < this._coordinates.Length; i++)
			{
				format += "{" + i + ":n2},";
			}

			format = format.Substring(0, format.Length - 1) + ")";

			return string.Format(format, this._coordinates);
		}
#endif

		public override int GetHashCode()
		{
			return 0;
		}

		public override bool Equals(object obj)
		{
			var p = obj as Point;
			if (p == null)
				return false;

			var result = true;

			for (int i = 0; i < this.GetDimension(); i++)
			{
				result &= this.GetAt(i) == p.GetAt(i);
			}

			return result;
		}

		public Point MoveDown(Plane plane)
		{
			var line = new Line(this, Vector.CreateFromPoints(this, plane.Point));
			//vector does not matter, it is a project and not a real intersection

			var result = line.IntersectPlane(plane);

			var newResult = new double[result.GetDimension() - 1];
			for (int i = 0; i < newResult.Length; i++)
			{
				newResult[0] = result.GetAt(i);
			}

			return new Point(newResult);
		}

		public Point AddVector(Vector vector)
		{
			var coords = new double[this.GetDimension()];

			for (int i = 0; i < coords.Length; i++)
			{
				coords[i] = this.GetAt(i) + vector.GetAt(i);
			}

			return new Point(coords);
		}

		public int GetDimension()
		{
			return this._coordinates.Length;
		}
	}
}
