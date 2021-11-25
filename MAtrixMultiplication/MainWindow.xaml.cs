using System;
using System.Windows;
using System.Threading;
using MAtrixMultiplicationWindow;

namespace MAtrixMultiplication
{
    public partial class MainWindow : Window
    {
        Controller controller;
        Thread[] threads;
        Matrix m1;
        Matrix m2;
        Matrix m3;
        Option option;
        Multithreading multithreading;

        public MainWindow()
        {
            InitializeComponent();
            this.controller = new Controller();
        }


        /////////////////////////////////////////////////////////////   GETTERS AND SETTERS ///////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Multithreading GetMultithreading()
        {
            return multithreading;
        }

        public void SetMultithreading(Multithreading multithreading)
        {
            this.multithreading = multithreading;
        }

        public Option GetOption()
        {
            return this.option;
        }

        public void SetOption(Option option)
        {
            this.option = option;
        }

        public Matrix GetMatrix1()
        {
            return m1;
        }

        public void SetMatrix1(Matrix matrix)
        {
            this.m1 = matrix;
        }

        public Matrix GetMatrix2()
        {
            return m2;
        }

        public void SetMatrix2(Matrix matrix)
        {
            this.m2 = matrix;
        }

        public Matrix GetResultMatrix()
        {
            return m3;
        }

        public void SetResultMatrix(Matrix matrix)
        {
            this.m3 = matrix;
        }

        public Thread[] GetThreads()
        {
            return this.threads;
        }

        public void SetThreads(Thread[] threads)
        {
            this.threads = new Thread[threads.Length];
        }

        public int GetThreadsLenght()
        {
            return this.threads.Length;
        }
        /////////////////////////////////////////////////////////////   GETTERS AND SETTERS ///////////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





        private void GenerateMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            controller.GenerateMatrixes((MainWindow)Application.Current.MainWindow);
        }

        private void MultiplicationInCppButton_Click(object sender, RoutedEventArgs e)
        {
            SetOption(Option.Cpp);
            controller.Multiplication((MainWindow)Application.Current.MainWindow);
            try
            {
                m3.PrintMatrixtoFile();
            }
            catch(System.NullReferenceException)
            {

            }
            finally
            {
                controller.ClearMatrixes((MainWindow)Application.Current.MainWindow);
                controller.ResetThreads((MainWindow)Application.Current.MainWindow);
                controller.ClearView((MainWindow)Application.Current.MainWindow);
            } 
        }

        private void MultiplicationInAsmButton_Click(object sender, RoutedEventArgs e)
        {
            SetOption(Option.Asm);
            controller.Multiplication((MainWindow)Application.Current.MainWindow);
            m3.PrintMatrixtoFile();
            controller.ClearMatrixes((MainWindow)Application.Current.MainWindow);
            controller.ResetThreads((MainWindow)Application.Current.MainWindow);
            controller.ClearView((MainWindow)Application.Current.MainWindow);
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void SaveFirstMatrixPathButton_Click(object sender, RoutedEventArgs e)
        {
            FirstMatrixPath.Text = controller.LoadMatrixFromPath(out m1, (MainWindow)Application.Current.MainWindow, FirstMatrixPath.Text);
        }

        private void SaveSecondMatrixPathButton_Click(object sender, RoutedEventArgs e)
        {
            SecondMatrixPath.Text = controller.LoadMatrixFromPath(out m2, (MainWindow)Application.Current.MainWindow, SecondMatrixPath.Text);
        }

        private void NumberOfThreadsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }

        private void LoadThreadsButton_Click(object sender, RoutedEventArgs e)
        {
            NumberOfThreadsTextBox.Text = controller.LoadThreads((MainWindow)Application.Current.MainWindow);
        }

        public void ThreadedFunction(int columns, int rowsCount, int rows, int columnsAfterAlign, int size)
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
                                int[] args = new int[] { rows, columns, columnsAfterAlign };
                                fixed (int* argsPtr = &args[0])
                                    MatrixMultiplication.App.AsmMultiplication(resultRow, rowToMultiply, colToMultiply, rows);
                                break;
                            }
                    }
                    
            }
        }

        public Thread StartTheThread(int columns, int rowsCount, int rows, int columnsAfterAlign, int size)
        {
            var t = new Thread(() => ThreadedFunction(columns, rowsCount, rows, columnsAfterAlign, size));
            t.Start();
            return t;
        }

        private void MeasuredTimeTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void M1RowsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void M1ColumnsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void M2RowsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void M2ColumnsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}