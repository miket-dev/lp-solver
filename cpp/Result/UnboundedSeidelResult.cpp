#include "UnboundedSeidelResult.h"
#include "../Elements/HalfSpace.h"
#include "../exceptionhelper.h"

using namespace LpSolveCpp::Elements;

namespace LpSolveCpp
{
	namespace Result
	{

		SeidelResult *UnboundedSeidelResult::Resolve(HalfSpace *halfSpace)
		{
			throw NotImplementedException();
		}
	}
}
