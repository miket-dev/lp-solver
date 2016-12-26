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
		public bool Exists { get; set; }

		public Plane(Point point, Vector vector)
		{
			if (point.GetDimension() != vector.GetDimension())
			{
				throw new ArgumentException(string.Format("Point{{0}} and Vector{{1}} are of different dimensions!", point.GetDimension(), vector.GetDimension()));
			}

			this._point = point;
			this._vector = vector;

			this.Exists = true;
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
						this._d -= this.Point.GetAt(i) * this.Vector.GetAt(i);
					}
				}

				return this._d.Value;
			}
		}

		public int GetDimension()
		{
			return this._point.GetDimension();
		}

		public Plane MoveDown()
		{
			switch (this._point.GetDimension())
			{
				case 3:
					{
						//searching intersect line between this plane and Oxy
						//|ax+by+cz+d=0;
						//|z=0;

						//y = (-ax - d) / b
						if (this._vector.GetAt(1) != 0.0)
						{
							var y = (this._vector.GetAt(0) - this.D) / this._vector.GetAt(1);
							var x = 0.0;
							if (this._vector.GetAt(0) != 0.0)
							{
								x = (y * this._vector.GetAt(1) + this.D) / (-this._vector.GetAt(0));
							}

							return new Plane(new Point(new double[] { x, y }), this._vector.MoveDown());
						}
						else if (this._vector.GetAt(0) != 0.0)
						{
							var x = (this._vector.GetAt(1) - this.D) / this._vector.GetAt(0);
							var y = 0.0;
							if (this._vector.GetAt(1) != 0.0)
							{
								y = (x * this._vector.GetAt(0) + this.D) / (-this._vector.GetAt(1));
							}

							return new Plane(new Point(new double[] { x, y }), this._vector.MoveDown());
						}
						else
						{
							var plane = new Plane(this._point.MoveDown(), this._vector.MoveDown());
							plane.Exists = false;
							return plane;
						}
					}
				case 2:
					{
						//|y = ax + d;
						//|y = 0;
						if (this._vector.GetAt(0) == 0.0)
						{
							var plane = new Plane(this._point.MoveDown(), this._vector.MoveDown());
							plane.Exists = false;
							return plane;
						}
						else
						{
							var x = this.D / this._vector.GetAt(0);
							return new Plane(new Point(new double[] { x }), this._vector.MoveDown());
						}
					}
				default:
					//TODO: implement
					throw new NotImplementedException("Implemented only for 2 and 3 dimensions");
			}
		}
	}
}
