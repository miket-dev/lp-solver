using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Interface;

namespace LpSolve.Elements
{
	public class Point : IElement<Point>
	{
		public double X { get { return this.GetAt(0); } }
		public double Y { get { return this.GetAt(1); } }
		public double Z { get { return this.GetAt(2); } }

		private double[] _coordinates;

		public Point(double[] coordinates)
		{
			this._coordinates = coordinates;
		}

		public double GetAt(int index)
		{
			if (this._coordinates.Length < index)
			{
				throw new ArgumentException("Point does not contain data for dimension " + (index + 1));
			}

			return this._coordinates[index];
		}

		public int GetDimension()
		{
			return this._coordinates.Length;
		}

		public Point MoveDown()
		{
			var coordinates = new double[this._coordinates.Length];
			for (int i = 0; i < coordinates.Length; i++)
			{
				coordinates[i] = this._coordinates[i];
			}

			return new Point(coordinates);
		}

		public Point MoveUp()
		{
			var coordinates = new double[this._coordinates.Length + 1];
			for (int i = 0; i < this._coordinates.Length; i++)
			{
				coordinates[i] = this._coordinates[i];
			}

			coordinates[this._coordinates.Length] = 0;

			return new Point(coordinates);
		}
	}
}
