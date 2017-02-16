#include "SeidelSolver.h"

using namespace LpSolveCpp::Elements;
#include "SeidelSolver.h"
#include <algorithm>

using namespace LpSolveCpp::Interface;
using namespace LpSolveCpp::Result;

namespace LpSolveCpp
{
	template<int M, template<typename> class F = std::less>
	struct TupleCompare
	{
		template<typename T>
		bool operator()(T const &t1, T const &t2)
		{
			return F<typename tuple_element<M, T>::type>()(std::get<M>(t1), std::get<M>(t2));
		}
	};


	SeidelResult *SeidelSolver::getResult() const
	{
		return this->_result;
	}

	SeidelSolver::SeidelSolver(std::vector<HalfSpace*> &halfSpaces, Vector *vector)
	{
		auto vectorSize = vector->GetDimension();

		this->_halfSpaces = halfSpaces;
		this->_vector = vector;

		this->_workHalfSpaces = std::vector<HalfSpace*>();
		this->_result = new UnboundedSeidelResult();
	}

	void SeidelSolver::Run()
	{
		if (this->_halfSpaces.size() > 0 && this->_halfSpaces[0]->GetDimension() == 1)
		{
			this->Resolve1D();
		}
		else if (this->_halfSpaces.size() > 0)
		{
			while (this->_halfSpaces.size() > 0 && (!(dynamic_cast<InfeasibleSeidelResult*>(this->_result) != nullptr) || this->_result == nullptr))
			{
				auto nextItem = rand() % this->_halfSpaces.size();

	#if defined(DEBUG)
				std::wcout << nextItem << std::endl;
	#endif

				auto space = this->_halfSpaces[nextItem];

				Iterate(space);
			}
		}
	}

	void SeidelSolver::Resolve1D()
	{
		auto set = std::vector<std::tuple<double, bool>>();

		for (auto item : this->_halfSpaces)
		{
			set.push_back(std::make_tuple(item->getPlane()->getPoint()->getX(), item->getPlane()->getVector()->getX() > 0));
		}

		std::sort(set.begin(), set.end(), TupleCompare<0>());

		auto containsTrue = false;

		for each (auto item in set)
		{
			containsTrue |= std::get<1>(item);
		}

		if (!std::get<1>(set.at(0)) && containsTrue)
		{
			this->_result = new InfeasibleSeidelResult();
			return;
		}
		//or if begins with true, when meets false and after it true again
		else if (std::get<1>(set.at(0)))
		{
			auto start = true;

			for (auto item : set)
			{
				if (!start && std::get<1>(item))
				{
					this->_result = new InfeasibleSeidelResult();
					return;
				}

				start = std::get<1>(item);
			}
		}

		if ((std::get<1>(set.at(set.size() - 1)) && this->_vector->getX() < 0) || 
			(!std::get<1>(set.at(0)) && this->_vector->getX() > 0))
		{
			this->_result = new UnboundedSeidelResult();

			return;
		}

		if (std::get<1>(set.at(set.size() - 1)) && this->_vector->getX() > 0)
		{
			auto minimumPoint = std::get<0>(set.at(set.size() - 1));

			set.pop_back();

			for (auto item : set)
			{
				if (std::get<0>(item) == minimumPoint)
				{
					Point *parentPoint = nullptr;
					for (auto space : this->_halfSpaces)
					{
						if (space->getPlane()->getPoint()->getX() == minimumPoint)
						{
							parentPoint = space->getPlane()->getPoint()->getParentPoint();
							break;
						}
					}

					auto arr = { new Point({ minimumPoint }, parentPoint) };

					this->_result = new AmbigousSeidelResult(new std::vector<Point*>(arr, arr + sizeof(arr)/sizeof(arr[0])));

					return;
				}
			}

			{
				Point *parentPoint = nullptr;
				for (auto item : this->_halfSpaces)
				{
					if (item->getPlane()->getPoint()->getX() == minimumPoint)
					{
						parentPoint = item->getPlane()->getPoint()->getParentPoint();
						break;
					}
				}

				Point tempVar(new double[] {minimumPoint}, parentPoint);
				this->_result = new MinimumSeidelResult(&tempVar);
				return;
			}
		}

		if (!orderedSet[0]->Item2 && this->_vector->getX() < 0)
		{
			auto minimumPoint = orderedSet[0]->Item1;

			orderedSet.erase(orderedSet.begin());

			for (auto item : orderedSet)
			{
				if (item->Item1 == minimumPoint)
				{
					this->_result = new AmbigousSeidelResult(std::vector<Point*> {new Point(std::vector<double> {minimumPoint})});

					return;
				}
			}

			Point *parentPoint = nullptr;
			for (auto item : this->_halfSpaces)
			{
				if (item->getPlane()->getPoint()->getX() == minimumPoint)
				{
					parentPoint = item->getPlane()->getPoint()->getParentPoint();
					break;
				}
			}

			Point tempVar2(new double[] {minimumPoint}, parentPoint);
			this->_result = new MinimumSeidelResult(&tempVar2);
			return;
		}

		if (orderedSet[0]->Item2 && !orderedSet[orderedSet.size() - 1]->Item2)
		{
			auto start = true;

			for (int i = 0; i < orderedSet.size(); i++)
			{
				auto item = orderedSet[i];
				if (start && !item->Item2)
				{
					Point *parentPoint = nullptr;
					auto minimumPoint = orderedSet[i - 1]->Item1;
					for (auto space : this->_halfSpaces)
					{
						if (space->getPlane()->getPoint()->getX() == minimumPoint)
						{
							parentPoint = space->getPlane()->getPoint()->getParentPoint();
							break;
						}
					}

					Point tempVar3(new double[] {minimumPoint}, parentPoint);
					this->_result = new MinimumSeidelResult(&tempVar3);
					return;
				}

				start = item->Item2;
			}
		}

		this->_result = new UnboundedSeidelResult();
	}

	void SeidelSolver::Iterate(HalfSpace *halfSpace)
	{
		this->_halfSpaces.Remove(halfSpace);

		if (dynamic_cast<MinimumSeidelResult*>(this->_result) != nullptr)
		{
			//simply check if current minimum satisfies the new constaint

			if (!halfSpace->Contains(this->_result->Point))
			{
				this->_result = new UnboundedSeidelResult();
			}
		}

		if (dynamic_cast<AmbigousSeidelResult*>(this->_result) != nullptr)
		{
			//check if there is only one satisfied point

			Point *minimumPoint = nullptr;
			auto minimumCount = 0;

			for (auto item : (static_cast<AmbigousSeidelResult*>(this->_result))->getAmbigousPoints())
			{
				if (halfSpace->Contains(item))
				{
					minimumCount++;
					minimumPoint = item;
				}
			}

			if (minimumCount == 0)
			{
				this->_result = new UnboundedSeidelResult();
			}
			else if (minimumCount == 1)
			{
				this->_result = new MinimumSeidelResult(minimumPoint);
			}
		}

		if (dynamic_cast<UnboundedSeidelResult*>(this->_result) != nullptr)
		{
			this->_result = this->FindMinimum(this->_workHalfSpaces, halfSpace);

			//if all items are in
			//then searching the minimum point in the halfspace,
			//which contains plane with same direction normal to target function
			if (!this->_halfSpaces.Any())
			{
				this->_workHalfSpaces.push_back(halfSpace);
				HalfSpace *workSpace = nullptr;
				for (auto item : this->_workHalfSpaces)
				{
					auto scalarProduct = this->_vector->ScalarProduct(item->getPlane()->getVector());
					if (scalarProduct >= 0)
					{
						workSpace = item;
						break;
					}
				}

				if (workSpace != nullptr)
				{
					this->_workHalfSpaces.Remove(workSpace);

					this->_result = this->FindMinimum(this->_workHalfSpaces, workSpace);
				}

				this->_workHalfSpaces.Remove(halfSpace);
			}
		}

		this->_workHalfSpaces.push_back(halfSpace);
	}

	SeidelResult *SeidelSolver::FindMinimum(std::vector<HalfSpace*> &workSpaces, HalfSpace *halfSpace)
	{
		auto passItems = std::vector<HalfSpace*>();
		for (auto item : workSpaces)
		{
			auto passItem = item->MoveDown(halfSpace->getPlane());

			if (passItem == nullptr)
			{
				if (!halfSpace->Contains(item->getPlane()->getPoint()) || !item->Contains(halfSpace->getPlane()->getPoint()))
				{
					return new InfeasibleSeidelResult();
				}
			}
			else
			{
				passItems.push_back(passItem);
			}
		}

		//if vectors are not of same direction - the minimum point can not be aligned on it
		if (halfSpace->getPlane()->getVector()->ScalarProduct(this->_vector) >= 0)
		{
			auto innerSolver = new SeidelSolver(passItems, this->_vector->MoveDown(halfSpace->getPlane()));
			innerSolver->Run();

			auto tempResult = innerSolver->getResult();

			if (dynamic_cast<MinimumSeidelResult*>(tempResult) != nullptr)
			{
				return new MinimumSeidelResult(tempResult->Point.ParentPoint);
			}
			else if (dynamic_cast<AmbigousSeidelResult*>(tempResult) != nullptr)
			{
				return new AmbigousSeidelResult((static_cast<AmbigousSeidelResult*>(tempResult))->getAmbigousPoints().Select([&] (void *x)
				{
					x::ParentPoint;
				})->ToArray());
			}

			return tempResult;
		}

		return new UnboundedSeidelResult();
	}

	SeidelSolver::FakeRandom::FakeRandom(int seed)
	{

	}

	int SeidelSolver::FakeRandom::Next(int count)
	{
		return count - 1;
	}

std::vector<int> SeidelSolver::FakeRandomSequence::_vals = {2, 0, 0, 1, 1, 0, 0};
int SeidelSolver::FakeRandomSequence::_counter = 0;

	SeidelSolver::FakeRandomSequence::FakeRandomSequence(int seed)
	{

	}

	int SeidelSolver::FakeRandomSequence::Next(int count)
	{
		return _vals[_counter++];
	}
}
