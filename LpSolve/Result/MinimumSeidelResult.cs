using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Elements;

namespace LpSolve.Result
{
	public class MinimumSeidelResult : SeidelResult
	{
		public MinimumSeidelResult(Point minimumPoint)
		{
			this.Point = minimumPoint;
		}

		public override SeidelResult Resolve(Polyhedron polyhedron, Vector vector, bool containsAny)
		{
			//simply check if current result satisfies the new constraint
			var lastHalfSpace = polyhedron.HalfSpaces[polyhedron.HalfSpaces.Count - 1];

			if (lastHalfSpace.Contains(this.Point))
			{
				return this;
			}

			polyhedron.RecountVertices();
			return this.SolveRecursive(polyhedron, vector);
		}
	}
}
