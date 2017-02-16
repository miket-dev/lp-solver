#pragma once

#include <vector>
#include <stdexcept>


namespace MathExt
{
	class Matrix
	{
	private:
		std::vector<std::vector<double>> _arr;

	public:
		int getRank() const;

	private:
		Matrix(std::vector<std::vector<double>> &array_Renamed);

	public:
		double Determinant();

	private:
		double Determinant(Matrix *matrix);

		Matrix *Except(int rowNumber, int columnNumber);

	public:
		Matrix *Replace(int columnNumber, Matrix *matrix);

	private:
		template<typename T>
		std::vector<T> RemoveElement(std::vector<T> &item, int index);
	};
}
