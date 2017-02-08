using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LpSolve.Elements;
using LpSolve.Interface;
using LpSolve.Result;

namespace LpSolve
{
	public class SeidelSolver
	{
		private List<HalfSpace> _halfSpaces;
		private Vector _vector;

		private Polyhedron _resultPolyhedron;
		private FakeRandom _random;
		private SeidelResult _resultType;

		public SeidelResult Result { get { return this._resultType; } }

		public SeidelSolver(List<HalfSpace> halfSpaces, Vector vector)
		{
			var vectorSize = vector.GetDimension();

			this._halfSpaces = halfSpaces;
			this._vector = vector;

			this._random = new FakeRandom(DateTime.Now.Millisecond);
			this._resultPolyhedron = new Polyhedron();
			this._resultType = new UnboundedSeidelResult();
		}

        public SeidelSolver(Polyhedron polyhedron, Vector vector, SeidelResult resultType)
			: this(new List<HalfSpace>(), vector)
		{
			this._resultPolyhedron = polyhedron;
            this._resultType = resultType;
		}

		public void Run()
		{
			if (!this._halfSpaces.Any())
			{
				this._resultType = this._resultType.Resolve(this._resultPolyhedron, this._vector, false);
				return;
			}

			while (this._halfSpaces.Any() && !(this._resultType is InfeasibleSeidelResult))
			{
				var space = this._halfSpaces[_random.Next(this._halfSpaces.Count)];

				this._halfSpaces.Remove(space);
				Iterate(space);
			}
		}

		private void Iterate(HalfSpace halfSpace)
		{
			this._resultPolyhedron.Add(halfSpace);

			if (!(this._resultType is MinimumSeidelResult))
			{
				this._resultPolyhedron.RecountVertices();
			}

			if ((this._resultPolyhedron.HalfSpaces.Count == 1) && !(this._resultType is InfeasibleSeidelResult))
			{
				this._resultType = new UnboundedSeidelResult();
				return;
			}

			this._resultType = this._resultType.Resolve(this._resultPolyhedron, this._vector, this._halfSpaces.Any());
		}

		private class FakeRandom
		{
			public FakeRandom(int seed)
			{

			}

			public int Next(int count)
			{
				return 0;
			}
		}
	}
}
