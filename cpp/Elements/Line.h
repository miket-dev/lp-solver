#pragma once

#include <string>
#include <vector>
#include <stdexcept>
#include "../exceptionhelper.h"
#include "../Matrix.h"
#include "../LESolver.h"
#include "../Interface/IElement.h"

namespace LpSolveCpp { namespace Elements { class Point; } }
namespace LpSolveCpp { namespace Elements { class Vector; } }
namespace LpSolveCpp { namespace Elements { class Plane; } }

using namespace LpSolveCpp::Interface;
using namespace MathExt;

namespace LpSolveCpp
{
	namespace Elements
	{
		class Line : public IElement<Line>
		{
		private:
			LpSolveCpp::Elements::Point *_point;
			LpSolveCpp::Elements::Vector *_vector;

		public:
			virtual ~Line()
			{
				delete _point;
				delete _vector;
			}

			LpSolveCpp::Elements::Point *getPoint() const;
			LpSolveCpp::Elements::Vector *getVector() const;

			double getC() const;

			Line(LpSolveCpp::Elements::Point *point, LpSolveCpp::Elements::Vector *vector);

			Line *MoveDown(Plane *plane);

			int GetDimension();

			LpSolveCpp::Elements::Point *IntersectPlane(Plane *plane);
		};
	}
}
