#pragma once

namespace LpSolveCpp { namespace Elements { class Plane; } }
namespace LpSolveCpp { namespace Elements { class Point; } }

using namespace LpSolveCpp::Interface;

namespace LpSolveCpp
{
	namespace Elements
	{
		class HalfSpace : public IElement<HalfSpace*>
		{
		private:
			LpSolveCpp::Elements::Plane *_plane;

		public:
			virtual ~HalfSpace()
			{
				delete _plane;
			}

			LpSolveCpp::Elements::Plane *getPlane() const;

			HalfSpace(LpSolveCpp::Elements::Plane *plane, bool isOnPositive);

			bool Contains(Point *p);

			HalfSpace *MoveDown(LpSolveCpp::Elements::Plane *plane);

			int GetDimension();
		};
	}
}
