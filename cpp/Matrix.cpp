#include "Matrix.h"


namespace MathExt
{

	int Matrix::getRank() const
	{
		return this->_arr.size() > this->_arr[0].size() ? this->_arr[0].size() : this->_arr.size();
	}

	Matrix::Matrix(std::vector<std::vector<double>> &array_Renamed)
	{
		this->_arr = array_Renamed;
	}

	double Matrix::Determinant()
	{
		return this->Determinant(this);
	}

	double Matrix::Determinant(Matrix *matrix)
	{
		if (matrix->getRank() == 1)
		{
			return matrix->_arr[0][0];
		}

		auto result = 0.0;
		for (int j = 0; j < matrix->getRank(); j++)
		{
			result += (j % 2 == 0 ? 1.0 : -1.0) * matrix->_arr[0][j] * this->Determinant(matrix->Except(0, j));
		}

		return result;
	}

	Matrix *Matrix::Except(int rowNumber, int columnNumber)
	{
		auto array_Renamed = std::vector<std::vector<double>>(this->_arr.size());
		std::copy(std::begin(this->_arr), std::end(this->_arr), array_Renamed);

		for (int i = 0; i < this->_arr.size(); i++)
		{
			auto newArray = std::vector<double>();
			std::copy(std::begin(this->_arr[i]), std::end(this->_arr[i]), newArray);

			array_Renamed[i] = newArray;
		}

		auto resultArray = array_Renamed.Select([&] (void *x)
		{
			this->RemoveElement(x, columnNumber).ToArray();
		});

		return Matrix::Create(this->RemoveElement(resultArray, rowNumber).ToArray());

	}

	Matrix *Matrix::Replace(int columnNumber, Matrix *matrix)
	{
		auto array_Renamed = std::vector<std::vector<double>>(this->_arr.size());
		Array::Copy(this->_arr, array_Renamed, this->_arr.size());

		for (int i = 0; i < this->_arr.size(); i++)
		{
			auto newArray = std::vector<double>(this->_arr[i].Length);
			Array::Copy(this->_arr[i], newArray, this->_arr[i].Length);

			array_Renamed[i] = newArray;
		}

		auto result = array_Renamed.ToList();

		for (int i = 0; i < result.size(); i++)
		{
			result[i][columnNumber] = matrix->_arr[i][0];
		}

		return Matrix::Create(result.ToArray());
	}

template<typename T>
	std::vector<T> Matrix::RemoveElement(std::vector<T> &item, int index)
	{
		auto array_Renamed = std::vector<T>(item.size()());
		Array::Copy(item.ToArray(), array_Renamed, item.size()());
		auto list = array_Renamed.ToList();

		list.erase(list.begin() + index);
		return list;
	}
}
