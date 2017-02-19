#include "Plane.h"

using namespace LpSolveCpp::Elements;
using namespace LpSolveCpp::Interface;
using namespace MathExt;
namespace LpSolveCpp
{
	namespace Elements
	{

		LpSolveCpp::Elements::Point *Plane::getPoint()
		{
			return this->_point;
		}

		LpSolveCpp::Elements::Vector *Plane::getVector()
		{
			return this->_vector;
		}

		Plane::Plane(LpSolveCpp::Elements::Point *point, LpSolveCpp::Elements::Vector *vector)
		{
			if (point->GetDimension() != vector->GetDimension())
			{
				throw std::invalid_argument("Point and Vector are of different dimensions!");
			}

			this->_point = point;
			this->_vector = vector;
		}

		double Plane::getD()
		{
			if (this->_d == 0)
			{
				_d = 0;

				auto size = this->getPoint()->GetDimension();
				for (int i = 0; i < size; i++)
				{
					_d += this->getPoint()->GetAt(i) * this->getVector()->GetAt(i);
				}
			}

			return _d;
		}

		Plane *Plane::MoveDown(Plane *plane)
		{
			if (this->GetDimension() == 2)
			{

				auto crossProduct = this->_vector->CrossProduct(plane->_vector);

				if (crossProduct->getLength() == 0)
				{
					return nullptr;
				}

				auto point = this->_point->MoveDown(plane, this->_vector);
				auto vec = this->_vector->MoveDown(plane);

				return new Plane(point, vec);
			}

			if (this->GetDimension() == 3)
			{
				//plane1 Ax+By+Cz-D = 0
				//plane2 A1x+B1y+C1z-D1 = 0

				auto solver = new LESolver();

				//set x == 0
				auto x = 0.0;

				auto vals1 = std::vector<std::vector<double>>
				{
					{this->_vector->getY(), this->_vector->getZ()},
					{ plane->getVector()->getY(), plane->getVector()->getZ() }
				};

				auto matrixA = new Matrix(vals1);

				auto vals2 = std::vector<std::vector<double>>
				{
					{ this->getD() },
					{ plane->getD() }
				};

				auto matrixB = new Matrix(vals2);

				auto result = solver->Solve(matrixA, matrixB);

				auto y = result[0];
				auto z = result[1];

				//if smth of this is infinity, set y to 0
				if (std::isinf(y) || std::isinf(z) || std::isnan(y) || std::isnan(z))
				{
					y = 0.0;

					auto valsA = std::vector<std::vector<double>>
					{
						{ this->_vector->getX(), this->_vector->getZ() },
						{ plane->getVector()->getX(), plane->getVector()->getZ() }
					};
					matrixA = new Matrix(valsA);

					auto valsB = std::vector<std::vector<double>>
					{
						{ this->getD() },
						{ plane->getD() }
					};

					matrixB = new Matrix(valsB);

					result = solver->Solve(matrixA, matrixB);

					x = result[0];
					z = result[1];
				}

				if (std::isinf(x) || std::isinf(z) || std::isnan(x) || std::isnan(z))
				{
					//set z to 0
					z = 0.0;

					auto valsA = std::vector<std::vector<double>>
					{
						{ this->_vector->getX(), this->_vector->getY() },
						{ plane->getVector()->getX(), plane->getVector()->getY() }
					};
					matrixA = new Matrix(valsA);

					auto valsB = std::vector<std::vector<double>>
					{
						{ this->getD() },
						{ plane->getD() }
					};

					matrixB = new Matrix(valsB);

					result = solver->Solve(matrixA, matrixB);

					x = result[0];
					y = result[1];
				}

				if (std::isinf(x) || std::isinf(y) || std::isinf(z) || std::isnan(x) || std::isnan(y) || std::isnan(z))
				{
					//there is no intersection
					return nullptr;
				}

				std::vector<double> pointCoords = { x, y };
				std::vector<double> parentPointCoords = { x, y, z };

				LpSolveCpp::Elements::Point tempVar(pointCoords, new Point(parentPointCoords));

				std::vector<double> vectorCoordinates = { this->getVector()->getX(), this->getVector()->getY() };
				LpSolveCpp::Elements::Vector tempVar2(vectorCoordinates);
				return new Plane(&tempVar, &tempVar2);
			}

			return nullptr;
		}

		int Plane::GetDimension()
		{
			return this->_point->GetDimension();
		}
	}
}
