#pragma once

#include <vector>
#include <cmath>
#include <stdexcept>
#include "Point.h"
#include "Plane.h"
#include "../Interface/IElement.h"

namespace LpSolveCpp { namespace Elements { class Point; } }
namespace LpSolveCpp { namespace Elements { class Plane; } }

using namespace LpSolveCpp::Interface;

namespace LpSolveCpp
{
	namespace Elements
	{
		class Vector : public IElement<Vector>
		{
		public:
			double getX();
			double getY();
			double getZ();

		private:
			std::vector<double> _coordinates;
			double  _length;

		public:
			double getLength();

			Vector(std::vector<double> &coordinates);

			double GetAt(int index);

			int GetDimension();

			static Vector *CreateFromPoints(Point *point0, Point *point1);

			void AddVector(Vector *vector);

			void SubtractVector(Vector *vector);

			double ScalarProduct(Vector *vector);

			Vector *CrossProduct(Vector *vector);

			void Flip();

			Vector *MoveDown(Plane *plane);

			Vector *Rotate90();
		};
	}
}
