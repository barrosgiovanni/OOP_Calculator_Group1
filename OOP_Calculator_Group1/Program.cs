using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// Assignment 1 - Calculator
// Group 1: Daniel Benjumea, Giovanni Barros, Max Dyson, Patricia Magalhaes.

namespace OOP_Calculator_Group1
{
    abstract class Operator
    {
        // properties
        public char Symbol { get; set; }
        public int Precedence { get; set; } // this property will be used to precedence of operations
        public bool isLeftAssociative { get; set; } = true; // we're not using rn because we're not doing exp and srt

        public abstract float Execute(float operand1 = 0, float operand2 = 0);
        public static bool IsOperator(char op) // method overloading to accept both char and string
        {
            if (op == '+' || op == '-' || op == '*' || op == '/')
                return true;

            return false;
        }
        public static bool IsOperator(string op) // method overloading to accept both char and string
        {
            if (op == "+" || op == "-" || op == "*" || op == "/")
                return true;

            return false;
        }
    }

    class Addition : Operator
    {
        public Addition()
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
        public Subtraction()
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
        public Multiplication()
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
        public Division() 
        {
            Symbol = '/';
            Precedence = 2;
        }
        public override float Execute(float operand1 = 0, float operand2 = 0)
        {
            return operand1 / operand2;
        }
    }
    class left_parentheses : Operator
    {
        public left_parentheses()
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
        public static List<string> OutputList { get; set; } = new List<string>();
        public static Stack<Operator> OperatorStack { get; set; } = new Stack<Operator>();

        // methods
        private static Stack<string> _convertListToStack(List<string> list)
        {
            Stack<string> stack = new Stack<string>();
            for (int j = 0; j < list.Count; j++) stack.Push(list[j]);
            return stack;
        }

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
            /*
                Here we use the shunting yard algorithm to build a postfix expression:
                example:
                    infix     postfix
                    1 + 2  -->   12+
                    1+2*3  -->   123*+
                Hint: We use the ExpressionProcessor.Outputlist and OperatorStack to build the correct postfixExp for each infix expression
             */
            Operator addition = new Addition();
            Operator subtraction = new Subtraction();
            Operator multiplication = new Multiplication();
            Operator division = new Division();
            Operator left_parentheses = new left_parentheses();

            void StackByPrecedence(string op)
            {
                Operator operador = null;

                if (op == "+") operador = addition;
                else if (op == "-") operador = subtraction;
                else if (op == "*") operador = multiplication;
                else if (op == "/") operador = division;

                if (OperatorStack.Count == 0 || OperatorStack.Peek() == left_parentheses)
                {
                    OperatorStack.Push(operador);
                }
                else
                {
                    if (operador.Precedence > OperatorStack.Peek().Precedence)
                        OperatorStack.Push(operador);

                    else if (operador.Precedence <= OperatorStack.Peek().Precedence)
                    {
                        while (OperatorStack.Count > 0 && OperatorStack.Peek() != left_parentheses && OperatorStack.Peek().Precedence >= operador.Precedence)
                            OutputList.Add(OperatorStack.Pop().Symbol + "");

                        OperatorStack.Push(operador);
                    }
                }
            }

            infixExp.ForEach((num) =>
            {
                if (float.TryParse(num, out float numero))
                    OutputList.Add(numero.ToString());
                else if (num == "(")
                    OperatorStack.Push(left_parentheses);
                else if (Operator.IsOperator(num))
                    StackByPrecedence(num);
                else if (num == ")")
                {
                    string op;
                    while ((op = OperatorStack.Pop().Symbol + "") != "(")
                        OutputList.Add(op);
                }
            });

            while (OperatorStack.Count > 0)
                OutputList.Add(OperatorStack.Pop().Symbol + "");

            //return _convertListToStack(OutputList);
            Stack<string> postfixStack = _convertListToStack(OutputList);
            OutputList.Clear();
            return postfixStack;
        }
        private static bool isOperationDetected(List<string> list, ref int index) // 1 2 + 3 * -->  * 3 + 2 1  --> * 3
        {
            bool isOperationDetected = false;
            for (int j = 0; j < list.Count - 2; j++)
            {
                if (Operator.IsOperator(list[j]) && float.TryParse(list[j + 1], out float num1) && float.TryParse(list[j + 2], out float num2))
                {
                    index = j;
                    isOperationDetected = true;
                    break;
                }
                continue;
            }
            return isOperationDetected;
        }

        private static float ExecuteOperator(List<string> temp, int index)
        {
            Operator op = null;
            float current = 0; // variable to hold the accumulated result. It will then be transfered to the temporary list later.
            if (temp[index] == "+") // checking for each one of the operators and executing operation according to.
            {
                op = new Addition(); 
                current = op.Execute(float.Parse(temp[index + 2]), float.Parse(temp[index + 1]));
                temp.RemoveRange(index, 3);
                temp.Add(current.ToString());
            }
            else if (temp[index] == "-")
            {
                op = new Subtraction();
                current = op.Execute(float.Parse(temp[index + 2]), float.Parse(temp[index + 1]));
                temp.RemoveRange(index, 3);
                temp.Add(current.ToString());
            }
            else if (temp[index] == "*")
            {
                op = new Multiplication();
                current = op.Execute(float.Parse(temp[index + 2]), float.Parse(temp[index + 1]));
                temp.RemoveRange(index, 3);
                temp.Add(current.ToString());
            }
            else if (temp[index] == "/")
            {
                op = new Division();
                current = op.Execute(float.Parse(temp[index + 2]), float.Parse(temp[index + 1]));
                temp.RemoveRange(index, 3);
                temp.Add(current.ToString());
            }
            return current;
        }

        public static string ExecutePostFix (Stack<string> postfixExp) // this method receives a strings Stack (postfix exp) and returns the result(string type).
        {
            List<string> temp = new List<string>(); // list created to hold values as we check for operator.
            int opIndex = 0; // will hold the index of the operator sign.
            bool isOpDetected = false;
            while (postfixExp.Count + temp.Count > 1)
            {
                if (postfixExp.Count > 0) // as long as we see elements inside the stack, we'll be popping the last item and adding to the temporary list.
                {
                    temp.Add(postfixExp.Pop()); 
                }
                isOpDetected = isOperationDetected(temp, ref opIndex);
                if (temp.Count > 2 && isOpDetected) // as long as we have 3 elements in the temporary list.
                {
                    ExecuteOperator(temp, opIndex);
                    for (int index = temp.Count - 1; index >= 0; index--) 
                    {
                        postfixExp.Push(temp[index]); // transfering final result to postfixExp variable.
                    }
                    temp.Clear(); // cleaning the temporary list.
                }
            }
            return postfixExp.Pop();
        }
    }
    internal class Program
    {    
        //Encapsulation Coupling Method.
        static void Calculate(string userInput)
        {
            List<string> tokenizedExp =  ExpressionProcessor.Tokenize(userInput); // Working fine.
            Stack<string> postfixExp = ExpressionProcessor.ToPostFix(tokenizedExp);
            Console.WriteLine($"Result: {ExpressionProcessor.ExecutePostFix(postfixExp)}");
        }

        static void Main(string[] args) // ass: Daniel
        {
            /*
             As discussed in class... Feel free to play in here to test your methods.
             Just remember to clear your changes before pushing, and to keep the methods execution chain as it is once you're done testing
             please and thank you.
             */
            Console.WriteLine("-- Calculator --");
            Console.WriteLine("Enter your expression:");
            string mathExpression = "";
            while (mathExpression.ToLower().Trim() != "exit")
            {
                mathExpression = Console.ReadLine();
                Calculate(mathExpression);
            }
        }
    }
}
