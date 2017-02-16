#pragma once

#include <string>
#include <vector>
#include <cmath>
#include <stdexcept>
#include "../exceptionhelper.h"
#include "Vector.h"
#include "Plane.h"
#include "Line.h"
#include "../Interface/IElement.h"

namespace LpSolveCpp { namespace Elements { class Vector; } }
namespace LpSolveCpp { namespace Elements { class Plane; } }

using namespace LpSolveCpp::Interface;

namespace LpSolveCpp
{
	namespace Elements
	{
		class Point : public IElement<Point*>
		{
		private:
			Point *_parentPoint;

		public:
			virtual ~Point()
			{
				delete _parentPoint;
			}

			double getX();
			double getY();
			double getZ();

			Point *getParentPoint();
			void setParentPoint(Point *value);

		private:
			std::vector<double> _coordinates;

		public:
			Point(std::vector<double> &coordinates);

			Point(std::vector<double> &coordinates, Point *parentPoint);

			double GetAt(int index);

	#if defined(DEBUG)
			virtual std::wstring ToString() override;
	#endif

			virtual bool Equals(Point *obj);

			Point *MoveDown(Plane *plane, Vector *vector);

			Point *MoveDown(Plane *plane);

			Point *AddVector(Vector *vector);

			int GetDimension();
		};
	}
}
