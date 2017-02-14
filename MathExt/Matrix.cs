using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExt
{
	public class Matrix
	{
		private double[][] _arr;

		public int Rank
		{
			get
			{
				return this._arr.Length > this._arr[0].Length ? this._arr[0].Length : this._arr.Length;
			}
		}

		private Matrix(double[][] array)
		{
			this._arr = array;
		}

		public static Matrix Create(params double[][] array)
		{
			if (array == null)
				throw new ArgumentNullException("array");

			var avgLength = array.Average(x => x.Length);
			if (array.Any(x => x.Length != avgLength))
				throw new ArgumentException("Inner arrays should have an equal length");

			return new Matrix(array);
		}

		public double Determinant()
		{
			return this.Determinant(this);
		}

		private double Determinant(Matrix matrix)
		{
			if (matrix.Rank == 1)
				return matrix._arr[0][0];

			var result = 0.0;
			for (int j = 0; j < matrix.Rank; j++)
			{
				result += (j % 2 == 0 ? 1.0 : -1.0) * matrix._arr[0][j] * this.Determinant(matrix.Except(0, j));
			}

			return result;
		}

		private Matrix Except(int rowNumber, int columnNumber)
		{
			var array = new double[this._arr.Length][];
			Array.Copy(this._arr, array, this._arr.Length);

			for (int i = 0; i < this._arr.Length; i++)
			{
				var newArray = new double[this._arr[i].Length];
				Array.Copy(this._arr[i], newArray, this._arr[i].Length);

				array[i] = newArray;
			}

			var resultArray = array.Select(x => this.RemoveElement(x, columnNumber).ToArray());

			return Matrix.Create(this.RemoveElement(resultArray, rowNumber).ToArray());

		}

		internal Matrix Replace(int columnNumber, Matrix matrix)
		{
			var array = new double[this._arr.Length][];
			Array.Copy(this._arr, array, this._arr.Length);

			for (int i = 0; i < this._arr.Length; i++)
			{
				var newArray = new double[this._arr[i].Length];
				Array.Copy(this._arr[i], newArray, this._arr[i].Length);

				array[i] = newArray;
			}

			var result = array.ToList();

			for (int i = 0; i < result.Count; i++)
			{
				result[i][columnNumber] = matrix._arr[i][0];
			}

			return Matrix.Create(result.ToArray());
		}

		private IEnumerable<T> RemoveElement<T>(IEnumerable<T> item, int index)
		{
			var array = new T[item.Count()];
			Array.Copy(item.ToArray(), array, item.Count());
			var list = array.ToList();

			list.RemoveAt(index);
			return list;
		}
	}
}
