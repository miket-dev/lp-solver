using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Interface;

namespace LpSolve.Elements
{
	public class Polyhedron : IElement<Polyhedron>
	{
		//actual polyhedron with vertices as points
		private List<Point> _vertices;
		//set of planes intersection lines
		private List<Line> _lines;
		private List<HalfSpace> _halfSpaces;

		public List<Point> Vertices
		{
			get { return this._vertices; }
		}

		public List<HalfSpace> HalfSpaces
		{
			get { return this._halfSpaces; }
		}

		public Polyhedron()
		{
			this._vertices = new List<Point>();
			this._lines = new List<Line>();
			this._halfSpaces = new List<HalfSpace>();
		}

		private Polyhedron(List<HalfSpace> halfspaces, List<Point> points, List<Line> lines)
		{
			this._vertices = points;
			this._halfSpaces = halfspaces;
			this._lines = lines;
		}

		public void Add(HalfSpace halfSpace)
		{
			this._halfSpaces.Add(halfSpace);
		}

		/// <summary>
		/// Manual trigger to recount polyhedron
		/// </summary>
		public void RecountVertices()
		{
			//the single halfspace does not create any vertices
			if (this._halfSpaces.Count == 0)
			{
				return;
			}

			//if we have more than one halfspace
			//assume that we already counted vertices for previous halfspaces
			//and searching intersections of current halfspace with others
			var lastHalfSpace = this._halfSpaces[this._halfSpaces.Count - 1];
			this._halfSpaces.Remove(lastHalfSpace);

			foreach (var item in this._halfSpaces)
			{
				var intersection = item.Intersect(lastHalfSpace);
				if (intersection != null) //if it is null just going to the next
				{
					if (intersection is Line)
					{
						//if we are in 3d we should find vertex of this line
						this.RecountVertices((Line)intersection);
					}
					else if (intersection is Point)
					{
						//we are in 2d, add it to result set
						this._vertices.Add((Point)intersection);
					}
				}
			}

			this._halfSpaces.Add(lastHalfSpace);

			//checking each vertex for matching with all halfspaces
			var excluded = new List<Point>();
			foreach (var item in this._halfSpaces)
			{
				foreach (var p in this._vertices)
				{
					if (!item.Contains(p))
					{
						excluded.Add(p);
					}
				}
			}

			foreach (var item in excluded)
			{
				this._vertices.Remove(item);
			}
		}

		private void RecountVertices(Line line)
		{
			if (this._lines.Count == 0)
			{
				this._lines.Add(line);
				return;
			}

			var excluded = new List<Line>();

			foreach (var item in this._lines)
			{
				var intersection = item.Intersect(line);
				if (intersection == null)
				{
					//there is no intersection with this line
					//will be removed later
					excluded.Add(item);
				}
				else
				{
					//simply adding it to result set
					this._vertices.Add(intersection);
				}
			}

			this._lines.Add(line);

			//removing excluded lines
			foreach (var item in excluded)
			{
				this._lines.Remove(item);
			}
		}

		public Polyhedron MoveDown()
		{
			var resultHalfspaces = new List<HalfSpace>();
			var resultLines = new List<Line>();
			var resultPoints = new List<Point>();

			foreach (var item in this._halfSpaces)
			{
				resultHalfspaces.Add(item.MoveDown());
			}

			foreach (var item in this._lines)
			{
				resultLines.Add(item.MoveDown());
			}

			foreach (var item in this._vertices)
			{
				resultPoints.Add(item.MoveDown());
			}

			return new Polyhedron(resultHalfspaces, resultPoints, resultLines);
		}

		public int GetDimension()
		{
			return this._halfSpaces.First().GetDimension();
		}
	}
}
