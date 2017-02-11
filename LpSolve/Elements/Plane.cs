using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Elements;
using LpSolve.Interface;

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
			var crossProduct = this._vector.CrossProduct(plane._vector);

			if (crossProduct.Length == 0)
			{
				return null;
			}

			var point = this._point.MoveDown(plane, this._vector);
			var vec = this._vector.MoveDown(plane);

			return new Plane(point, vec);
		}

		public int GetDimension()
		{
			return this._point.GetDimension();
		}
	}
}
