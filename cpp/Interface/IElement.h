#pragma once

#include <type_traits>

using namespace LpSolveCpp::Elements;

namespace LpSolveCpp
{
	namespace Interface
	{
		template<typename TResult>
		class IElement
		{
			/// <summary>
			/// Projects element on passed n-dimensional plane
			/// </summary>
		public:
			virtual TResult MoveDown(Plane *plane) = 0;
			virtual int GetDimension() = 0;
		};
	}
}
