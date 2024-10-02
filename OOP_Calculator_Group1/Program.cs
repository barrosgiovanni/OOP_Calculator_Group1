using System;
using System.CodeDom;
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
        public static bool IsOperator(char op) // method overloading to accept both char and string
        {
            return false;
        }
       public static bool IsOperator(string op) // method overloading to accept both char and string
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
        public static Stack<string> ToPostFix(List<string> infixExp) // ass: Max Dyson // this method receives a tokenized list of strings and converts it to a Stack of strings.
        {

            /*
               Here we use the shunting yard algorithm to build a postfix expression:
               example:
                   infix     postfix
                   1 + 2  -->   12+
                   1+2*3  -->   123*+
               Hint: We use the ExpressionProcessor.Outputlist and OperatorStack to build the correct postfixExp for each infix expression
            */



            OutputList = new List<string>();
            OperatorStack = new Stack<Operator>();

        
       
            foreach(string tokens in infixExp) {

                // Code inside
                
                // Checks if the tokenized list of strings contains any numbers

                if (float.TryParse(tokens, out float number))
                {

                    //  Adds only the numbers from the tokenized list of strings to the OutputList
                  
                    OutputList.Add(tokens);

                }


                // Checks if the tokenized list of strings has any operators


                if (tokens.Contains("+") || tokens.Contains("-") || tokens.Contains("*") ||
                    tokens.Contains("/") || tokens.Contains("(") || tokens.Contains(")"))
                {

                    Operator op = GetOperator(tokens);


                    if (op != null)
                    {

                        // While there is an operator at the top of the stack (Peek()) with greater or equal precedence
                        // pop it from the stack to the OutputList.

                        while (OperatorStack.Count > 0 && OperatorStack.Peek().Precedence >= op.Precedence)
                        {

                            // ToString converts a char symbol into a string 

                            OutputList.Add(OperatorStack.Pop().Symbol.ToString());



                        }


                        // Push the current operator onto the OperatorStack
                       
                        OperatorStack.Push(op);

                    }



                    // Push the left parenthesis onto the OperatorStack

                    if (tokens == "(")
                    {


                        Operator lp = new LeftParenthesis();
                        OperatorStack.Push(lp);

                    }


                 
               


                    if (tokens == ")")
                    {


                        // Pop the right parenthesis onto the OutputList until a left parenthesis is at the top of the OperatorStack ((Peek())).

                        while (OperatorStack.Count > 0 && OperatorStack.Peek().Symbol.ToString() != "(") 
                        {

                          OutputList.Add(OperatorStack.Pop().Symbol.ToString());


                        }


                        // Remove the left parenthesis from the stack but do not add it to the OutputList

                        if (OperatorStack.Count > 0 && OperatorStack.Pop().Symbol.ToString() == "(")
                        {

                            OperatorStack.Pop().Symbol.ToString();

                           
                        }

                       
                    }
               
             }

               
         }


         
        return new Stack<string>(OutputList); 
    }
      
       
        
        // Return a Operator object
        public static Operator GetOperator(string tokens)
        {

            switch (tokens){

                case "+":

                    return new Addition();

                case "-":

                    return new Subtraction();

                case "*":

                    return new Multiplication();


                case "/":

                    return new Division();

                

                default:

                    return null;

            }


        }


        public static string ExecutePostFix (Stack<string> postfixExp) // ass: Giovanni // this method receives a Stack of strings and return a postfix expression (string type).
        {

            return "";
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
