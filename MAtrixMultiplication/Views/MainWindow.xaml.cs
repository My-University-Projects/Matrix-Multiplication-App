using System;
using System.Windows;
using System.Threading;
using MAtrixMultiplicationWindow;

namespace MAtrixMultiplication
{
    public partial class MainWindow : Window
    {
        Controller controller;

        public MainWindow()
        {
            InitializeComponent();
            this.controller = new Controller();
        }
        private void GenerateMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            controller.GenerateMatrixes((MainWindow)Application.Current.MainWindow);
        }

        private void MultiplicationInCppButton_Click(object sender, RoutedEventArgs e)
        {

            controller.Multiplication(Option.Cpp, (MainWindow)Application.Current.MainWindow);
        }

        private void MultiplicationInAsmButton_Click(object sender, RoutedEventArgs e)
        {
            controller.Multiplication(Option.Asm, (MainWindow)Application.Current.MainWindow);
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void SaveFirstMatrixPathButton_Click(object sender, RoutedEventArgs e)
        {
            controller.LoadMatrixFromPath(1, (MainWindow)Application.Current.MainWindow, FirstMatrixPath.Text);
        }

        private void SaveSecondMatrixPathButton_Click(object sender, RoutedEventArgs e)
        {
            controller.LoadMatrixFromPath(2, (MainWindow)Application.Current.MainWindow, SecondMatrixPath.Text);
        }

        private void NumberOfThreadsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }

        private void LoadThreadsButton_Click(object sender, RoutedEventArgs e)
        {
            controller.LoadThreads((MainWindow)Application.Current.MainWindow);
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