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
        public static List<string> Tokenize (string userInput) // ass: Patricia // this method receives user input (a string) and returns a List of strings. Contains the tokenize expression.
        {
            return new List<string>();
        }
        public static Stack<string> ToPostFix (List<string> infixExp) // ass: Max // this method receives a tokenized list of strings and converts it to a Stack of strings.
        {
            Console.WriteLine("Made a new branch called ToPostFix");
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
        }
    }
}
