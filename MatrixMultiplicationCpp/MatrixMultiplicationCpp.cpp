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