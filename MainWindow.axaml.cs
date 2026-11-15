using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;


namespace Calculator;

public partial class MainWindow : Window
{
    private readonly CalculatorLogic _calculatorLogic;

    public MainWindow()
    {
        InitializeComponent();
        _calculatorLogic = new CalculatorLogic();
        _calculatorLogic.OnDisplayChanged += DisplayChanged;
        DisplayChanged();
    }

    private void OnNumberButtonClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && Int32.TryParse(button.Content?.ToString(), out int digit))
        {
            _calculatorLogic.AddDigit(digit); 
        }
    }

    private void OnCalculateClick(object? sender, RoutedEventArgs e)
    {
        string op = (sender as Button)?.Content?.ToString() ?? "";
        if (op == "=")
            _calculatorLogic.Calculate();
        else
            _calculatorLogic.SetOperator(op);
    }
    
    public void OnBackspaceClick(object? sender, RoutedEventArgs e)
    {
        _calculatorLogic.Backspace();
    }
    
    public void OnClearClick(object? sender, RoutedEventArgs e)
    {
        _calculatorLogic.Clear();
    }
    
    private void DisplayChanged()
    {
        Calculation.Text = _calculatorLogic.PrimaryDisplay;
        Result.Text = _calculatorLogic.SecondaryDisplay;
    }
    
    ~MainWindow()
    {
        _calculatorLogic.OnDisplayChanged -= DisplayChanged;
    }
}