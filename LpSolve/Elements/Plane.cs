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
						this._d += this.Point.GetAt(i) * this.Vector.GetAt(i);
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
			//if this plane can be proected to the target plane then return it
			var d = this.GetDimension();
			var proectPointCoordinates = new double[d];
			for (int i = 0; i < proectPointCoordinates.Length; i++)
			{
				proectPointCoordinates[i] = 0.0;
			}

			var proectVectorCoordinates = new double[d];
			for (int i = 0; i < proectVectorCoordinates.Length; i++)
			{
				proectVectorCoordinates[i] = 0.0;
			}

			proectVectorCoordinates[d - 1] = 1.0;


			if (d == 1)
			{
				throw new ArgumentException("Could not move down from 1 dimension!");
			}

			var result = new Plane(this._point.MoveDown(), this._vector.MoveDown());

			if (this.Intersect(new Plane(new Point(proectPointCoordinates), new Vector(proectVectorCoordinates))) == null)
			{
				result.Exists = false;
			}

			return result;
		}

		/// <summary>
		/// Returns line or point due to dimension of plane
		/// </summary>
		public IElement Intersect(Plane plane)
		{
			switch (this._point.GetDimension())
			{
				case 3:
					{
						//|ax+by+cz-d=0;
						//|a1x+b1y+c1z-d1=0;

						//check if planes are parallel
						if (this.IsNotNull(this._vector.X, this._vector.Y, this._vector.Z, plane.Vector.X, plane.Vector.Y, plane.Vector.Z))
						{
							var coeffX = this._vector.X / plane.Vector.X;
							var coeffY = this._vector.Y / plane.Vector.Y;
							var coeffZ = this._vector.Z / plane.Vector.Z;

							if (coeffX == coeffY && coeffY == coeffZ)
							{
								return null;
							}
						}

						//z==0, reduce planes to lines and check theirs intersection
						var delta = this.Vector.X * plane.Vector.Y - plane.Vector.X * this.Vector.Y;
						if (delta != 0.0)
						{
							var x0 = (plane.Vector.Y * this.D - this.Vector.Y * plane.D) / delta;
							var y0 = (this.Vector.X * plane.D - plane.Vector.X * this.D) / delta;

							var z0 = 0.0;
							if (this._vector.Z != 0)
							{
								z0 = -(this._vector.X * x0 + this._vector.Y * y0 - this.D) / this._vector.Z;
							}

							var crossProduct = this._vector.CrossProduct(plane.Vector);

							return new Line(new Point(new double[] { x0, y0, z0 }), crossProduct);
						}

						return null;
					}
				case 2:
					{
						//|ax+by+d = 0
						//|a1x+b1y+d = 0

						var delta = this.Vector.X * plane.Vector.Y - plane.Vector.X * this.Vector.Y;
						if (delta != 0.0)
						{
							var x0 = (plane.Vector.Y * this.D - this.Vector.Y * plane.D) / delta;
							var y0 = (this.Vector.X * plane.D - plane.Vector.X * this.D) / delta;

							return new Point(new double[] { x0, y0 });
						}

						return null;
					}
				default:
					//TODO: implement
					throw new NotImplementedException("Implemented only for 2 and 3 dimensions");
			}
		}

		private bool IsNotNull(params double[] vals)
		{
			foreach (var item in vals)
			{
				if (item == 0.0)
					return false;
			}

			return true;
		}
	}
}
