#pragma once

#include <string>
#include <vector>
#include <cmath>
#include <stdexcept>
#include "Point.h"
#include "Vector.h"
#include "../Interface/IElement.h"
#include "../Matrix.h"

namespace LpSolveCpp { namespace Elements { class Point; } }
namespace LpSolveCpp { namespace Elements { class Vector; } }

using namespace LpSolveCpp::Elements;
using namespace LpSolveCpp::Interface;
using namespace MathExt;

namespace LpSolveCpp
{
	namespace Elements
	{
		class Plane : public IElement<Plane>
		{
		private:
			double _d = 0.0;
			LpSolveCpp::Elements::Point *_point;
			LpSolveCpp::Elements::Vector *_vector;

		public:
			virtual ~Plane()
			{
				delete _point;
				delete _vector;
			}

			LpSolveCpp::Elements::Point *getPoint();
			LpSolveCpp::Elements::Vector *getVector();

			Plane(LpSolveCpp::Elements::Point *point, LpSolveCpp::Elements::Vector *vector);

			double getD();

			Plane *MoveDown(Plane *plane);

			int GetDimension();
		};
	}
}
