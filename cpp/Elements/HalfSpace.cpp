#include "HalfSpace.h"
#include "Plane.h"
#include "Point.h"
#include "Vector.h"

using namespace LpSolveCpp::Interface;
namespace LpSolveCpp
{
	namespace Elements
	{

		LpSolveCpp::Elements::Plane *HalfSpace::getPlane() const
		{
			return this->_plane;
		}

		HalfSpace::HalfSpace(LpSolveCpp::Elements::Plane *plane, bool isOnPositive)
		{
			this->_plane = plane;

			if (!isOnPositive)
			{
				this->_plane->getVector()->Flip();
			}
		}

		bool HalfSpace::Contains(Point *p)
		{
			auto vect = Vector::CreateFromPoints(this->_plane->getPoint(), p);

			return vect->ScalarProduct(this->_plane->getVector()) >= 0;
		}

		HalfSpace *HalfSpace::MoveDown(LpSolveCpp::Elements::Plane *plane)
		{
			auto pl = this->_plane->MoveDown(plane);

			if (pl == nullptr)
			{
				return nullptr;
			}

			return new HalfSpace(pl, true);
		}

		int HalfSpace::GetDimension()
		{
			return this->_plane->GetDimension();
		}
	}
}
