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
        int rows;
        int columns;


        public Matrix(int size)
        {
            this.matrixSize = size;
            this.matrix = new int[size, size];
        }
        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            this.matrix = new int[rows, columns];
        }

        public int GetRows()
        {
            return this.rows;
        }

        public void SetRows(int rows)
        {
            this.rows = rows;
        }

        public void SetColumns(int columns)
        {
            this.columns = columns;
        }

        public int GetColumns()
        {
            return this.columns;
        }


        public static Matrix LoadMatrix2(string filePath)
        {
            try
            {
                StreamReader reader = new StreamReader(filePath);
                int rows, columns;
                if (int.TryParse(reader.ReadLine(), out rows) == false) 
                {
                    Matrix m = new Matrix(0);
                    return m;
                }
                if (int.TryParse(reader.ReadLine(), out columns) == false)
                {
                    Matrix m = new Matrix(0);
                    return m;
                }
                Matrix matrix = new Matrix(rows, columns);
                int val;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
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

        public static Matrix LoadMatrix(string filePath)
        {
            try
            {
                StreamReader reader = new StreamReader(filePath);
                int rows, columns;
                columns = (reader.Read() - 48);
                if (columns == -1)
                {
                    Matrix m = new Matrix(0);
                    return m;
                }
                reader.Read();
                rows = (reader.Read() - 48);
                if (rows == -1)
                {
                    Matrix m = new Matrix(0);
                    return m;
                }
                reader.Read();
                reader.Read();

                Matrix matrix = new Matrix(rows, columns);
                string tmp;
                string number;
                string line;
                int lineIterator;
                int val;

                for (int i = 0; i < rows; i++)
                {
                    line = reader.ReadLine();
                    if(line == null)
                    {
                        return matrix;
                    }
                    lineIterator = 0;
                    for (int j = 0; j < columns; j++)
                    {
                        number = "";
                        while (lineIterator < line.Length)
                        {
                            if(line[lineIterator] == ' ')
                            {
                                lineIterator++;
                                break;
                            }
                            tmp = line[lineIterator].ToString();
                            number += tmp;
                            lineIterator++;
                        }
                        if(int.TryParse(number, out val) == true)
                        {
                            matrix.matrix[i, j] = val;
                        }
                        else
                        {
                            Matrix m = new Matrix(0);
                            return m;
                        }

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

        public static Matrix GenerateMatrix(int rows, int columns)
        {
            Matrix matrix = new Matrix(rows, columns);
            System.Random rnd = new System.Random();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
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
                writer.Write(rows + " " + columns);
                writer.WriteLine();
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns ; j++)
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

        public void Transponate()
        {
            Matrix matrix = new Matrix(this.columns, this.rows);
            for (int iw = 0; iw < matrix.columns; iw++)
            {
                for (int ik = 0; ik < matrix.rows; ik++) 
                {
                    matrix.matrix[ik, iw] = this.matrix[iw, ik];
                }
            }
            this.matrix = matrix.matrix;
            this.columns = matrix.columns;
            this.rows = matrix.rows;
        }

        public void AlignColumns()
        {
            int alignedColumns = 4 - (this.columns % 4);
            Matrix alignedMatrix = new Matrix(this.rows, (this.columns + alignedColumns));
            for(int i = 0; i < rows; i++)
            {
                for(int z = 0; z < alignedColumns; z++)
                {
                    alignedMatrix.matrix[i, (columns + z)] = 0;
                }
                for(int j = 0; j < columns; j++)
                {
                    alignedMatrix.matrix[i, j] = matrix[i, j];
                }
            }

            this.matrix = alignedMatrix.matrix;
            this.columns = alignedMatrix.columns;
        }

        public void AlignRows()
        {
            int alignedRows = 4 - (this.rows % 4);
            Matrix alignedMatrix = new Matrix((this.rows + alignedRows), this.columns);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    alignedMatrix.matrix[i, j] = matrix[i, j];
                }
            }

            for(int z = 0; z < alignedRows; z++)
            {
                for (int j = 0; j < columns; j++)
                {
                    alignedMatrix.matrix[(rows + z), j] = 0;
                }
            }

            this.matrix = alignedMatrix.matrix;
            this.rows = alignedMatrix.rows;
        }
    }
}