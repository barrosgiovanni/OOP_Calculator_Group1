using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

// Assignment 1 - Calculator
// Group 1: Daniel Benjumea, Giovanni Barros, Max Dyson, Patricia Magalhaes, Vagner Emmanuel da Cruz

namespace OOP_Calculator_Group1
{
    abstract class Operator
    {
        // properties
        public char Symbol { get; set; }
        public int Precedence { get; set; } // this property will be used to precedence of operations
        public bool isLeftAssociative { get; set; } = true; // we're not using rn because we're not doing exp and srt

        public abstract float Execute(float operand1 = 0, float operand2 = 0);
        private static bool IsOperator(char op) // method overloading to accept both char and string
        {
            return false;
        }
        private static bool IsOperator(string op) // method overloading to accept both char and string
        {
            return false;
        }
    }

    class Addition : Operator
    {
        Addition()
        {
            Symbol = '+';
            Precedence = 1;
        }
        public override float Execute(float operand1 = 0, float operand2 = 0)
        {
            return operand1 + operand2;
        }
    }
    class Subtraction : Operator
    {
        Subtraction()
        {
            Symbol = '-';
            Precedence = 1;
        }
        public override float Execute(float operand1 = 0, float operand2 = 0)
        {
            return operand1 - operand2;
        }
    }
    class Multiplication : Operator
    {
        Multiplication()
        {
            Symbol = '*';
            Precedence = 2;
        }
        public override float Execute(float operand1 = 0, float operand2 = 0)
        {
            return operand1 * operand2;
        }
    }
    class Division : Operator
    {
        Division()
        {
            Symbol = '/';
            Precedence = 2;
        }
        public override float Execute(float operand1 = 0, float operand2 = 0)
        {
            return operand1 / operand2;
        }
    }
    class LeftParenthesis : Operator
    {
        LeftParenthesis()
        {
            Symbol = '(';
            Precedence = 3;
        }
        public override float Execute(float operand1 = 0, float operand2 = 0)
        {
            throw new Exception("It is not doing anything.");
        }
    }

    static class ExpressionProcessor 
    {   
        // properties
        public static List<string> OutputList { get; set; } 
        public static Stack<Operator> OperatorStack { get; set; }
        // methods

        public static List<string> Tokenize(string userInput) // ass: Patricia Diniz // this method receives user input (a string) and returns a List of strings. Contains the tokenize expression.
        {
            List<string> tokens = new List<string>(); // List to store tokens
            string currentNumber = ""; //string to accumulate current number
            bool isOperator = false; //verify operators to handling errors - 10/01/24
            bool isNumber = false; // verify numbers to handling errors - 10/01/24

            try
            {
                bool isNegative = false; // Flag to check if the number is negative
                foreach (char c in userInput)
                {
                    if (char.IsDigit(c) || c == '.') // Verify if char is a digit (number) or a "." --> work with decimal number
                    {
                        if (isNegative)
                        {
                            currentNumber = "-" + currentNumber; // Prepend the negative sign - 10/01/24
                            isNegative = false; // Reset the flag
                        }
                        currentNumber += c; // Accumulate digit which forms a number including "."
                        isNumber = true;
                    }
                    else if ("+-*/()".Contains(c)) // Verify if char is an operator
                    {
                        if (!string.IsNullOrEmpty(currentNumber))
                        {
                            tokens.Add(currentNumber); // Add accumulated number to list
                            currentNumber = "";
                        }
                        if (c == '-' && (tokens.Count == 0 || "+-*/(".Contains(tokens.Last()))) // Check if '-' is a negative sign
                        {
                            isNegative = true; // Set the flag for negative number
                        }
                        //else
                        //{
                        //    tokens.Add(c.ToString()); // Add operator to list
                        //    isOperator = true;
                        //}
                        else // include a multiplication signal before parentheses
                        {
                            if (c == '(' && tokens.Count > 0 && (char.IsDigit(tokens.Last().Last()) || tokens.Last().Last() == ')'))                        
                            {
                                tokens.Add("*"); // Add multiplication sign before '(' if needed
                            }
                            tokens.Add(c.ToString()); // Add operator to list
                            isOperator = true;
                        }
                    }
                }

                // There is an accumulated number at the end, add it to list
                if (!string.IsNullOrEmpty(currentNumber))
                {
                    tokens.Add(currentNumber);
                }

                if (!isOperator)
                {
                    throw new ArgumentException("The string must contain at least one operator");
                }
                if (!isNumber)
                {
                    throw new ArgumentException("The string must contain at least one number");
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            return tokens; // Return list of tokens        
        }



        public static Stack<string> ToPostFix (List<string> infixExp) // ass: Max // this method receives a tokenized list of strings and converts it to a Stack of strings.
        {
            return new Stack<string>(); 
        }
        public static string ExecutePostFix (Stack<string> postfixExp) // ass: Giovanni // this method receives a Stack of strings and return a postfix expression (string type).
        {
            return "";
        }

    }
    internal class Program
    {        
       static void Main(string[] args) // ass: Daniel 
        {
            //This code was written by Patricia to test method Tokenize
            Console.WriteLine("Type a mathematic expression to calculate:");
            string userInput = Console.ReadLine();

            List<string> tokens = ExpressionProcessor.Tokenize(userInput); // shows each char
 

            Console.WriteLine("These are the tokens:");
            foreach (var token in tokens)
            {
                Console.WriteLine(token); // Print on screen each char added in the expression
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(); 
        }
    }
}
