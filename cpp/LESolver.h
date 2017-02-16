#pragma once

#include <vector>
#include <stdexcept>

namespace MathExt { class Matrix; }


namespace MathExt
{
	class LESolver
	{
	public:
		std::vector<double> Solve(Matrix *A, Matrix *B);
	};
}
