using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExt
{
	public class LESolver
	{
		/// <summary>
		/// Solves equations that in form Ax=B, where A, B are matrices
		/// </summary>
		public double[] Solve(Matrix A, Matrix B)
		{
			if (B.Rank != 1)
				throw new ArgumentException("Matrix B should have Rank == 1");

			var detA = A.Determinant();

			var addDetA = new double[A.Rank];
			for (int i = 0; i < A.Rank; i++)
			{
				addDetA[i] = A.Replace(i, B).Determinant();
			}

			var result = new double[A.Rank];
			for (int i = 0; i < A.Rank; i++)
			{
				result[i] = addDetA[i] / detA;
			}

			return result;
		}
	}
}
