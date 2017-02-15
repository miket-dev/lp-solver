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
		private Random _random;
		private SeidelResult _result;

		public SeidelResult Result { get { return this._result; } }

		public SeidelSolver(List<HalfSpace> halfSpaces, Vector vector)
		{
			var vectorSize = vector.GetDimension();

			this._halfSpaces = halfSpaces;
			this._vector = vector;

			this._random = new Random(DateTime.Now.Millisecond);
			this._workHalfSpaces = new List<HalfSpace>();
			this._result = new UnboundedSeidelResult();
		}

		public void Run()
		{
			if (this._halfSpaces.Any() && this._halfSpaces[0].GetDimension() == 1)
			{
				this.Resolve1D();
			}
			else if (this._halfSpaces.Any())
			{
				while (this._halfSpaces.Any() && (!(this._result is InfeasibleSeidelResult) || this._result == null))
				{
					var nextItem = _random.Next(this._halfSpaces.Count);

#if DEBUG
					Console.WriteLine(nextItem);
#endif

					var space = this._halfSpaces[nextItem];

					Iterate(space);
				}
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
						Point parentPoint = null;
						foreach (var space in this._halfSpaces)
						{
							if (space.Plane.Point.X == minimumPoint)
							{
								parentPoint = space.Plane.Point.ParentPoint;
								break;
							}
						}

						this._result = new AmbigousSeidelResult(new Point[] { new Point(new double[] { minimumPoint }, parentPoint) });

						return;
					}
				}

				{
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

			this._result = new UnboundedSeidelResult();
		}

		private void Iterate(HalfSpace halfSpace)
		{
			this._halfSpaces.Remove(halfSpace);

			if (this._result is MinimumSeidelResult)
			{
				//simply check if current minimum satisfies the new constaint

				if (!halfSpace.Contains(this._result.Point))
				{
					this._result = new UnboundedSeidelResult();
				}
			}

			if (this._result is AmbigousSeidelResult)
			{
				//check if there is only one satisfied point

				Point minimumPoint = null;
				var minimumCount = 0;

				foreach (var item in ((AmbigousSeidelResult)this._result).AmbigousPoints)
				{
					if (halfSpace.Contains(item))
					{
						minimumCount++;
						minimumPoint = item;
					}
				}

				if (minimumCount == 0)
				{
					this._result = new UnboundedSeidelResult();
				}
				else if (minimumCount == 1)
				{
					this._result = new MinimumSeidelResult(minimumPoint);
				}
			}

			if (this._result is UnboundedSeidelResult)
			{
				this._result = this.FindMinimum(this._workHalfSpaces, halfSpace);
				
				//if all items are in
				//then searching the minimum point in the halfspace,
				//which contains plane with same direction normal to target function
				if (!this._halfSpaces.Any())
				{
					this._workHalfSpaces.Add(halfSpace);
					HalfSpace workSpace = null;
					foreach (var item in this._workHalfSpaces)
					{
						var scalarProduct = this._vector.ScalarProduct(item.Plane.Vector);
						if (scalarProduct >= 0)
						{
							workSpace = item;
							break;
						}
					}

					if (workSpace != null)
					{
						this._workHalfSpaces.Remove(workSpace);

						this._result = this.FindMinimum(this._workHalfSpaces, workSpace);
					}

					this._workHalfSpaces.Remove(halfSpace);
				}
			}

			this._workHalfSpaces.Add(halfSpace);
		}

		private SeidelResult FindMinimum(List<HalfSpace> workSpaces, HalfSpace halfSpace)
		{
			var passItems = new List<HalfSpace>();
			foreach (var item in workSpaces)
			{
				var passItem = item.MoveDown(halfSpace.Plane);

				if (passItem == null)
				{
					if (!halfSpace.Contains(item.Plane.Point) ||
						!item.Contains(halfSpace.Plane.Point))
					{
						return new InfeasibleSeidelResult();
					}
				}
				else
				{
					passItems.Add(passItem);
				}
			}

			//if vectors are not of same direction - the minimum point can not be aligned on it
			if (halfSpace.Plane.Vector.ScalarProduct(this._vector) >= 0)
			{
				var innerSolver = new SeidelSolver(passItems, this._vector.MoveDown(halfSpace.Plane));
				innerSolver.Run();

				var tempResult = innerSolver.Result;

				if (tempResult is MinimumSeidelResult)
				{
					return new MinimumSeidelResult(tempResult.Point.ParentPoint);
				}
				else if (tempResult is AmbigousSeidelResult)
				{
					return new AmbigousSeidelResult(((AmbigousSeidelResult)tempResult).AmbigousPoints.Select(x => x.ParentPoint).ToArray());
				}

				return tempResult;
			}

			return new UnboundedSeidelResult();
		}

		private class FakeRandom
		{
			public FakeRandom(int seed)
			{

			}

			public int Next(int count)
			{
				return count - 1;
			}
		}

		private class FakeRandomSequence
		{
			private static int[] _vals = new int[]{ 2, 0, 0, 1, 1, 0, 0 };
			private static int _counter = 0;

			public FakeRandomSequence(int seed)
			{

			}

			public int Next(int count)
			{
				return _vals[_counter++];
			}
		}
	}
}
