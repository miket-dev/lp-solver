using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Elements;

namespace LpSolve.Result
{
	public class AmbigousSeidelResult : SeidelResult
	{
		public override SeidelResult Resolve(Polyhedron polyhedron, Vector vector, bool containsAny)
		{
			//just recalculate minimum
			return this.SolveRecursive(polyhedron, vector);
		}
	}
}
