using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAtrixMultiplicationWindow
{
    public class Matrix
    {
        public int[,] matrix;
        private int matrixSize;


        public Matrix(int size)
        {
            this.matrixSize = size;
            this.matrix = new int[size, size];
        }

        public static Matrix LoadMatrix(string filePath)
        {
            try
            {
                StreamReader reader = new StreamReader(filePath);
                int size;
                if (int.TryParse(reader.ReadLine(), out size) == false)
                {
                    Matrix m = new Matrix(0);
                    return m;
                }
                Matrix matrix = new Matrix(size);
                matrix.matrixSize = size;
                int val;
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (int.TryParse(reader.ReadLine(), out val) == false)
                        {
                            Matrix m = new Matrix(0);
                            return m;
                        }
                        matrix.matrix[i, j] = val;
                    }
                }
                return matrix;
            }
            catch (FileNotFoundException)
            {
                Matrix m = new Matrix(0);
                return m;
            }
        }

        public static Matrix GenerateMatrix(int size)
        {
            Matrix matrix = new Matrix(size);
            System.Random rnd = new System.Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix.matrix[i, j] = j + 1;
                }
            }
            return matrix;
        }

        public int GetSize()
        {
            return this.matrixSize;
        }

        public void PrintMatrixtoFile()
        {
            using (StreamWriter writer = new StreamWriter("wynik.txt"))
            {
                for (int i = 0; i < matrixSize; i++)
                {
                    for (int j = 0; j < matrixSize; j++)
                    {
                        string x = matrix[i, j].ToString();
                        writer.Write(x + " ");
                    }
                    writer.WriteLine();
                }
            }
        }

        public void ResetMatrix()
        {
            Matrix m = new Matrix(0);
            this.matrix = m.matrix;
            this.matrixSize = m.matrixSize;
        }
    }
}