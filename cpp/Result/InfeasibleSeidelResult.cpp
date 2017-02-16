#include "InfeasibleSeidelResult.h"

using namespace LpSolveCpp::Elements;
namespace LpSolveCpp
{
	namespace Result
	{

		SeidelResult *InfeasibleSeidelResult::Resolve(HalfSpace *halfSpace)
		{
			//if on the various step it is become infeasible
			//it will be infeasible till the end
			return this;
		}
	}
}
