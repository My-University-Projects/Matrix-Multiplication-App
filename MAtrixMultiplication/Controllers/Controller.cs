using MAtrixMultiplication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MAtrixMultiplicationWindow
{
    /// <summary>
    /// Controller class
    /// </summary>
    class Controller
    {
        /// <summary>
        /// Field with threads array
        /// </summary>
        private Thread[] threads;

        /// <summary>
        /// Field with first matrix
        /// </summary>
        private Matrix m1;

        /// <summary>
        /// Field with second matrix
        /// </summary>
        private Matrix m2;

        /// <summary>
        /// Field with result matrix
        /// </summary>
        private Matrix m3;

        /// <summary>
        /// Field with DLL option
        /// </summary>
        private Option option;

        /// <summary>
        /// Foield with Multithreading option
        /// </summary>
        private Multithreading multithreading;

        /// <summary>
        /// Threads property
        /// </summary>
        public Thread[] Threads
        {
            get => threads;
            set
            {
                threads = new Thread[value.Length];
            }
        }

        /// <summary>
        ///  First matrix property
        /// </summary>
        public Matrix M1
        {
            get => m1;
            set
            {
                m1 = value;
            }
        }

        /// <summary>
        /// Second matrix property
        /// </summary>
        public Matrix M2
        {
            get => m2;
            set
            {
                m2 = value;
            }
        }

        /// <summary>
        /// Result matrix property
        /// </summary>
        public Matrix M3
        {
            get => m3;
            set
            {
                m3 = value;
            }
        }

        /// <summary>
        /// Option DLL property
        /// </summary>
        public Option Option
        {
            get => option;
            set
            {
                option = value;
            }
        }

        /// <summary>
        /// Option multithrrading property
        /// </summary>
        public Multithreading Multithreading
        {
            get => multithreading;
            set
            {
                multithreading = value;
            }
        }

        /// <summary>
        /// Controller constructor
        /// </summary>
        public Controller() { }

        /// <summary>
        /// Method that generates matrixes
        /// </summary>
        /// <param name="window"></param>
        public void GenerateMatrixes(MainWindow window)
        {
            int m1Rows, m2Rows, m1Columns, m2Columns;
            if (this.CheckConditions(window, out string message, out m1Rows, out m2Rows, out m1Columns, out m2Columns) == false)
            {
                MessageBox.Show(message, "Błąd!");
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

        /// <summary>
        /// Method that manages multiplication of matrixes process
        /// </summary>
        /// <param name="option"></param>
        /// <param name="Window"></param>
        public void Multiplication(Option option, MainWindow Window)
        {
            if (this.m1.Columns != this.m2.Rows){
                MessageBox.Show("Podane w pliku macierze nie mogą zostać pomnożone!\nIlość kolumn macierzy pierwszej musi być równa ilości wierszy macierzy drugiej", "Błąd wymiarów");
                return;
            }
            string measuredTime = "";
            this.m3 = new Matrix(this.m1.Rows, this.m2.Columns);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            switch (this.multithreading){
                case Multithreading.OFF:{
                        this.SingleThreadMultiplication(Window, this.m1.Columns, this.m2.Columns, this.m1.Rows);
                        break;
                    }
                case Multithreading.ON:{
                        int threadsCount = this.threads.Length;
                        this.MultiThreadMultiplication(Window, this.m1.Rows, threadsCount, this.m2.Columns, this.m1.Columns);
                        break;
                    }
            }
            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            measuredTime = elapsedMs.ToString() + "ms";
            try{
                m3.PrintMatrixtoFile();
            }
            catch (System.NullReferenceException) { }
            finally{
                this.ClearAndReset(measuredTime);
            }
        }
        
        /// <summary>
        /// Method that cleares matrixes, resets threas and shows result message and file
        /// </summary>
        /// <param name="measuredTime"></param>
        private void ClearAndReset(string measuredTime){
            MessageBox.Show("Czas Wykonywania - " + measuredTime + "\nIlość użytych wątków - " + this.threads.Length.ToString() + "\nWynikowa macierz zapisana w pliku wynik.txt");
            this.ClearMatrixes((MainWindow)Application.Current.MainWindow);
            this.ResetThreads((MainWindow)Application.Current.MainWindow);
            this.ClearView((MainWindow)Application.Current.MainWindow);
            new Process{
                StartInfo = new ProcessStartInfo(@"wynik.txt"){
                        UseShellExecute = true
                    }
                }.Start();
            }

        /// <summary>
        /// Method that manages multiplicstion using one thread
        /// </summary>
        /// <param name="Window"></param>
        /// <param name="m1Columns"></param>
        /// <param name="m2Columns"></param>
        /// <param name="rows"></param>
        public void SingleThreadMultiplication(MainWindow Window, int m1Columns, int m2Columns, int rows){
            this.m2.Transponate();
            for (int rowsCount = 0; rowsCount < rows; rowsCount++){
                unsafe{
                    fixed (int* resultRow = &this.m3.matrix[rowsCount, 0])
                    fixed (int* rowToMultiply = &this.m1.matrix[rowsCount, 0])
                    fixed (int* colToMultiply = &this.m2.matrix[0, 0])
                        switch (this.option){
                            case Option.Cpp:{
                                    MatrixMultiplication.App.CppMultiplication(resultRow, rowToMultiply, colToMultiply, m2Columns, m1Columns);
                                    break;
                                }
                            case Option.Asm:{
                                    int[] args = new int[2];
                                    args[0] = m2Columns;
                                    args[1] = m1Columns;
                                    fixed (int* argsPtr = &args[0])
                                        MatrixMultiplication.App.AsmMultiplication(resultRow, rowToMultiply, colToMultiply, argsPtr);
                                    break;
                                }
                        }
                }
            }
        }

        /// <summary>
        /// Method that manages multiplications using more thanm one thread
        /// </summary>
        /// <param name="Window"></param>
        /// <param name="rows"></param>
        /// <param name="threadsCount"></param>
        /// <param name="columns"></param>
        /// <param name="m1Columns"></param>
        public void MultiThreadMultiplication(MainWindow Window, int rows, int threadsCount, int columns, int m1Columns){
            this.m2.Transponate();
            int rowsCount = 0;
            for (int i = 0; i < threadsCount; i++){
                this.threads[i] = this.StartTheThread(columns, rowsCount, rows, m1Columns);
                rowsCount++;
                if (i == (threadsCount - 1)){
                    if (rowsCount < rows){
                        for (int j = 0; j < threadsCount; j++){
                            this.threads[j].Join();
                        }
                        i = 0;
                    }
                }
                if (rowsCount == rows){
                    break;
                }
            }
        }

        /// <summary>
        /// Methos that manages loading matrix from file proxess
        /// </summary>
        /// <param name="whatMatrix"></param>
        /// <param name="window"></param>
        /// <param name="matrixPath"></param>
        public void LoadMatrixFromPath(int whatMatrix, MainWindow window, string matrixPath){
            if (matrixPath.Equals(null)){
                switch (whatMatrix){
                    case 1:{
                            this.M1 = new Matrix(0);
                            window.FirstMatrixPath.Text = "Wpisz poprawną ścieżkę!";
                            break;
                        }
                    case 2:{
                            this.M2 = new Matrix(0);
                            window.SecondMatrixPath.Text = "Wpisz poprawną ścieżkę!";
                            break;
                        }
                }
                return;
            }
            else if (matrixPath.Contains(".txt") == false){
                switch (whatMatrix){
                    case 1:{
                            this.M1 = new Matrix(0);
                            window.FirstMatrixPath.Text = "Wpisz poprawną ścieżkę!";
                            break;
                        }
                    case 2:{
                            this.M2 = new Matrix(0);
                            window.SecondMatrixPath.Text = "Wpisz poprawną ścieżkę!";
                            break;
                        }
                }
                return;
            }
            else
            {
                switch (whatMatrix){
                    case 1:{
                            this.m1 = Matrix.LoadMatrix(matrixPath);
                            if (m1.Columns % 4 != 0) {
                                m1.AlignColumns();
                            }
                            break;
                        }
                    case 2:{
                            this.m2 = Matrix.LoadMatrix(matrixPath);
                            if (m2.Columns % 4 != 0){
                                m2.AlignColumns();
                            }
                            break;
                        }
                }
                MessageBox.Show("Wczytano macierz!", "Sukces!");
                return;
            }
        }

        /// <summary>
        /// Method that manages loading threads number procees 
        /// </summary>
        /// <param name="window"></param>
        public void LoadThreads(MainWindow window){
            int numberOfThreads;
            try{
                numberOfThreads = Convert.ToInt32(window.NumberOfThreadsTextBox.Text);
            }
            catch (FormatException){
                this.threads = new Thread[0];
                window.NumberOfThreadsTextBox.Text = "Zła liczba wątków!";
                return;
            }
            if (numberOfThreads == 0){
                this.threads = new Thread[0];
                window.NumberOfThreadsTextBox.Text = "Zła liczba wątków!";
                return;
            }
            this.threads = new Thread[numberOfThreads];
            if (numberOfThreads == 1){
                this.multithreading = Multithreading.OFF;
            }
            else{
                this.multithreading = Multithreading.ON;
            }
            MessageBox.Show("Załadowano wątki!", "Sukces!");
            return;
        }

        /// <summary>
        /// Method that reset threads count
        /// </summary>
        /// <param name="window"></param>
        public void ResetThreads(MainWindow window){
            this.Threads = Array.Empty<Thread>();
        }

        /// <summary>
        /// Methos that clears view
        /// </summary>
        /// <param name="window"></param>
        public void ClearView(MainWindow window){
            window.FirstMatrixPath.Text = "";
            window.SecondMatrixPath.Text = "";
            window.M1RowsTextBox.Text = "";
            window.M2RowsTextBox.Text = "";
            window.M1ColumnsTextBox.Text = "";
            window.M2ColumnsTextBox.Text = "";
            window.NumberOfThreadsTextBox.Text = "";
        }

        /// <summary>
        /// Method that clears all matrixes
        /// </summary>
        /// <param name="window"></param>
        public void ClearMatrixes(MainWindow window){
            this.m1.ResetMatrix();
            this.m2.ResetMatrix();
            this.m3.ResetMatrix();
        }

        /// <summary>
        /// Method that checks if matrixes with sizes passed in args can be multuplied each other
        /// </summary>
        /// <param name="window"></param>
        /// <param name="message"></param>
        /// <param name="m1Rows"></param>
        /// <param name="m2Rows"></param>
        /// <param name="m1Columns"></param>
        /// <param name="m2Columns"></param>
        /// <returns></returns>
        public bool CheckConditions(MainWindow window, out string message, out int m1Rows, out int m2Rows, out int m1Columns, out int m2Columns){
            if (window.M1RowsTextBox.Text == null || window.M1ColumnsTextBox.Text == null || window.M2ColumnsTextBox == null || window.M2RowsTextBox == null){
                message = "Któryś z podanych wymiarów macierzy jest niepoprawmy!";
                m1Rows = m2Rows = m1Columns = m2Columns = 0;
                return false;
            }
            else{
                try{
                    m1Rows = Convert.ToInt32(window.M1RowsTextBox.Text);
                    m2Rows = Convert.ToInt32(window.M2RowsTextBox.Text);
                    m1Columns = Convert.ToInt32(window.M1ColumnsTextBox.Text);
                    m2Columns = Convert.ToInt32(window.M2ColumnsTextBox.Text);

                    if (m1Columns != m2Rows){
                        message = "Liczba kolumn w Macierzy pierwszej\nmusi być równa liczbie wierszy macierzy drugiej!";
                        m1Rows = m2Rows = m1Columns = m2Columns = 0;
                        return false;
                    }
                }
                catch (System.FormatException){
                    message = "Podaj liczby!";
                    m1Rows = m2Rows = m1Columns = m2Columns = 0;
                    return false;
                }
            }
            message = null;
            return true;
        }

        /// <summary>
        /// Functrion that will be used in threads
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rowsCount"></param>
        /// <param name="rows"></param>
        /// <param name="size"></param>
        public void ThreadedFunction(int columns, int rowsCount, int rows, int size){
            unsafe{
                fixed (int* resultRow = &m3.matrix[rowsCount, 0])
                fixed (int* rowToMultiply = &m1.matrix[rowsCount, 0])
                fixed (int* colToMultiply = &m2.matrix[0, 0])
                    switch (option){
                        case Option.Cpp:{
                                MatrixMultiplication.App.CppMultiplication(resultRow, rowToMultiply, colToMultiply, columns, size);
                                break;
                            }
                        case Option.Asm:{
                                int[] args = new int[] {
                                    columns,
                                    size
                                };
                                fixed (int* argsPtr = &args[0])
                                    MatrixMultiplication.App.AsmMultiplication(resultRow, rowToMultiply, colToMultiply, argsPtr);
                                break;
                         }
                    }
            }
        }

        /// <summary>
        /// Method that starts thread
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rowsCount"></param>
        /// <param name="rows"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Thread StartTheThread(int columns, int rowsCount, int rows, int size){
            var t = new Thread(() => ThreadedFunction(columns, rowsCount, rows, size));
            t.Start();
            return t;
        }

        /// <summary>
        /// Method that shows dialog box with instruction
        /// </summary>
        public void ShowInstruction()
        {
            MessageBox.Show("1. Wczytaj macierze z plików .txt(Pamiętaj o wymiarach macierzy!)\n" +
                            "lub wygeneruj dwie losowe macierze wpisując w pola tekstowe odpowiednie wymiary\n" +
                            "2. Wpisz w odpowiednie pole liczbę wątków, które mają zostać wykorzystane przy mnożeniu\n" +
                            "3. Wybierz metodę mnożenia wciskając odpowiedni przycisk  ", "Instrukcja");
        }
    }
}