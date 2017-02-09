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
		public Point[] AmbigousPoints { get; private set; }

		public AmbigousSeidelResult(Point[] points)
		{
			this.AmbigousPoints = points;
		}

		public override SeidelResult Resolve(HalfSpace halfSpace)
		{
			throw new NotImplementedException();
		}
	}
}
