#include "stdafx.h"
#include "CppUnitTest.h"
#include <assert.h>
#include "../cpp/Elements/HalfSpace.h"
#include "../cpp/Elements/Point.h"
#include "../cpp/Elements/Plane.h"
#include "../cpp/Elements/Vector.h"
#include "../cpp/SeidelSolver.h"
#include "../cpp/Result/MinimumSeidelResult.h"
#include <typeinfo>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

using namespace LpSolveCpp::Elements;

namespace LpSolveCppTest
{		
	TEST_CLASS(SeidelTest)
	{
	public:
		
		TEST_METHOD(Seidel_SimpleTest_Cpp)
		{
			//5x+3y>=30
			std::vector<double> halfSpace1PointCoords = { 6.0, 0.0 };
			std::vector<double> halfSpace1VectorCoords = { 5.0, 3.0 };

			auto halfSpace1 = new HalfSpace(
				new Plane(
					new Point(halfSpace1PointCoords),
					new Vector(halfSpace1VectorCoords)
				),
				true
			);

			//x-y<=3
			std::vector<double> halfSpace2PointCoords = { 3.0, 0.0 };
			std::vector<double> halfSpace2VectorCoords = { 1.0, -1.0 };
			auto halfSpace2 = new HalfSpace(
				new Plane(
					new Point(halfSpace2PointCoords),
					new Vector(halfSpace2VectorCoords)
				),
				false
			);

			//-3x+5y<=15
			std::vector<double> halfSpace3PointCoords = { 5.0, 6.0 };
			std::vector<double> halfSpace3VectorCoords = { -3.0, 5.0 };
			auto halfSpace3 = new HalfSpace(
				new Plane(
					new Point(halfSpace3PointCoords),
					new Vector(halfSpace3VectorCoords)
				),
				false
			);

			//x >= 0
			std::vector<double> halfSpace4PointCoords = { 0.0, 0.0 };
			std::vector<double> halfSpace4VectorCoords = { 1.0, 0.0 };
			auto halfSpace4 = new HalfSpace(
				new Plane(
					new Point(halfSpace4PointCoords),
					new Vector(halfSpace4VectorCoords)
				),
				true
			);

			//y >= 0
			std::vector<double> halfSpace5PointCoords = { 0.0, 0.0 };
			std::vector<double> halfSpace5VectorCoords = { 0.0, 1.0 };
			auto halfSpace5 = new HalfSpace(
				new Plane(
					new Point(halfSpace5PointCoords),
					new Vector(halfSpace5VectorCoords)
				),
				true
			);

			assert(halfSpace1->getPlane()->getD() == 30);
			assert(halfSpace2->getPlane()->getD() == -3);
			assert(halfSpace3->getPlane()->getD() == -15);

			//-x+2y -> min
			std::vector<HalfSpace*> halfSpaces = { halfSpace1, halfSpace2, halfSpace3, halfSpace4, halfSpace5 };
			std::vector<double> vectorCoords = { -1.0, 2.0 };
			auto solver = LpSolveCpp::SeidelSolver(halfSpaces, new Vector(vectorCoords));
			solver.Run();

			assert(typeid(*solver.getResult()) == typeid(MinimumSeidelResult));
			assert(solver.getResult()->getPoint()->getX() == 4.875);
			assert(solver.getResult()->getPoint()->getY() == 1.875);
		}

	};
}