#pragma once

#include <vector>
#include <stdexcept>


namespace MathExt
{
	class Matrix
	{
	private:
		std::vector<std::vector<double>> _arr;
		double Determinant(Matrix *matrix);
		Matrix *Except(int rowNumber, int columnNumber);
		std::vector<std::vector<double>> RemoveElement(std::vector<std::vector<double>> &item, int index);
		std::vector<double> RemoveSingleElement(std::vector<double> &item, int index);

	public:
		int getRank() const;
		Matrix(std::vector<std::vector<double>> &array_Renamed);
		double Determinant();
		Matrix *Replace(int columnNumber, Matrix *matrix);
	};
}
