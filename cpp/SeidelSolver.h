#pragma once

#include <vector>
#include <iostream>
#include <tuple>
#include "Result\SeidelResult.h"
#include "Result\InfeasibleSeidelResult.h"
#include "Result\UnboundedSeidelResult.h"
#include "Result\AmbigousSeidelResult.h"
#include "Result\MinimumSeidelResult.h"
#include "Interface\IElement.h"
#include "Elements\HalfSpace.h"
#include "Elements\Vector.h"

using namespace LpSolveCpp::Elements;
using namespace LpSolveCpp::Interface;
using namespace LpSolveCpp::Result;

namespace LpSolveCpp
{
	class SeidelSolver
	{
	private:
		std::vector<HalfSpace*> _halfSpaces;
		Vector *_vector;

		std::vector<HalfSpace*> _workHalfSpaces;
		SeidelResult *_result;

	public:
		virtual ~SeidelSolver()
		{
			delete _vector;
			delete _result;
		}

		SeidelResult *getResult() const;

		SeidelSolver(std::vector<HalfSpace*> &halfSpaces, Vector *vector);

		void Run();

	private:
		void Resolve1D();

		void Iterate(HalfSpace *halfSpace);

		SeidelResult *FindMinimum(std::vector<HalfSpace*> &workSpaces, HalfSpace *halfSpace);

	private:
		class FakeRandom
		{
		public:
			FakeRandom(int seed);

			int Next(int count);
		};

	private:
		class FakeRandomSequence
		{
		private:
			static std::vector<int> _vals;
			static int _counter;

		public:
			FakeRandomSequence(int seed);

			int Next(int count);
		};
	};
}
