using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAtrixMultiplicationWindow
{
    /// <summary>
    /// Class representanting a matrix
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// Matrix  2D array field
        /// </summary>
        public int[,] matrix;

        /// <summary>
        /// Matrix size field
        /// </summary>
        private int matrixSize;

        /// <summary>
        /// Field that contains rows number of matrix
        /// </summary>
        private int rows;

        /// <summary>
        /// Field that contains columns number of matrix
        /// </summary>
        private int columns;

        /// <summary>
        /// Matrix size field property
        /// </summary>
        public int MatrixSize
        {
            get => matrixSize;
            set
            {
                matrixSize = value;
            }
        }

        /// <summary>
        /// Matrix rows number property
        /// </summary>
        public int Rows
        {
            get => rows;
            set
            {
                rows = value;
            }
        }

        /// <summary>
        /// Matrix columns number property
        /// </summary>
        public int Columns
        {
            get => columns;
            set
            {
                columns = value;
            }
        }

        /// <summary>
        /// Matrix constructor used to generate empty 0x0 matrix
        /// </summary>
        /// <param name="size"></param>
        /// Size of the matrix
        public Matrix(int size)
        {
            this.matrixSize = size;
            this.matrix = new int[size, size];
        }

        /// <summary>
        /// Matrix constructor used to generate rowsxcolumns matrix
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            this.matrix = new int[rows, columns];
        }

        /// <summary>
        /// Method that loads a matrix from file which name is passed in argument
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Loaded matrix</returns>
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
                    if (line == null)
                    {
                        return matrix;
                    }
                    lineIterator = 0;
                    for (int j = 0; j < columns; j++)
                    {
                        number = "";
                        while (lineIterator < line.Length)
                        {
                            if (line[lineIterator] == ' ')
                            {
                                lineIterator++;
                                break;
                            }
                            tmp = line[lineIterator].ToString();
                            number += tmp;
                            lineIterator++;
                        }
                        if (int.TryParse(number, out val) == true)
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

        /// <summary>
        /// Method that generates rowsxcolumns passed in args matrix with random values 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <returns>Generated matrix</returns>
        public static Matrix GenerateMatrix(int rows, int columns)
        {
            Matrix matrix = new Matrix(rows, columns);
            System.Random rnd = new System.Random();
            int x;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    x = rnd.Next(1, 9);
                    matrix.matrix[i, j] = x; //j + 1;
                }
            }
            return matrix;
        }

        /// <summary>
        /// Method that prints result matrix to file wynik.txt
        /// </summary>
        public void PrintMatrixtoFile()
        {
            using (StreamWriter writer = new StreamWriter("wynik.txt"))
            {
                writer.Write(rows + " " + columns);
                writer.WriteLine();
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        string x = matrix[i, j].ToString();
                        writer.Write(x + " ");
                    }
                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        /// Method that cleares matrix
        /// </summary>
        public void ResetMatrix()
        {
            Matrix m = new Matrix(0);
            this.matrix = m.matrix;
            this.matrixSize = m.matrixSize;
        }

        /// <summary>
        /// Method that transponates matrix
        /// </summary>
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

        /// <summary>
        /// Method that alligns matrix columns to 4
        /// </summary>
        public void AlignColumns()
        {
            int alignedColumns = 4 - (this.columns % 4);
            Matrix alignedMatrix = new Matrix(this.rows, (this.columns + alignedColumns));
            for (int i = 0; i < rows; i++)
            {
                for (int z = 0; z < alignedColumns; z++)
                {
                    alignedMatrix.matrix[i, (columns + z)] = 0;
                }
                for (int j = 0; j < columns; j++)
                {
                    alignedMatrix.matrix[i, j] = matrix[i, j];
                }
            }

            this.matrix = alignedMatrix.matrix;
            this.columns = alignedMatrix.columns;
        }

        /// <summary>
        /// Method that aligns matrix rows to 4
        /// </summary>
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

            for (int z = 0; z < alignedRows; z++)
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