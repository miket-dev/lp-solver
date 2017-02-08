using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Elements;

namespace LpSolve.Result
{
	/// <summary>
	/// Base class for LP result
	/// </summary>
	public abstract class SeidelResult
	{
		public Point Point { get; protected set; }

		public abstract SeidelResult Resolve(Polyhedron polyhedron, Vector vector, bool containsAny);

		protected SeidelResult ExtractMinimum(Polyhedron polyhedron, Vector vector)
		{
			var set = new List<Tuple<double, bool>>(); //point; is to positive, filled only when 1d

			if (vector.GetDimension() == 1)
			{
				//if every halfspace has same direction with the vector - unbounded
				//else searching the minimum
				var directions = new List<double>();

				foreach (var item in polyhedron.HalfSpaces)
				{
					var scalarProduct = item.Plane.Vector.ScalarProduct(vector) * (item.IsOnPositive ? 1.0 : -1.0);

					directions.Add(scalarProduct);

					var point = item.Get1DPoint();
					if (point != null)
					{
						set.Add(Tuple.Create((double)point, (item.Plane.Vector.X > 0 && item.IsOnPositive) || (item.Plane.Vector.X < 0 && !item.IsOnPositive)));
					}
				}

				var orderedSet = set.OrderBy(x => x.Item1).ToList();

				if (!polyhedron.Vertices.Any())
				{
					//infeasible if set begins with false and when it become true
					if (!orderedSet[0].Item2 && orderedSet.Any(x => x.Item2))
					{
						return new InfeasibleSeidelResult();
					}
					//or if begins with true, when meets false and after it true again
					else if (orderedSet[0].Item2)
					{
						var start = true;

						foreach (var item in orderedSet)
						{
							if (!start && item.Item2)
							{
								return new InfeasibleSeidelResult();
							}

							start = item.Item2;
						}
					}
				}

				if ((orderedSet[orderedSet.Count - 1].Item2 && vector.X < 0)
					|| (!orderedSet[0].Item2 && vector.X > 0))
				{
					return new UnboundedSeidelResult();
				}
			}

			var iterateArray = polyhedron.Vertices.Any() ? polyhedron.Vertices : set.Select(x => new Point(new double[] { x.Item1 })).ToList();

			return this.FindMinimum(iterateArray, vector);
		}

		protected SeidelResult SolveRecursive(Polyhedron polyhedron, Vector vector)
		{
			//move one dimension lower
			var passPolyhedron = polyhedron.MoveDown();

			var lastHalfSpace = polyhedron.HalfSpaces[polyhedron.HalfSpaces.Count - 1];

			var innerSolver = new SeidelSolver(passPolyhedron, vector.MoveDown());
			innerSolver.Run();

			if (innerSolver.Result is InfeasibleSeidelResult)
			{
				return new InfeasibleSeidelResult();
			}

			var returnResult = innerSolver.Result;

			if (returnResult is MinimumSeidelResult)
			{
				var resultPoint = this.FindPointOnPlane(lastHalfSpace, returnResult.Point);

				if (resultPoint == null)
				{
					return new UnboundedSeidelResult();
				}

				return new MinimumSeidelResult(resultPoint);
			}

			if (returnResult is AmbigousSeidelResult)
			{
				var ambigousResult = (AmbigousSeidelResult)returnResult;

				var halfSpaces = new List<HalfSpace>();

				var points = new List<Point>();
				foreach (var point in ambigousResult.AmbigousPoints)
				{
					//searching point on planes
					foreach (var halfSpace in polyhedron.HalfSpaces)
					{
						var resultPoint = this.FindPointOnPlane(halfSpace, point);

						if (resultPoint == null)
						{
							continue;
						}

						points.Add(resultPoint);
					}
				}

				return this.FindMinimum(points.Distinct().ToList(), vector);
			}

			return returnResult;
		}

		private Point FindPointOnPlane(HalfSpace halfspace, Point point)
		{
			//searching point on last plane
			var plane = halfspace.Plane;
			var planeNorm = plane.Vector;

			double? result = 0.0;
			for (int i = 0; i < planeNorm.GetDimension(); i++)
			{
				if (i != planeNorm.GetDimension() - 1)
				{
					result -= planeNorm.GetAt(i) * point.GetAt(i);
				}
			}

			result += plane.D;
			var den = planeNorm.GetAt(planeNorm.GetDimension() - 1);

			if (den == 0) //plane is parallel to this dimensional axis
			{
				result = null;
			}
			else
			{
				result /= den;
			}

			var resultCoeffs = new double[planeNorm.GetDimension()];
			for (int i = 0; i < point.GetDimension(); i++)
			{
				resultCoeffs[i] = point.GetAt(i);
			}

			if (result == null)
			{
				return null;
			}

			resultCoeffs[resultCoeffs.Length - 1] = (double)result;

			return new Point(resultCoeffs);
		}

		private SeidelResult FindMinimum(List<Point> iterateArray, Vector vector)
		{
			var minimumArray = new List<double>();
			var minimumPointArray = new List<Point>();

			if (iterateArray.Count == 0)
			{
				return new UnboundedSeidelResult();
			}

			foreach (var item in iterateArray)
			{
				var tempMin = 0.0;
				for (int i = 0; i < vector.GetDimension(); i++)
				{
					tempMin += vector.GetAt(i) * item.GetAt(i);
				}

				minimumArray.Add(tempMin);
				minimumPointArray.Add(item);
			}

			var minimum = double.MaxValue;
			var index = new List<int>();
			var count = 0;
			for (int i = 0; i < minimumArray.Count; i++)
			{
				var item = minimumArray[i];
				if (minimum > item)
				{
					minimum = item;
					index.Clear();
					index.Add(i);
					count = 0;
				}

				if (!index.Contains(i) && minimum == item)
				{
					index.Add(i);
					count++;
				}
			}

			if (count > 0)
			{
				var points = new Point[index.Count];
				for (int i = 0; i < index.Count; i++)
				{
					var item = index[i];
					points[i] = minimumPointArray[item];
				}

				return new AmbigousSeidelResult(points);
			}

			return new MinimumSeidelResult(minimumPointArray[index[0]]);
		}
	}
}
