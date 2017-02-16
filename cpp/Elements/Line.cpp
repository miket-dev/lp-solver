#include "Line.h"
#include "Point.h"
#include "Vector.h"
#include "Plane.h"

using namespace LpSolveCpp::Interface;
using namespace MathExt;
namespace LpSolveCpp
{
	namespace Elements
	{

		LpSolveCpp::Elements::Point *Line::getPoint() const
		{
			return this->_point;
		}

		LpSolveCpp::Elements::Vector *Line::getVector() const
		{
			return this->_vector;
		}

		double Line::getC() const
		{
			return this->_vector->getX() * this->_point->getX() + this->_vector->getY() * this->_point->getY();
		}

		Line::Line(LpSolveCpp::Elements::Point *point, LpSolveCpp::Elements::Vector *vector)
		{
			if (point->GetDimension() != vector->GetDimension())
			{
				throw std::invalid_argument("Point and Vector are of different dimensions!");
			}

			this->_point = point;
			this->_vector = vector;
		}

		Line *Line::MoveDown(Plane *plane)
		{
			auto point = this->_point->MoveDown(plane, this->_vector);
			auto vec = this->_vector->MoveDown(plane);
			return new Line(point, vec);
		}

		int Line::GetDimension()
		{
			return this->_point->GetDimension();
		}

		LpSolveCpp::Elements::Point *Line::IntersectPlane(Plane *plane)
		{
			auto fakeLinePoint = this->_point;
			auto fakeLineVector = this->_vector;

			if (plane->GetDimension() == 2)
			{
				//it is the line
				//|ax + by - c = 0
				//|a1x + b1y - c1 = 0

				std::vector<std::vector<double>> arrA;
				std::vector<double> arrA1;
				arrA1.push_back(this->_vector->getX());
				arrA1.push_back(this->_vector->getY());

				std::vector<double> arrA2;
				arrA2.push_back(plane->getVector()->getX());
				arrA2.push_back(plane->getVector()->getY());

				arrA.push_back(arrA1);
				arrA.push_back(arrA2);


				auto matrixA = Matrix::Create(arrA);

				auto matrixB = Matrix::Create(std::vector<std::vector<double>>
				{
					{this->getC()},
					{plane->getD()}
				});

				auto solver = new LESolver();

				auto result = solver->Solve(matrixA, matrixB);

				return new Point(result, this->_point->getParentPoint());
			}

			if (plane->GetDimension() == 3)
			{

			}

			throw NotImplementedException("Implemented only for 2d");
		}
	}
}
