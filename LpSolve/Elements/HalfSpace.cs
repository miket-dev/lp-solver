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

		public Plane Plane { get { return this._plane; } }

		public HalfSpace(Plane plane, bool isOnPositive)
		{
			this._plane = plane;

			if (!isOnPositive)
			{
				this._plane.Vector.Flip();
			}
		}

		public bool Contains(Point p)
		{
			var vect = Vector.CreateFromPoints(this._plane.Point, p);

			return vect.ScalarProduct(this._plane.Vector) >= 0;
		}

		public HalfSpace MoveDown(Plane plane)
		{
			var pl = this._plane.MoveDown(plane);

			if (pl == null)
			{
				return null;
			}

			return new HalfSpace(pl, true);
		}

		public int GetDimension()
		{
			return this._plane.GetDimension();
		}
	}
}