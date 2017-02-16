#pragma once

#include "SeidelResult.h"

namespace LpSolveCpp { namespace Result { class SeidelResult; } }

using namespace LpSolveCpp::Elements;

namespace LpSolveCpp
{
	namespace Result
	{
		class InfeasibleSeidelResult : public SeidelResult
		{
		public:
			virtual SeidelResult *Resolve(HalfSpace *halfSpace) override;
		};
	}
}
