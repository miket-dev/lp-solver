using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Elements;
using LpSolve.Interface;
using LpSolve.Result;

namespace LpSolve
{
	public class SeidelSolver
	{
		private List<HalfSpace> _halfSpaces;
		private Vector _vector;

		private List<HalfSpace> _workHalfSpaces;
		private FakeRandom _random;
		private SeidelResult _result;

		public SeidelResult Result { get { return this._result; } }

		public SeidelSolver(List<HalfSpace> halfSpaces, Vector vector)
		{
			var vectorSize = vector.GetDimension();

			this._halfSpaces = halfSpaces;
			this._vector = vector;

			this._random = new FakeRandom(DateTime.Now.Millisecond);
			this._workHalfSpaces = new List<HalfSpace>();
		}

		public void Run()
		{
			if (this._halfSpaces.Any() && this._halfSpaces[0].GetDimension() == 1)
			{
				this.Resolve1D();
			}
			else
			{
				var space = this._halfSpaces[_random.Next(this._halfSpaces.Count)];

				this._halfSpaces.Remove(space);
				Iterate(space);
			}
		}

		private void Resolve1D()
		{
			var set = new List<Tuple<double, bool>>();

			foreach (var item in this._halfSpaces)
			{
				set.Add(Tuple.Create(item.Plane.Point.X, item.Plane.Vector.X > 0));
			}

			var orderedSet = set.OrderBy(x => x.Item1).ToList();

			if (!orderedSet[0].Item2 && orderedSet.Any(x => x.Item2))
			{
				this._result = new InfeasibleSeidelResult();
				return;
			}
			//or if begins with true, when meets false and after it true again
			else if (orderedSet[0].Item2)
			{
				var start = true;

				foreach (var item in orderedSet)
				{
					if (!start && item.Item2)
					{
						this._result = new InfeasibleSeidelResult();
						return;
					}

					start = item.Item2;
				}
			}

			if ((orderedSet[orderedSet.Count - 1].Item2 && this._vector.X < 0)
				|| (!orderedSet[0].Item2 && this._vector.X > 0))
			{
				this._result = new UnboundedSeidelResult();

				return;
			}

			if (orderedSet[orderedSet.Count - 1].Item2 && this._vector.X > 0)
			{
				var minimumPoint = orderedSet[orderedSet.Count - 1].Item1;

				orderedSet.RemoveAt(orderedSet.Count - 1);

				foreach (var item in orderedSet)
				{
					if (item.Item1 == minimumPoint)
					{
						this._result = new AmbigousSeidelResult(new Point[] { new Point(new double[] { minimumPoint }) });

						return;
					}
				}

				Point parentPoint = null;
				foreach (var item in this._halfSpaces)
				{
					if (item.Plane.Point.X == minimumPoint)
					{
						parentPoint = item.Plane.Point.ParentPoint;
						break;
					}
				}

				this._result = new MinimumSeidelResult(new Point(new double[] { minimumPoint }, parentPoint));
				return;
			}

			if (!orderedSet[0].Item2 && this._vector.X < 0)
			{
				var minimumPoint = orderedSet[0].Item1;

				orderedSet.RemoveAt(0);

				foreach (var item in orderedSet)
				{
					if (item.Item1 == minimumPoint)
					{
						this._result = new AmbigousSeidelResult(new Point[] { new Point(new double[] { minimumPoint }) });

						return;
					}
				}

				Point parentPoint = null;
				foreach (var item in this._halfSpaces)
				{
					if (item.Plane.Point.X == minimumPoint)
					{
						parentPoint = item.Plane.Point.ParentPoint;
						break;
					}
				}

				this._result = new MinimumSeidelResult(new Point(new double[] { minimumPoint }, parentPoint));
				return;
			}

			if (orderedSet[0].Item2 && !orderedSet[orderedSet.Count - 1].Item2)
			{
				var start = true;

				for (int i = 0; i < orderedSet.Count; i++)
				{
					var item = orderedSet[i];
					if (start && !item.Item2)
					{
						Point parentPoint = null;
						var minimumPoint = orderedSet[i - 1].Item1;
						foreach (var space in this._halfSpaces)
						{
							if (space.Plane.Point.X == minimumPoint)
							{
								parentPoint = space.Plane.Point.ParentPoint;
								break;
							}
						}

						this._result = new MinimumSeidelResult(new Point(new double[] { minimumPoint }, parentPoint));
						return;
					}

					start = item.Item2;
				}
			}
		}

		private void Iterate(HalfSpace halfSpace)
		{
			var plane = halfSpace.Plane;

			var passSpaces = new List<HalfSpace>();
			foreach (var item in this._halfSpaces)
			{
				var passItem = item.MoveDown(plane);

				if (passItem == null)
				{
					if (!halfSpace.Contains(item.Plane.Point) ||
						!item.Contains(plane.Point))
					{
						this._result = new InfeasibleSeidelResult();

						return;
					}
				}
				else
				{
					passSpaces.Add(passItem);
				}
			}

			this._workHalfSpaces.Add(halfSpace);

			var innerSolver = new SeidelSolver(passSpaces, this._vector.MoveDown(plane));
			innerSolver.Run();

			var tempResult = innerSolver.Result;

			if (tempResult is InfeasibleSeidelResult ||
				tempResult is UnboundedSeidelResult)
			{
				this._result = tempResult;
				return;
			}
			else if (tempResult is MinimumSeidelResult)
			{
				//move up the point
				var point = tempResult.Point.ParentPoint;

				this._result = new MinimumSeidelResult(point);
				return;
			}
			else if (tempResult is AmbigousSeidelResult)
			{
				throw new ApplicationException("Moving up is not implemented, check the result");
			}
		}

		private class FakeRandom
		{
			public FakeRandom(int seed)
			{

			}

			public int Next(int count)
			{
				return 0;
			}
		}
	}
}
