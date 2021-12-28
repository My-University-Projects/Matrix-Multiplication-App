using MAtrixMultiplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MAtrixMultiplicationWindow
{
    class Controller
    {

        private Thread[] threads;
        private Matrix m1;
        private Matrix m2;
        private Matrix m3;
        private Option option;
        private Multithreading multithreading;

        public Thread[] Threads { get => threads; set { threads = new Thread[value.Length]; } }
        public Matrix M1 { get => m1; set { m1 = value; } }
        public Matrix M2 { get => m2; set { m2 = value; } }
        public Matrix M3 { get => m3; set { m3 = value; } }
        public Option Option { get => option; set { option = value; } }
        public Multithreading Multithreading { get => multithreading; set { multithreading = value; } }


        public Controller() { }

        public void GenerateMatrixes(MainWindow window)
        {
            int m1Rows, m2Rows, m1Columns, m2Columns;
            if(this.CheckConditions(window, out string message, out  m1Rows, out m2Rows, out m1Columns, out m2Columns) == false)
            {
                MessageBox.Show(message);
                ClearView(window);
            }
            else
            {
               this.m1 = Matrix.GenerateMatrix(m1Rows, m1Columns);
               this.m2 = Matrix.GenerateMatrix(m2Rows, m2Columns);

                if (this.m1.Columns % 4 != 0)
                {
                    this.m1.AlignColumns();
                }
                if (this.m2.Rows % 4 != 0)
                {
                    this.m2.AlignRows();
                }

                MessageBox.Show("Załadowano macierze!", "Sukces!");
            }
        }

        public void Multiplication(Option option, MainWindow Window)
        {
            if (this.m1.Columns != this.m2.Rows)
            {
                MessageBox.Show("Podane w pliku macierze nie mogą zostać pomnożone!\nIlość kolumn macierzy pierwszej musi być równa ilości wierszy macierzy drugiej", "Błąd wymiarów");
                return;
            }

            this.option = option;

            string measuredTime = "";
            int rows, columns, size;
            rows = this.m1.Rows;
            columns = this.m2.Columns;
            size = this.m1.Columns;

            if(option == Option.Cpp)
            {
                this.m3 = new Matrix(rows, columns);
            }
            else
            {
                if(columns % 2 == 1)
                {
                    this.m3 = new Matrix(rows, columns);
                }
                else
                {
                    this.m3 = new Matrix(rows, columns);
                }

            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            switch (this.multithreading)
            {
                case Multithreading.OFF:
                    {
                        this.SingleThreadMultiplication(Window, size, columns, rows);
                        break;
                    }
                case Multithreading.ON:
                    {
                        int threadsCount = this.threads.Length;
                        this.MultiThreadMultiplication(Window, rows, threadsCount, columns, size);
                        break;
                    }
            }
            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            measuredTime = elapsedMs.ToString() + "ms";
            try
            {
                m3.PrintMatrixtoFile();
            }
            catch (System.NullReferenceException)
            {

            }
            finally
            {
                MessageBox.Show("Czas Wykonywania - " + measuredTime + "\nIlość użytych wątków - " + this.threads.Length.ToString() + "\nWynikowa macierz zapisana w pliku wynik.txt");
                this.ClearMatrixes((MainWindow)Application.Current.MainWindow);
                this.ResetThreads((MainWindow)Application.Current.MainWindow);
                this.ClearView((MainWindow)Application.Current.MainWindow);
            }
        }

        public void SingleThreadMultiplication(MainWindow Window, int size, int columns, int rows)
        {
           this.m2.Transponate();
            for (int rowsCount = 0; rowsCount < rows; rowsCount++)
            {
                unsafe
                { 
                    fixed (int* resultRow = &this.m3.matrix[rowsCount, 0])
                    fixed (int* rowToMultiply = &this.m1.matrix[rowsCount, 0])
                    fixed (int* colToMultiply = &this.m2.matrix[0, 0])
                        switch (this.option)
                        {
                            case Option.Cpp:
                                {
                                    MatrixMultiplication.App.CppMultiplication(resultRow, rowToMultiply, colToMultiply, columns, size);
                                    break;
                                }
                            case Option.Asm:
                                {
                                    int[] args = new int[2];
                                    args[0] = columns; args[1] = size;
                                    fixed (int* argsPtr = &args[0])
                                        MatrixMultiplication.App.AsmMultiplication(resultRow, rowToMultiply, colToMultiply, argsPtr);
                                    break;
                                }
                        }
                }
            }
        }

        public void MultiThreadMultiplication(MainWindow Window, int rows, int threadsCount, int columns, int size)
        {
            this.m2.Transponate();
            int rowsCount = 0;
            for (int i = 0; i < threadsCount; i++)
            {
                this.threads[i] = this.StartTheThread(columns, rowsCount, rows, size);
                rowsCount++;
                if (i == (threadsCount - 1))
                {
                    if (rowsCount < rows) 
                    {
                        for (int j = 0; j < threadsCount; j++)
                        {
                            this.threads[j].Join();
                        }
                        i = 0;
                    }
                }
                if (rowsCount == rows) { break; }
            }
            
        }

        public void LoadMatrixFromPath(int whatMatrix, MainWindow window, string matrixPath)
        {
            if (matrixPath.Equals(null))
            {
                switch (whatMatrix)
                {
                    case 1:
                        {
                            this.M1 = new Matrix(0);
                            window.FirstMatrixPath.Text = "Wpisz poprawną ścieżkę!";
                            break;
                        }
                    case 2:
                        {
                            this.M2 = new Matrix(0);
                            window.SecondMatrixPath.Text = "Wpisz poprawną ścieżkę!";
                            break;
                        }
                }
                return;
            }
            else if (matrixPath.Contains(".txt") == false)
            {
                switch (whatMatrix)
                {
                    case 1:
                        {
                            this.M1 = new Matrix(0);
                            window.FirstMatrixPath.Text = "Wpisz poprawną ścieżkę!";
                            break;
                        }
                    case 2:
                        {
                            this.M2 = new Matrix(0);
                            window.SecondMatrixPath.Text = "Wpisz poprawną ścieżkę!";
                            break;
                        }
                }
                return;
            }
            else
            {
                switch (whatMatrix)
                {
                    case 1:
                        {
                            this.M1 = Matrix.LoadMatrix(matrixPath);
                            if (m1.Columns % 4 != 0)
                            {
                                m1.AlignColumns();
                            }
                            break;
                        }
                    case 2:
                        {
                            this.M2 = Matrix.LoadMatrix(matrixPath);
                            if (m2.Columns % 4 != 0)
                            {
                                m2.AlignColumns();
                            }
                            break;
                        }
                }
                MessageBox.Show("Wczytano macierz!", "Sukces!");
                return;
            }
        }

        public void LoadThreads(MainWindow window)
        {
            int numberOfThreads;
            try
            {
                numberOfThreads = Convert.ToInt32(window.NumberOfThreadsTextBox.Text);
            }
            catch (FormatException)
            {
                this.threads = new Thread[0];
                window.NumberOfThreadsTextBox.Text = "Zła liczba wątków!";
                return;
            }
            if (numberOfThreads == 0)
            {
                this.threads = new Thread[0];
                window.NumberOfThreadsTextBox.Text = "Zła liczba wątków!";
                return;
            }
            this.threads = new Thread[numberOfThreads];
            if (numberOfThreads == 1)
            {
                this.multithreading = Multithreading.OFF;
            }
            else
            {
                this.multithreading = Multithreading.ON;
            }
            MessageBox.Show("Załadowano wątki!", "Sukces!");
            return;
        }

        public void ResetThreads(MainWindow window)
        {
            this.Threads = Array.Empty<Thread>();
        }

        public void ClearView(MainWindow window)
        {
            window.FirstMatrixPath.Text = "";
            window.SecondMatrixPath.Text = "";
            window.M1RowsTextBox.Text = "";
            window.M2RowsTextBox.Text = "";
            window.M1ColumnsTextBox.Text = "";
            window.M2ColumnsTextBox.Text = "";
            window.NumberOfThreadsTextBox.Text = "";
        }

        public void ClearMatrixes(MainWindow window)
        {
            this.m1.ResetMatrix();
            this.m2.ResetMatrix();
            this.m3.ResetMatrix();
        }

        public bool CheckConditions(MainWindow window, out string message, out int m1Rows, out int m2Rows, out int m1Columns, out int m2Columns)
        {
            if (window.M1RowsTextBox.Text == null || window.M1ColumnsTextBox.Text == null || window.M2ColumnsTextBox == null || window.M2RowsTextBox == null)
            {
                message = "Któryś z podanych wymiarów macierzy jest niepoprawmy!";
                m1Rows = m2Rows = m1Columns = m2Columns = 0;
                return false;
            }
            else
            {
                try
                {
                    m1Rows = Convert.ToInt32(window.M1RowsTextBox.Text);
                    m2Rows = Convert.ToInt32(window.M2RowsTextBox.Text);
                    m1Columns = Convert.ToInt32(window.M1ColumnsTextBox.Text);
                    m2Columns = Convert.ToInt32(window.M2ColumnsTextBox.Text);

                    if(m1Columns != m2Rows)
                    {
                        message = "Liczba kolumn w Macierzy pierwszej\nmusi być równa liczbie wierszy macierzy drugiej!";
                        m1Rows = m2Rows = m1Columns = m2Columns = 0;
                        return false;
                    }
                }
                catch (System.FormatException)
                {
                    message = "Podaj liczby!";
                    m1Rows = m2Rows = m1Columns = m2Columns = 0;
                    return false;
                }
            }
            message = null;
            return true;
        }

        public void ThreadedFunction(int columns, int rowsCount, int rows, int size)
        {
            unsafe
            {
                fixed (int* resultRow = &m3.matrix[rowsCount, 0])
                fixed (int* rowToMultiply = &m1.matrix[rowsCount, 0])
                fixed (int* colToMultiply = &m2.matrix[0, 0])
                    switch (option)
                    {
                        case Option.Cpp:
                            {
                                MatrixMultiplication.App.CppMultiplication(resultRow, rowToMultiply, colToMultiply, columns, size);
                                break;
                            }
                        case Option.Asm:
                            {
                                int[] args = new int[] { columns, size };
                                fixed (int* argsPtr = &args[0])
                                    MatrixMultiplication.App.AsmMultiplication(resultRow, rowToMultiply, colToMultiply, argsPtr);
                                break;
                            }
                    }
            }
        }

        public Thread StartTheThread(int columns, int rowsCount, int rows, int size)
        {
            var t = new Thread(() => ThreadedFunction(columns, rowsCount, rows, size));
            t.Start();
            return t;
        }
    }
}
