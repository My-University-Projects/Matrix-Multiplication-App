using System;
using System.Windows;
using System.Threading;
using MAtrixMultiplicationWindow;

namespace MAtrixMultiplication
{
    /// <summary>
    /// Main Window class
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Controller field
        /// </summary>
        Controller controller;

        /// <summary>
        /// Constructor of the Window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.controller = new Controller();
        }
        /// <summary>
        /// Method is called when the generate matrix button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            controller.GenerateMatrixes((MainWindow)Application.Current.MainWindow);
        }

        /// <summary>
        /// Method is called when the multiplication in CPP button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiplicationInCppButton_Click(object sender, RoutedEventArgs e)
        {

            controller.Multiplication(Option.Cpp, (MainWindow)Application.Current.MainWindow);
        }

        /// <summary>
        /// Method is called when the multiplication in Asm button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiplicationInAsmButton_Click(object sender, RoutedEventArgs e)
        {
            controller.Multiplication(Option.Asm, (MainWindow)Application.Current.MainWindow);
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Method is called when the save first matrix path button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFirstMatrixPathButton_Click(object sender, RoutedEventArgs e)
        {
            controller.LoadMatrixFromPath(1, (MainWindow)Application.Current.MainWindow, FirstMatrixPath.Text);
        }

        /// <summary>
        /// Method is called when the save second matrix path button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSecondMatrixPathButton_Click(object sender, RoutedEventArgs e)
        {
            controller.LoadMatrixFromPath(2, (MainWindow)Application.Current.MainWindow, SecondMatrixPath.Text);
        }

        private void NumberOfThreadsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { }

        /// <summary>
        /// Method is called when the load threads button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            controller.ShowInstruction();
        }
    }
}