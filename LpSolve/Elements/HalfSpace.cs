using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Interface;

namespace LpSolve.Elements
{
	public class HalfSpace : IElement<HalfSpace>
	{
		private Plane _plane;
		private bool _isOnPositive;

		public Plane Plane { get { return this._plane; } }
		public bool IsOnPositive { get { return this._isOnPositive; } }

		public HalfSpace(Plane plane, bool isOnPositive)
		{
			this._plane = plane;
			this._isOnPositive = isOnPositive;
		}

		public bool Contains(Point p)
		{
			var vect = Vector.CreateFromPoints(this._plane.Point, p);

			if (this._isOnPositive)
			{
				return vect.ScalarProduct(this._plane.Vector) >= 0;
			}

			return vect.ScalarProduct(this._plane.Vector) <= 0;
		}

		public IElement Intersect(HalfSpace halfSpace)
		{
			var dimension = this.GetDimension();
			if (dimension == 1) //it is a 1-d ray
			{
				//we do not really know what is the intersection
				return null;
			}

			return this._plane.Intersect(halfSpace._plane);
		}

		public HalfSpace MoveDown()
		{
			return new HalfSpace(this._plane.MoveDown(), this._isOnPositive);
		}

		public int GetDimension()
		{
			return this._plane.GetDimension();
		}
	}
}