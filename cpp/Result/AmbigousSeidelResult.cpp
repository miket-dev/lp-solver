#include "AmbigousSeidelResult.h"

using namespace LpSolveCpp::Elements;
namespace LpSolveCpp
{
	namespace Result
	{

		std::vector<Point*> AmbigousSeidelResult::getAmbigousPoints() const
		{
			return privateAmbigousPoints;
		}

		void AmbigousSeidelResult::setAmbigousPoints(std::vector<Point*> &value)
		{
			privateAmbigousPoints = value;
		}

		AmbigousSeidelResult::AmbigousSeidelResult(std::vector<Point*> &points)
		{
			this->setAmbigousPoints(points);
		}

		SeidelResult *AmbigousSeidelResult::Resolve(HalfSpace *halfSpace)
		{
			throw NotImplementedException();
		}
	}
}
