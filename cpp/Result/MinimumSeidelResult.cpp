#include "MinimumSeidelResult.h"

using namespace LpSolveCpp::Elements;
namespace LpSolveCpp
{
	namespace Result
	{

		MinimumSeidelResult::MinimumSeidelResult(Point *minimumPoint)
		{
			this->setPoint(minimumPoint);
		}

		SeidelResult *MinimumSeidelResult::Resolve(HalfSpace *halfSpace)
		{
			throw NotImplementedException();
		}
	}
}
