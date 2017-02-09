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
			while (this._halfSpaces.Any() && !(this._result is InfeasibleSeidelResult))
			{
				var space = this._halfSpaces[_random.Next(this._halfSpaces.Count)];

				this._halfSpaces.Remove(space);
				Iterate(space);
			}
		}

		private void Iterate(HalfSpace halfSpace)
		{
			if (this._vector.GetDimension() != 1)
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

					passSpaces.Add(passItem);
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
					var point = tempResult.Point;

					throw new ApplicationException("Moving up is not implemented, check the result");
				}
				else if (tempResult is AmbigousSeidelResult)
				{
					throw new ApplicationException("Moving up is not implemented, check the result");
				}

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
