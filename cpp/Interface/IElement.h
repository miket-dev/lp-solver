#pragma once

#include <type_traits>

namespace LpSolveCpp
{
	namespace Interface
	{
		template<typename TResult>
		class IElement
		{
		public:
			virtual TResult* MoveDown(void *plane)
			{
				return nullptr;
			};
			virtual int GetDimension() = 0;
		};
	}
}
