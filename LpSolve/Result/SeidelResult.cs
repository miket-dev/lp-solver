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
			var minimumArray = new List<double>();
			var minimumPointArray = new List<Point>();

			foreach (var item in polyhedron.Vertices)
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
			var index = -1;
			var count = 0;
			for (int i = 0; i < minimumArray.Count; i++)
			{
				var item = minimumArray[i];
				if (minimum > item)
				{
					minimum = item;
					index = i;
					count = 0;
				}

				if (index != i && minimum == item)
				{
					index = -1;
					count++;
				}
			}

			if (count > 0)
			{
				return new AmbigousSeidelResult();
			}

			return new MinimumSeidelResult(minimumPointArray[index]);
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
				//searching point on last plane
				var plane = lastHalfSpace.Plane;
				var planeNorm = plane.Vector;

				var result = 0.0;
				for (int i = 0; i < planeNorm.GetDimension(); i++)
				{
					if (i != planeNorm.GetDimension() - 1)
					{
						result -= planeNorm.GetAt(i);
					}
				}

				result += plane.D;
				result /= planeNorm.GetAt(planeNorm.GetDimension() - 1);

				var point = returnResult.Point;
				var resultCoeffs = new double[planeNorm.GetDimension()];
				for (int i = 0; i < point.GetDimension(); i++)
				{
					resultCoeffs[i] = point.GetAt(i);
				}

				resultCoeffs[resultCoeffs.Length - 1] = result;

				return new MinimumSeidelResult(new Point(resultCoeffs));
			}

			return returnResult;
		}
	}
}
