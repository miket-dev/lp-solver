#pragma once
#include "../Elements/Point.h"
#include "../Elements/HalfSpace.h"
using namespace LpSolveCpp::Elements;

namespace LpSolveCpp
{
	namespace Result
	{
		/// <summary>
		/// Base class for LP result
		/// </summary>
		class SeidelResult
		{
		private:
			Point *privatePoint;

			public:
				Point *getPoint() const;
				void setPoint(Point *value);

			virtual SeidelResult *Resolve(HalfSpace *halfSpace) = 0;
		};
	}
}
