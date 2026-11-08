using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Calculator;

public partial class MainWindow : Window
{
    private List<double> numbers = new List<double> { 0 };
    private List<string> operations = new List<string>();
    private int numberIndex = 0;
    private bool _hasTypedCurrentNumber = false;
    private bool _shouldResetOnNextNumber = false;

    public MainWindow()
    {
        InitializeComponent();
        UpdateDisplay();
    }

    private void OnNumberButtonClick(object? sender, RoutedEventArgs e)
    {
        Button? button = (Button)sender!;
        if (Int32.TryParse(button.Content?.ToString(), out int digit))
        {
            if (_shouldResetOnNextNumber)
            {
                numbers = new List<double> { 0 };
                operations.Clear();
                numberIndex = 0;
                _shouldResetOnNextNumber = false;
                Result.Text = ""; 
            }

            UpdateNumber(digit);
            _hasTypedCurrentNumber = true; 
            UpdateDisplay();
        }
    }

    private void UpdateNumber(int digit)
    {
        numbers[numberIndex] = numbers[numberIndex] * 10 + digit;
    }

    private void OnCalculateClick(object? sender, RoutedEventArgs e)
    {
        Button? button = (Button)sender!;
        string op = button.Content?.ToString() ?? "";

        if (op == "=")
        {
            if (operations.Count > 0 && numberIndex == 1)
            {
                double res = CalculateSingleOperation(numbers[0], operations[0], numbers[1]);
                Result.Text = res.ToString();
                numbers = new List<double> { res };
                operations.Clear();
                numberIndex = 0;
                _hasTypedCurrentNumber = false;
                _shouldResetOnNextNumber = true; 
            }
        }
        else 
        {
            _shouldResetOnNextNumber = false;

            if (numberIndex == 0) 
            {
                if (operations.Count == 0) 
                {
                    operations.Add(op);
                    numbers.Add(0); 
                    numberIndex = 1;
                }
                else 
                {
                    operations[0] = op;
                    if (numbers.Count == 1) numbers.Add(0);
                    numberIndex = 1;
                }
                _hasTypedCurrentNumber = false;
                UpdateDisplay(); 
            }
            else if (numberIndex == 1) 
            {
                if (!_hasTypedCurrentNumber) 
                {
                    operations[0] = op; 
                    UpdateDisplay(); 
                }
                else 
                {
                    double res = CalculateSingleOperation(numbers[0], operations[0], numbers[1]);
                    Result.Text = res.ToString();
                    
                    numbers[0] = res;       
                    operations[0] = op;     
                    numbers[1] = 0;         
                    numberIndex = 1;
                    _hasTypedCurrentNumber = false; 
                    
                    UpdateDisplay(); 
                }
            }
        }
    }

    private double CalculateSingleOperation(double num1, string op, double num2)
    {
        double result = num1;

        switch (op)
        {
            case "+":
                result = num1 + num2;
                break;
            case "-":
                result = num1 - num2;
                break;
            case "*":
                result = num1 * num2;
                break;
            case "/":
                if (num2 != 0)
                {
                    result = num1 / num2;
                }
                else
                {
                    result = 0; 
                }
                break;
        }
        return result;
    }

    private void UpdateDisplay()
    {
        string text = numbers[0].ToString();
        
        if (operations.Count > 0)
        {
            text += $" {operations[0]} "; 
            
            if (numbers.Count > 1)
            {
                text += numbers[1].ToString(); 
            }
        }
        
        Calculation.Text = text;
    }
    
    public void OnBackspaceClick(object? sender, RoutedEventArgs e)
    {
        if (_shouldResetOnNextNumber)
        {
            OnClearClick(null, null);
            return;
        }
        if (!_hasTypedCurrentNumber && numberIndex == 1)
        {
            operations.Clear();
            numbers.RemoveAt(1);
            numberIndex = 0;
            _hasTypedCurrentNumber = true; 
            UpdateDisplay();
            return;
        }
        numbers[numberIndex] = Math.Truncate(numbers[numberIndex] / 10);
        UpdateDisplay();
    }
    
    public void OnClearClick(object? sender, RoutedEventArgs e)
    {
        numbers = new List<double> { 0 };
        operations.Clear();
        numberIndex = 0;
        _hasTypedCurrentNumber = false;
        _shouldResetOnNextNumber = false;
        
        Calculation.Text = "0";
        Result.Text = "";
    }
}