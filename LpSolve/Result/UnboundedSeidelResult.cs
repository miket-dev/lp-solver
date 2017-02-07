using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Elements;

namespace LpSolve.Result
{
	public class UnboundedSeidelResult : SeidelResult
	{
		public override SeidelResult Resolve(Polyhedron polyhedron, Vector vector, bool containsAny)
		{
			//there are halfspaces but they do not form polyhedron - infeasible
			if (!polyhedron.Vertices.Any() && polyhedron.HalfSpaces.Count > 1)
			{
				return new InfeasibleSeidelResult();
			}

			//there are halfspaces and they do not form polyhedron - unbounded
			if (!polyhedron.Vertices.Any())
			{
				return this;
			}

			if (!containsAny)
			{
				return this.ExtractMinimum(polyhedron, vector);
			}

			return this.SolveRecursive(polyhedron, vector);
		}
	}
}
