#include "LESolver.h"
#include "Matrix.h"


namespace MathExt
{

	std::vector<double> LESolver::Solve(Matrix *A, Matrix *B)
	{
		if (B->getRank() != 1)
		{
			throw std::invalid_argument("Matrix B should have Rank == 1");
		}

		auto detA = A->Determinant();

		auto addDetA = std::vector<double>(A->getRank());
		for (int i = 0; i < A->getRank(); i++)
		{
//C# TO C++ CONVERTER TODO TASK: There is no direct native C++ equivalent to this .NET String method:
			addDetA[i] = A->Replace(i, B)->Determinant();
		}

		auto result = std::vector<double>(A->getRank());
		for (int i = 0; i < A->getRank(); i++)
		{
			result[i] = addDetA[i] / detA;
		}

		return result;
	}
}
