using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		/// Decreases the dimension
		/// </summary>
		TResult MoveDown();
	}
}