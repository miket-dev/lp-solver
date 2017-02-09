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

		public override SeidelResult Resolve(HalfSpace halfSpace)
		{
			throw new NotImplementedException();
		}
	}
}
