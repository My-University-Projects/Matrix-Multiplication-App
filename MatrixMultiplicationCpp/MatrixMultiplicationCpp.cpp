#include"pch.h"
#include<string>
#include<fstream>




void CppMultiplication(int* resultRow, int* row, int* column, int columns, int rows)
{
    int* startCol = column;
    int* startRow = row;
    for (int i = 0; i < columns; i++)
    {
        row = startRow;
        (*resultRow) = 0;
 
        for (int j = 0; j < rows; j++)
        {
            (*resultRow) += ((*row) * (*column));
            row++;
            column++;
        }
        resultRow++;
    }
}