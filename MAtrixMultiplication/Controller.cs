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
        public Controller() { }

        public void GenerateMatrixes(MainWindow Window)
        {
            if (Window.MatrixSize.Text == null || Window.MatrixSize.Text == "")
            {
                Window.MatrixSize.Text = "Wpisz wymiar!";
            }
            else
            {
                int size;
                try
                {

                    size = Convert.ToInt32(Window.MatrixSize.Text);
                }
                catch (System.FormatException)
                {
                    Window.MatrixSize.Text = "Niepoprawny wymiar!";
                    return;
                }
                Window.SetMatrix1(Matrix.GenerateMatrix(size));
                Window.SetMatrix2(Matrix.GenerateMatrix(size));
                Window.MatrixSize.Text = "Udało sie!";
            }
        }

        public void Multiplication(MainWindow Window)
        {
            if (Window.GetMatrix1().GetSize() != Window.GetMatrix2().GetSize())
            {
                return;
            }
            string measuredTime = "";
            int size = Window.GetMatrix1().GetSize();
            Window.SetResultMatrix(new Matrix(size));
            switch (Window.GetMultithreading())
            {
                case Multithreading.OFF:
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        this.SingleThreadMultiplication(Window, size);
                        watch.Stop();
                        long elapsedMs = watch.ElapsedMilliseconds;
                        measuredTime = elapsedMs.ToString() + "ms";
                        break;
                    }
                case Multithreading.ON:
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        int threadsCount = Window.GetThreadsLenght();
                        this.MultiThreadMultiplication(Window, size, threadsCount);
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        measuredTime = elapsedMs.ToString() + "ms";
                        break;
                    }
            }
            MessageBox.Show("Czas Wykonywania - " + measuredTime + "\n wynikowa macierz zapisana w pliku wynik.txt");
        }

        public void SingleThreadMultiplication(MainWindow Window, int size)
        {
            for (int rows = 0; rows < size; rows++)
            {
                unsafe
                {
                    fixed (int* resultRow = &Window.GetResultMatrix().matrix[rows, 0])
                    fixed (int* rowToMultiply = &Window.GetMatrix1().matrix[rows, 0])
                    fixed (int* colToMultiply = &Window.GetMatrix2().matrix[0, 0])
                        switch (Window.GetOption())
                        {
                            case Option.Cpp:
                                {
                                    MatrixMultiplication.App.multiply(resultRow, rowToMultiply, colToMultiply, size);
                                    break;
                                }
                            case Option.Asm:
                                {
                                    MatrixMultiplication.App.AsmMultiplication(resultRow, rowToMultiply, colToMultiply, size);
                                    break;
                                }
                        }
                }
            }
        }

        public void MultiThreadMultiplication(MainWindow Window, int size, int threadsCount)
        {
            int rows = 0;
            for (int i = 0; i < threadsCount; i++)
            {
                Window.GetThreads()[i] = Window.StartTheThread(size, rows);
                rows++;
                if (i == (threadsCount - 1))
                {
                    if (rows < size) { i = 0; }
                }
                if (rows == size) { break; }
            }
            for (int i = 0; i < threadsCount; i++)
            {
                Window.GetThreads()[i].Join();
            }
        }

        public string LoadMatrixFromPath(out Matrix matrix, MainWindow window, string matrixPath)
        {
            if (matrixPath.Equals(null))
            {
                matrix = new Matrix(0);
                return "Wpisz ścieżkę do pliku!";
            }
            else if (matrixPath.Contains(".txt") == false)
            {
                matrix = new Matrix(0);
                return "Nie znaleziono pliku!";
            }
            else
            {
                matrix = Matrix.LoadMatrix(matrixPath);
                return "Udało sie!";
            }
        }

        public string LoadThreads(MainWindow window)
        {
            int numberOfThreads;
            try
            {
                numberOfThreads = Convert.ToInt32(window.NumberOfThreadsTextBox.Text);
            }
            catch (FormatException)
            {
                window.SetThreads (new Thread[0]);
                return "Zła liczba wątków!";
            }
            if (numberOfThreads == 0)
            {
                window.SetThreads( new Thread[0]);
                return "Zła liczba wątków!";
            }
            window.SetThreads(new Thread[numberOfThreads]);
            if (numberOfThreads == 1)
            {
                window.SetMultithreading(Multithreading.OFF);
            }
            else
            {
                window.SetMultithreading(Multithreading.ON);
            }
           return "Załadowano!";
        }

        public void ResetThreads(MainWindow window)
        {
            window.SetThreads(Array.Empty<Thread>());
        }

        public void ClearView(MainWindow window)
        {
            window.FirstMatrixPath.Text = "";
            window.SecondMatrixPath.Text = "";
            window.MatrixSize.Text = "";
            window.NumberOfThreadsTextBox.Text = "";
        }

        public void ClearMatrixes(MainWindow window)
        {
            window.GetMatrix1().ResetMatrix();
            window.GetMatrix2().ResetMatrix();
            window.GetResultMatrix().ResetMatrix();
        }
    }
}
