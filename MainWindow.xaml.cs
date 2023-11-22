using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfCalculatorApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await CSharpScript.EvaluateAsync("1 + 1", ScriptOptions.Default.WithImports("System.Math"));
        }

        private void CeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text))
            {
                int index = InputTextBox.Text.LastIndexOfAny(new char[] { '+', '/', '*', '-' });
                InputTextBox.Text = InputTextBox.Text.Substring(0, index + 1);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text))
            {
                InputTextBox.Text = InputTextBox.Text.Substring(0, InputTextBox.Text.Length - 1);
            }
        }

        private void NumpadButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                InputTextBox.Text += button.Content;
            }
        }

        private async void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text))
            {
                try
                {
                    var result = await CSharpScript.EvaluateAsync(InputTextBox.Text, ScriptOptions.Default.WithImports("System.Math"));
                    ResultTextBox.Text = result.ToString();
                }
                catch (CompilationErrorException ex)
                {
                    ResultTextBox.Text = $"Error: {string.Join(Environment.NewLine, ex.Diagnostics)}";
                }
                catch (Exception ex)
                {
                    ResultTextBox.Text = $"Error: {ex.Message}";
                }
            }
        }

        private void CButton_Click(object sender, RoutedEventArgs e)
        {
            InputTextBox.Clear();
            ResultTextBox.Clear();
        }
    }
}
