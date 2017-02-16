#pragma once

#include "SeidelResult.h"

namespace LpSolveCpp { namespace Result { class SeidelResult; } }

using namespace LpSolveCpp::Elements;

namespace LpSolveCpp
{
	namespace Result
	{
		class MinimumSeidelResult : public SeidelResult
		{
		public:
			MinimumSeidelResult(Point *minimumPoint);

			virtual SeidelResult *Resolve(HalfSpace *halfSpace) override;
		};
	}
}
