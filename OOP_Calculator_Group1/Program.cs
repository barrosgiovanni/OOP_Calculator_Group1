using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
    class LeftParenthesis : Operator
    {
        public LeftParenthesis()
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
            //Example:
            /*
                user input: 1+ 2 - (-3 *4.15)
                tokenized expression: { "1", "+", "2", "-", "(", "-3", "*", "4.15", ")" }
             */
            return new List<string>();
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
            return new Stack<string>(); 
        }
        public static string ExecutePostFix (Stack<string> postfixExp)
        {
            // this method receives a strings Stack (postfix exp) and returns the result(string type).
            List<string> temp = new List<string>(); // list created to hold values as we check for operator.
            int opIndex = 0; // will hold the index of the operator sign.
            bool opDetected = false;
            while (postfixExp.Count + temp.Count > 1)
            {
                if (postfixExp.Count > 0) // as long as we see elements inside the stack, we'll be popping the last item and adding to the temporary list.
                {
                    temp.Add(postfixExp.Pop()); 
                }
                for (int index = 0; index < temp.Count - 2; index++)
                {
                    if ((temp[index] == "+" || temp[index] == "*" || temp[index] == "-" || temp[index] == "/") && float.TryParse(temp[index + 1], out float value1) && float.TryParse(temp[index + 2], out float value2))
                    // if we find the pattern of a operator and 2 numbers in sequence.
                    {
                        opIndex = index;
                        opDetected = true;
                        break;
                    }
                    continue;
                }
                if (temp.Count > 2 && opDetected) // as long as we have 3 elements in the temporary list.
                {
                    Operator op = null;
                    float current = 0; // variable to hold the accumulated result. It will then be transfered to the temporary list later.
                    if (temp[opIndex] == "+")
                    {
                        op = new Addition(); // executing the operation according to the operator.
                        current = op.Execute(float.Parse(temp[opIndex + 2]), float.Parse(temp[opIndex + 1]));
                        temp.RemoveRange(opIndex, 3); // after we complete the operation, we must remove the current values inside the temporary list.
                        temp.Add(current.ToString()); // transfering result to the temporary list.
                    }
                    if (temp[opIndex] == "-")
                    {
                        op = new Subtraction();
                        current = op.Execute(float.Parse(temp[opIndex + 2]), float.Parse(temp[opIndex + 1]));
                        temp.RemoveRange(opIndex, 3);
                        temp.Add(current.ToString());
                    }
                    if (temp[opIndex] == "*")
                    {
                        op = new Multiplication();
                        current = op.Execute(float.Parse(temp[opIndex + 2]), float.Parse(temp[opIndex + 1]));
                        temp.RemoveRange(opIndex, 3);
                        temp.Add(current.ToString());
                    }
                    if (temp[opIndex] == "*")
                    {
                        op = new Division();
                        current = op.Execute(float.Parse(temp[opIndex + 2]), float.Parse(temp[opIndex + 1])); 
                        temp.RemoveRange(opIndex, 3);
                        temp.Add(current.ToString());
                    }
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
            List<string> tokenizedExp = ExpressionProcessor.Tokenize(userInput);
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
