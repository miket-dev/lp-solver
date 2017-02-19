#include "Point.h"

using namespace LpSolveCpp::Interface;
namespace LpSolveCpp
{
	namespace Elements
	{

		double Point::getX()
		{
			return this->GetAt(0);
		}

		double Point::getY()
		{
			return this->GetAt(1);
		}

		double Point::getZ()
		{
			return this->GetAt(2);
		}

		Point *Point::getParentPoint()
		{
			return this->_parentPoint;
		}

		void Point::setParentPoint(Point *value)
		{
			this->_parentPoint = value;
		}

		Point::Point(std::vector<double> &coordinates)
		{
			this->_coordinates = coordinates;
			this->_parentPoint = nullptr;
		}

		Point::Point(std::vector<double> &coordinates, Point *parentPoint)
		{
			this->_parentPoint = parentPoint;
			this->_coordinates = coordinates;
		}

		double Point::GetAt(int index)
		{
			if (this->_coordinates.size() < index)
			{
				throw std::invalid_argument("Point does not contain data for dimension " + std::to_string(index + 1));
			}

			return this->_coordinates[index];
		}

		bool Point::Equals(Point *obj)
		{
			auto result = true;

			for (int i = 0; i < this->GetDimension(); i++)
			{
				result &= this->GetAt(i) == obj->GetAt(i);
			}

			return result;
		}

		Point *Point::MoveDown(Plane *plane, Vector *vector)
		{
			//point itself does not belong to any line or plane
			//it requires to find an intersection of passed line or plane and projective plane
			auto line = new Line(this, vector);

			auto result = line->IntersectPlane(plane);

			if (result == nullptr || std::isinf(result->getX()) || std::isinf(result->getY()))
			{
				return nullptr;
			}

			if (plane->GetDimension() == 2)
			{
				//set axis beginning as plane point
				auto vect = Vector::CreateFromPoints(plane->getPoint(), result);

				auto scalarProduct = vect->ScalarProduct(vector);

				std::vector<double> coords;
				coords.push_back(scalarProduct > 0 ? vect->getLength() : -vect->getLength());

				return new Point(coords, result);
			}

			throw NotImplementedException("Not implemented for 3d");
		}

		Point *Point::MoveDown(Plane *plane)
		{
			throw ApplicationException("MoveDown(Plane, Vector) should be used");
		}

		Point *Point::AddVector(Vector *vector)
		{
			auto coords = std::vector<double>(this->GetDimension());

			for (int i = 0; i < coords.size(); i++)
			{
				coords[i] = this->GetAt(i) + vector->GetAt(i);
			}

			return new Point(coords);
		}

		int Point::GetDimension()
		{
			return this->_coordinates.size();
		}
	}
}
