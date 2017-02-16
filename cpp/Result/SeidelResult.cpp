#include "SeidelResult.h"

using namespace LpSolveCpp::Elements;
namespace LpSolveCpp
{
	namespace Result
	{

		Point *SeidelResult::getPoint() const
		{
			return privatePoint;
		}

		void SeidelResult::setPoint(Point *value)
		{
			privatePoint = value;
		}
	}
}
