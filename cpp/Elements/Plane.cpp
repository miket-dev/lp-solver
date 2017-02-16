#include "Plane.h"

using namespace LpSolveCpp::Elements;
using namespace LpSolveCpp::Interface;
using namespace MathExt;
namespace LpSolveCpp
{
	namespace Elements
	{

		LpSolveCpp::Elements::Point *Plane::getPoint() const
		{
			return this->_point;
		}

		LpSolveCpp::Elements::Vector *Plane::getVector() const
		{
			return this->_vector;
		}

		Plane::Plane(LpSolveCpp::Elements::Point *point, LpSolveCpp::Elements::Vector *vector)
		{
			if (point->GetDimension() != vector->GetDimension())
			{
				throw std::invalid_argument(std::wstring::Format(L"Point{{0}} and Vector{{1}} are of different dimensions!", point->GetDimension(), vector->GetDimension()));
			}

			this->_point = point;
			this->_vector = vector;
		}

		double Plane::getD() const
		{
			if (!this->_d)
			{
				this->_d = 0;

				auto size = this->getPoint()->GetDimension();
				for (int i = 0; i < size; i++)
				{
					this->_d += this->getPoint()->GetAt(i) * this->getVector()->GetAt(i);
				}
			}

			return this->_d.value();
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
				auto matrixA = Matrix::Create(std::vector<std::vector<double>>
				{
					{this->_vector->getY(), this->_vector->getZ()},
					{plane->getVector()->getY(), plane->getVector()->getZ()}
				});

				auto matrixB = Matrix::Create(std::vector<std::vector<double>>
				{
					{this->getD()},
					{plane->getD()}
				});

				auto result = solver->Solve(matrixA, matrixB);

				auto y = result[0];
				auto z = result[1];

				//if smth of this is infinity, set y to 0
				if (std::isinf(y) || std::isinf(z) || std::isnan(y) || std::isnan(z))
				{
					y = 0.0;
					matrixA = Matrix::Create(std::vector<std::vector<double>>
					{
						{this->_vector->getX(), this->_vector->getZ()},
						{plane->getVector()->getX(), plane->getVector()->getZ()}
					});

					matrixB = Matrix::Create(std::vector<std::vector<double>>
					{
						{this->getD()},
						{plane->getD()}
					});

					result = solver->Solve(matrixA, matrixB);

					x = result[0];
					z = result[1];
				}

				if (std::isinf(x) || std::isinf(z) || std::isnan(x) || std::isnan(z))
				{
					//set z to 0
					z = 0.0;
					matrixA = Matrix::Create(std::vector<std::vector<double>>
					{
						{this->_vector->getX(), this->_vector->getY()},
						{plane->getVector()->getX(), plane->getVector()->getY()}
					});

					matrixB = Matrix::Create(std::vector<std::vector<double>>
					{
						{this->getD()},
						{plane->getD()}
					});

					result = solver->Solve(matrixA, matrixB);

					x = result[0];
					y = result[1];
				}

				if (std::isinf(x) || std::isinf(y) || std::isinf(z) || std::isnan(x) || std::isnan(y) || std::isnan(z))
				{
					//there is no intersection
					return nullptr;
				}

				LpSolveCpp::Elements::Point tempVar(new double[] {x, y}, new Point(new double[] {x, y, z}));
				LpSolveCpp::Elements::Vector tempVar2(new double[] {this->getVector()->getX(), this->getVector()->getY()});
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
