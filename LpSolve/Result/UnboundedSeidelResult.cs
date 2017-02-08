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
			if (!containsAny)
			{
				return this.ExtractMinimum(polyhedron, vector);
			}

			return this.SolveRecursive(polyhedron, vector);
		}
	}
}
