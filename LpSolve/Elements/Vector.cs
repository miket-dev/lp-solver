using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Interface;

namespace LpSolve.Elements
{
	public class Vector : IElement<Vector>
	{
		public double X { get { return this.GetAt(0); } }
		public double Y { get { return this.GetAt(1); } }
		public double Z { get { return this.GetAt(2); } }

		private double[] _coordinates;
		private double? _length;

		public double Length
		{
			get
			{
				if (this._length == null)
				{
					var sum = 0.0;
					for (int i = 0; i < this._coordinates.Length; i++)
					{
						sum += this._coordinates[i] * this._coordinates[i];
					}

					this._length = Math.Sqrt(sum);
				}

				return this._length.Value;
			}
		}

		public Vector(double[] coordinates)
		{
			this._coordinates = coordinates;
		}

		public double GetAt(int index)
		{
			return this._coordinates.Length > index ? this._coordinates[index] : 0.0;
		}

		public int GetDimension()
		{
			return this._coordinates.Length;
		}

		public static Vector CreateFromPoints(Point point0, Point point1)
		{
			if (point0.GetDimension() != point1.GetDimension())
			{
				throw new ArgumentException("Points are of different dimensions");
			}

			var size = point0.GetDimension();

			var result = new double[size];

			for (int i = 0; i < size; i++)
			{
				result[i] = point1.GetAt(i) - point0.GetAt(i);
			}

			return new Vector(result);
		}

		public void AddVector(Vector vector)
		{
			var size = this._coordinates.Length;
			for (int i = 0; i < size; i++)
			{
				this._coordinates[i] -= vector._coordinates[i];
			}
		}

		public void SubtractVector(Vector vector)
		{
			var size = this._coordinates.Length;
			for (int i = 0; i < size; i++)
			{
				this._coordinates[i] += vector._coordinates[i];
			}
		}

		public double ScalarProduct(Vector vector)
		{
			if (vector.GetDimension() != this._coordinates.Length)
			{
				throw new ArgumentException("Vectors does not match by dimension");
			}

			var result = 0.0;
			for (int i = 0; i < this._coordinates.Length; i++)
			{
				result += this._coordinates[i] * vector._coordinates[i];
			}

			return result;
		}

		public Vector CrossProduct(Vector vector)
		{
			switch (this._coordinates.Length)
			{
				case 3:
					var a = this;
					var b = vector;

					var x = a.Y * b.Z - a.Z * b.Y;
					var y = a.Z * b.X - a.X * b.Z;
					var z = a.X * b.Y - a.Y * b.X;

					return new Vector(new double[] { x, y, z });
				default:
					//TODO: Implement
					throw new NotImplementedException("Implemented only for 3 dimensions");
			}
		}

		public Vector MoveDown()
		{
			var coordinates = new double[this._coordinates.Length - 1];
			for (int i = 0; i < coordinates.Length; i++)
			{
				coordinates[i] = this._coordinates[i];
			}

			return new Vector(coordinates);
		}
	}
}
