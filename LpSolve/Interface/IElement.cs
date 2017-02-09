using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Elements;

namespace LpSolve.Interface
{
	public interface IElement
	{
		int GetDimension();
	}

	public interface IElement<TResult> : IElement
		where TResult : IElement
	{
		/// <summary>
		/// Projects element on passed n-dimensional plane
		/// </summary>
		TResult MoveDown(Plane plane);
	}
}