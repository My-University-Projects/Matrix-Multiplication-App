using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using MAtrixMultiplication;

namespace MatrixMultiplication
{
    public partial class App : Application
    {
        [DllImport(@"C:\Users\ASUS\OneDrive\Pulpit\MAtrixMultiplication\x64\Debug\MatrixMultiplicationAsm.dll")]
        static public extern unsafe int  AsmMultiplication(int* resultRow, int* row, int* column, int size); //static public extern unsafe int  AsmMultiplication(int size, int* matrix1, int* matrix2);

        [DllImport(@"C:\Users\ASUS\OneDrive\Pulpit\MAtrixMultiplication\x64\Debug\MatrixMultiplicationCpp.dll")]
        public static extern unsafe int CppMultiplication(int size, int* matrix1, int* matrix2);

        [DllImport(@"C:\Users\ASUS\OneDrive\Pulpit\MAtrixMultiplication\x64\Debug\MatrixMultiplicationCpp.dll")]
        public static extern unsafe void multiply(int* resultRow, int* row, int* column, int size);
    }

}