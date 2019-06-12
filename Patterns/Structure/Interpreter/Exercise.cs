using System;
using System.Collections.Generic;
using System.Text;

namespace Patterns.Structure.Interpreter.Exercise
{
    class Exercise
    {


        public static void Test()
        {
            var proc = new ExpressionProcessor();
            proc.Variables.Add('x', 3);

            Console.WriteLine(proc.Calculate("1+2+3"));
            Console.WriteLine(proc.Calculate("1+2+xy"));
            Console.WriteLine(proc.Calculate("10-2-x"));

        }
    }

    public class ExpressionProcessor
    {
        public Dictionary<char, int> Variables = new Dictionary<char, int>();



        public int Calculate(string expression)
        {
            var list = new List<Token>();

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '+')
                {
                    list.Add(new Token { Operation = Token.Op.Plus });
                }
                else if (expression[i] == '-')
                {
                    list.Add(new Token { Operation = Token.Op.Minus });
                }
                else
                {
                    var sb = new StringBuilder();
                    if (char.IsDigit(expression[i]))
                    //if (int.TryParse(expression[i].ToString(), out int v))
                    {
                        sb.Append(expression[i]);
                        for (var j = i + 1; j < expression.Length; j++)
                        {
                            if (char.IsDigit(expression[j]))
                            {
                                sb.Append(expression[j]);
                                i = j;
                            }
                            else break;
                        }

                        list.Add(new Token { Operation = Token.Op.Value, Value = int.Parse(sb.ToString()) });
                    }
                    if (char.IsLetter(expression[i]))
                    {
                        bool wrong = false;
                        for (var j = i + 1; j < expression.Length; j++)
                        {
                            if (char.IsLetter(expression[j]))
                            {
                                wrong = true;
                            }
                            else
                            {
                                i = j;
                                break;
                            }

                        }
                        if (wrong) return 0;
                        else
                            list.Add(new Token()
                            {
                                Operation = Token.Op.Var,
                                Value = Variables[expression[i]]
                            });
                    }
                }
            }

            var multiplier = 1;
            var result = 0;
            foreach (var element in list)
            {
                switch (element.Operation)
                {
                    case Token.Op.Plus:
                        multiplier = 1;
                        break;
                    case Token.Op.Minus:
                        multiplier = -1;
                        break;
                    case Token.Op.Value:
                    case Token.Op.Var:
                        result += multiplier * element.Value;
                        break;
                }
            }

            return result;
        }
    }

    public class Token
    {
        public enum Op
        {
            Plus, Minus, Value, Var
        }

        public int Value;
        public Op Operation;
    }
}
