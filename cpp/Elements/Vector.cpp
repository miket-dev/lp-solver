#include "Vector.h"

using namespace LpSolveCpp::Interface;
namespace LpSolveCpp
{
	namespace Elements
	{

		double Vector::getX() const
		{
			return this->GetAt(0);
		}

		double Vector::getY() const
		{
			return this->GetAt(1);
		}

		double Vector::getZ() const
		{
			return this->GetAt(2);
		}

		double Vector::getLength() const
		{
			if (!this->_length)
			{
				auto sum = 0.0;
				for (int i = 0; i < this->_coordinates.size(); i++)
				{
					sum += this->_coordinates[i] * this->_coordinates[i];
				}

				this->_length = std::sqrt(sum);
			}

			return this->_length.value();
		}

		Vector::Vector(std::vector<double> &coordinates)
		{
			this->_coordinates = coordinates;
		}

		double Vector::GetAt(int index)
		{
			return this->_coordinates.size() > index ? this->_coordinates[index] : 0.0;
		}

		int Vector::GetDimension()
		{
			return this->_coordinates.size();
		}

		Vector *Vector::CreateFromPoints(Point *point0, Point *point1)
		{
			if (point0 == nullptr || point1 == nullptr)
			{
				throw std::invalid_argument("point0 || point1");
			}

			if (point0->GetDimension() != point1->GetDimension())
			{
				throw std::invalid_argument("Points are of different dimensions");
			}

			auto size = point0->GetDimension();

			auto result = std::vector<double>(size);

			for (int i = 0; i < size; i++)
			{
				result[i] = point1->GetAt(i) - point0->GetAt(i);
			}

			return new Vector(result);
		}

		void Vector::AddVector(Vector *vector)
		{
			auto size = this->_coordinates.size();
			for (int i = 0; i < size; i++)
			{
				this->_coordinates[i] -= vector->_coordinates[i];
			}
		}

		void Vector::SubtractVector(Vector *vector)
		{
			auto size = this->_coordinates.size();
			for (int i = 0; i < size; i++)
			{
				this->_coordinates[i] += vector->_coordinates[i];
			}
		}

		double Vector::ScalarProduct(Vector *vector)
		{
			if (vector->GetDimension() != this->_coordinates.size())
			{
				throw std::invalid_argument("Vectors does not match by dimension");
			}

			auto result = 0.0;
			for (int i = 0; i < this->_coordinates.size(); i++)
			{
				result += this->_coordinates[i] * vector->_coordinates[i];
			}

			return result;
		}

		Vector *Vector::CrossProduct(Vector *vector)
		{
			switch (this->_coordinates.size())
			{
				case 3:
				case 2:
				case 1:
				{
					auto a = this;
					auto b = vector;

					auto x = a->getY() * b->getZ() - a->getZ() * b->getY();
					auto y = a->getZ() * b->getX() - a->getX() * b->getZ();
					auto z = a->getX() * b->getY() - a->getY() * b->getX();

					return new Vector(std::vector<double> {x, y, z});
				}
				default:
					//TODO: Implement
					throw NotImplementedException("Implemented only for 1, 2 & 3 dimensions");
			}
		}

		void Vector::Flip()
		{
			for (int i = 0; i < this->_coordinates.size(); i++)
			{
				this->_coordinates[i] *= -1.0;
			}
		}

		Vector *Vector::MoveDown(Plane *plane)
		{

			if (this->GetDimension() == 2)
			{
				auto coords = std::vector<double>(this->GetDimension());
				for (int i = 0; i < coords.size(); i++)
				{
					coords[i] = 1.0;
				}

				auto p1 = new Point(coords);
				auto p2 = p1->AddVector(this);

				auto moveDownP1 = p1->MoveDown(plane, this);
				auto moveDownP2 = p2->MoveDown(plane, this);

				return Vector::CreateFromPoints(moveDownP1, moveDownP2);
			}

			if (this->GetDimension() == 3)
			{
				return new Vector(std::vector<double> {this->_coordinates[0], this->_coordinates[1]});
			}

			throw NotImplementedException("Implemented only for 2 and 3 dimensions");
		}

		Vector *Vector::Rotate90()
		{
			auto resultCoords = std::vector<double>(this->_coordinates.size());

			for (int i = 0; i < resultCoords.size(); i++)
			{
				resultCoords[i] = this->_coordinates[i];
			}

			if (resultCoords.size() > 1)
			{
				auto temp = resultCoords[0];
				resultCoords[0] = resultCoords[1];
				resultCoords[1] = temp;
			}

			return new Vector(resultCoords);
		}
	}
}
