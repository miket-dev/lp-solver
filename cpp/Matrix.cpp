#pragma warning(disable:4996)

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
		//if (matrix->getRank() == 1)
		//{
		//	return matrix->_arr[0][0];
		//}

		//auto result = 0.0;
		//for (int j = 0; j < matrix->getRank(); j++)
		//{
		//	result += (j % 2 == 0 ? 1.0 : -1.0) * matrix->_arr[0][j] * this->Determinant(matrix->Except(0, j));
		//}

		//return result;
		throw std::invalid_argument("");
	}

	Matrix *Matrix::Except(int rowNumber, int columnNumber)
	{
		//auto array_Renamed = std::vector<std::vector<double>>(this->_arr.size());
		//std::copy(std::begin(this->_arr), std::end(this->_arr), array_Renamed);

		//for (int i = 0; i < this->_arr.size(); i++)
		//{
		//	auto newArray = std::vector<double>();
		//	std::copy(std::begin(this->_arr[i]), std::end(this->_arr[i]), newArray);

		//	array_Renamed[i] = newArray;
		//}

		//std::vector<std::vector<double>> resultArray;

		//for (int i = 0; i < array_Renamed.size(); i++)
		//{
		//	auto arr = array_Renamed.at(i);
		//	resultArray.push_back(this->RemoveSingleElement(arr, columnNumber));
		//}

		//return new Matrix(this->RemoveElement(resultArray, rowNumber));
		throw std::invalid_argument("");
	}

	Matrix *Matrix::Replace(int columnNumber, Matrix *matrix)
	{
		//auto array_Renamed = std::vector<std::vector<double>>(this->_arr.size());
		//std::copy(std::begin(this->_arr), std::end(this->_arr), array_Renamed);

		//for (int i = 0; i < this->_arr.size(); i++)
		//{
		//	auto newArray = std::vector<double>(this->_arr[i].size());

		//	std::copy(std::begin(this->_arr[i]), std::end(this->_arr[i]), newArray);

		//	array_Renamed[i] = newArray;
		//}

		//auto result = array_Renamed;

		//for (int i = 0; i < result.size(); i++)
		//{
		//	result[i][columnNumber] = matrix->_arr[i][0];
		//}

		//return new Matrix(result);
		throw std::invalid_argument("");
	}

	std::vector<std::vector<double>> Matrix::RemoveElement(std::vector<std::vector<double>> &item, int index)
	{
		std::vector<std::vector<double>> array_Renamed(item);

		array_Renamed.erase(array_Renamed.begin() + index);

		return array_Renamed;
	}

	std::vector<double> Matrix::RemoveSingleElement(std::vector<double> &item, int index)
	{
		std::vector<double> array_Renamed(item);

		array_Renamed.erase(array_Renamed.begin() + index);

		return array_Renamed;
	}
}
