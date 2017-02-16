#pragma once

#include "SeidelResult.h"
#include <vector>

namespace LpSolveCpp { namespace Result { class SeidelResult; } }

using namespace LpSolveCpp::Elements;

namespace LpSolveCpp
{
	namespace Result
	{
		class AmbigousSeidelResult : public SeidelResult
		{
		private:
			std::vector<Point*> privateAmbigousPoints;

			public:
				std::vector<Point*> getAmbigousPoints() const;
				void setAmbigousPoints(std::vector<Point*> &value);

			AmbigousSeidelResult(std::vector<Point*> &points);

			virtual SeidelResult *Resolve(HalfSpace *halfSpace) override;
		};
	}
}
