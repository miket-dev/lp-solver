using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Elements;

namespace LpSolve.Result
{
	public class InfeasibleSeidelResult : SeidelResult
	{
		public override SeidelResult Resolve(HalfSpace halfSpace)
		{
			//if on the various step it is become infeasible
			//it will be infeasible till the end
			return this;
		}
	}
}
