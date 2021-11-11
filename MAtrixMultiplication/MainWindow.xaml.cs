using System;
using System.Windows;
using System.Threading;
using MAtrixMultiplicationWindow;

namespace MAtrixMultiplication
{
    public partial class MainWindow : Window
    {
        public Thread[] threads;
        public static Matrix m1;
        public static Matrix m2;
        public static Matrix m3;
        public static bool Asm;

        public MainWindow()
        {
            InitializeComponent();
        }



        private void GenerateMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            if (MatrixSize.Text == null || MatrixSize.Text == "")
            {
                this.MatrixSize.Text = "Wpisz wymiar!";
            }
            else
            {
                int size;
                try
                {

                    size = Convert.ToInt32(MatrixSize.Text);
                }
                catch (System.FormatException)
                {
                    this.MatrixSize.Text = "Niepoprawny wymiar!";
                    return;
                }
                //size = Convert.ToInt32(MatrixSize.Text);
                m1 = Matrix.GenerateMatrix(size);
                m2 = Matrix.GenerateMatrix(size);
                this.MatrixSize.Text = "Udało sie!";
            }
        }

        private void MatrixSize_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void MultiplicationInCppButton_Click(object sender, RoutedEventArgs e)
        {
            Asm = false;
            if (m1.GetSize() != m2.GetSize())
            {
                return;
            }
            String measuredTime;
            int size = m1.GetSize();
            m3 = new Matrix(size);
            if (threads.Length == 1)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                for (int rows = 0; rows < size; rows++)
                {
                    unsafe//
                    {//
                        fixed (int* rowAddress = &m3.matrix[rows, 0]) //
                        for (int columns = 0; columns < size; columns++)
                        {
                            Matrix.StartMultiplication(size, rows, columns, out m3.matrix[rows, columns], m1, m2, Asm, rowAddress);
                        }
                    }//
                }
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                measuredTime = elapsedMs.ToString() + "ms";
            }
            else
            {
                int threadsCount = threads.Length;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                int rows = 0;
                for (int i = 0; i < threadsCount; i++)
                {
                    threads[i] = this.StartTheThread(size, rows);
                    rows++;
                    if (i == (threadsCount - 1))
                    {
                        if (rows < size) { i = 0; }
                    }
                    if (rows == size) { break; }
                }
                for (int i = 0; i < threadsCount; i++)
                {
                    threads[i].Join();
                }
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                measuredTime = elapsedMs.ToString() + "ms";
            }
            m3.PrintMatrixtoFile();
            MessageBox.Show("Czas Wykonywania - " + measuredTime + "\n wynikowa macierz zapisana w pliku wynik.txt");
            m1.ResetMatrix();
            m2.ResetMatrix();
            m3.ResetMatrix();
            ResetThreads();
        }

        private void MultiplicationInAsmButton_Click(object sender, RoutedEventArgs e)
        {
            Asm = true;
            if (m1.GetSize() != m2.GetSize())
            {
                return;
            }
            String measuredTime;
            int size = m1.GetSize();
            m3 = new Matrix(size);
            if (threads.Length == 1)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                for (int rows = 0; rows < size; rows++)
                {
                    for (int columns = 0; columns < size; columns++)
                    {
                        Matrix.StartMultiplication(size, rows, columns, out m3.matrix[rows, columns], m1, m2, Asm);
                    }
                }
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                measuredTime = elapsedMs.ToString() + "ms";
            }
            else
            {
                int threadsCount = threads.Length;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                int rows = 0;
                for (int i = 0; i < threadsCount; i++)
                {
                    threads[i] = this.StartTheThread(size, rows);
                    rows++;
                    if (i == (threadsCount - 1))
                    {
                        if (rows < size) { i = 0; }
                    }
                    if (rows == size) { break; }
                }
                for (int i = 0; i < threadsCount; i++)
                {
                    threads[i].Join();
                }
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                measuredTime = elapsedMs.ToString() + "ms";
            }
            m3.PrintMatrixtoFile();
            MessageBox.Show("Czas Wykonywania - " + measuredTime + "\n wynikowa macierz zapisana w pliku wynik.txt");
            m1.ResetMatrix();
            m2.ResetMatrix();
            m3.ResetMatrix();
            ResetThreads();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void SaveFirstMatrixPathButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.FirstMatrixPath.Text.Contains(".txt") == false)
            {
                this.FirstMatrixPath.Text = "Nie znaleziono pliku!";
            }
            else if (this.FirstMatrixPath.Text == null || this.FirstMatrixPath == null)
            {
                this.FirstMatrixPath.Text = "Wpisz ścieżkę do pliku!";
            }
            else
            {
                string path = FirstMatrixPath.Text;
                m1 = Matrix.LoadMatrix(path);
                this.FirstMatrixPath.Text = "Udało sie!";
            }
        }

        private void SaveSecondMatrixPathButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SecondMatrixPath.Text.Contains(".txt") == false)
            {
                this.SecondMatrixPath.Text = "Nie znaleziono pliku!";
            }
            else if (this.SecondMatrixPath.Text == null)
            {
                this.SecondMatrixPath.Text = "Wpisz ścieżkę do pliku!";
            }
            else
            {
                string path = SecondMatrixPath.Text;
                m2 = Matrix.LoadMatrix(path);
                this.SecondMatrixPath.Text = "Udało sie!";
            }
        }

        private void NumberOfThreadsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }

        private void LoadThreadsButton_Click(object sender, RoutedEventArgs e)
        {
            int numberOfThreads;
            try
            {
                numberOfThreads = Convert.ToInt32(this.NumberOfThreadsTextBox.Text);
            }
            catch (FormatException)
            {
                this.NumberOfThreadsTextBox.Text = "Zła liczba wątków!";
                return;
            }
            if(numberOfThreads == 0)
            {
                this.NumberOfThreadsTextBox.Text = "Zła liczba wątków!";
                return;
            }
            threads = new Thread[numberOfThreads];
            this.NumberOfThreadsTextBox.Text = "Załadowano!";

        }

        public void ThreadedFunction(int size, int rows)
        {
            for (int columns = 0; columns < size; columns++)
            {
                Matrix.StartMultiplication(size, rows, columns, out m3.matrix[rows, columns], m1, m2, Asm);
            }
        }

        public Thread StartTheThread(int size, int rows)
        {
            var t = new Thread(() => ThreadedFunction(size, rows));
            t.Start();
            return t;
        }

        private void MeasuredTimeTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        public void ResetThreads()
        {
            this.threads = Array.Empty<Thread>();
        }

    }
}
