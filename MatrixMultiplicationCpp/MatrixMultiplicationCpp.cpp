#include"pch.h"
#include<string>
#include<fstream>



int CppMultiplication(int size, int* matrix1Rows, int* matrix2Cols) {
	int result = 0;

	for (int i = 0; i < size; i++) {
		result += ((*matrix1Rows) * (*matrix2Cols));
		matrix1Rows++;
		matrix2Cols++;
	}
	return result;
}

void multiply(int* resultRow, int* row, int* column, int size)
{
    int* startCol = column;
    int* start = row;
    for (int i = 0; i < size; i++)
    {
        column = startCol;
        row = start;
        column += i;
        (*resultRow) = 0;
 
        for (int j = 0; j < size; j++)
        {
            (*resultRow) += ((*row) * (*column));
            row++;
            column += size;
        }
        resultRow++;
    }
}