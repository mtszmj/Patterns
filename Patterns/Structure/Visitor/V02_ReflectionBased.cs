using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Visitor
{
    using DictType = Dictionary<Type, Action<V02_ReflectionBased.Expression, StringBuilder>>;

    class V02_ReflectionBased
    {
        public static void Test()
        {
            var e = new AdditionalExpression(
                new DoubleExpression(1),
                new AdditionalExpression(
                    new DoubleExpression(2),
                    new DoubleExpression(3)
                    ));
            var sb = new StringBuilder();
            ExpressionPrinter.Print(e, sb);
            Console.WriteLine(sb);

            sb = new StringBuilder();
            ExpressionPrinter.Print2(e, sb);
            Console.WriteLine(sb);
        }

        public static class ExpressionPrinter {
            // gdyby nie bylo print w hierarchi
            public static void Print(Expression e, StringBuilder sb)
            {

                // lamie to Open-Closed principle, bo po kazdym dodaniu Expression w hierarchi trzeba wrocic i zmieniac metode
                if(e is DoubleExpression de)
                {
                    sb.Append(de.value);
                } 
                else if(e is AdditionalExpression ae)
                {
                    sb.Append("(");
                    Print(ae.left, sb);
                    sb.Append("+");
                    Print(ae.right, sb);
                    sb.Append(")");
                }
            }

            private static DictType actions = new DictType
            {
                [typeof(DoubleExpression)] = (e, sb) =>
                {
                    var de = (DoubleExpression)e;
                    sb.Append(de.value);
                },
                [typeof(AdditionalExpression)] = (e, sb) =>
                {
                    var ae = (AdditionalExpression)e;
                    sb.Append("(");
                    Print2(ae.left, sb);
                    sb.Append("+");
                    Print2(ae.right, sb);
                    sb.Append(")");
                }
            };

            // ten przynajmniej wyrzuci wyjatek jesli typ sie nie zgodzi
            public static void Print2(Expression e, StringBuilder sb)
            {
                actions[e.GetType()](e, sb);
            }
        }

        public abstract class Expression
        {
            public abstract void Print(StringBuilder sb);
        }

        public class DoubleExpression : Expression
        {
            // zmienione na internal
            internal double value;

            public DoubleExpression(double value)
            {
                this.value = value;
            }

            public override void Print(StringBuilder sb)
            {
                sb.Append(value);
            }
        }

        public class AdditionalExpression : Expression
        {
            internal Expression left, right;

            public AdditionalExpression(Expression left, Expression right)
            {
                this.left = left ?? throw new ArgumentNullException(nameof(left));
                this.right = right ?? throw new ArgumentNullException(paramName: nameof(right));
            }

            public override void Print(StringBuilder sb)
            {
                sb.Append("(");
                left.Print(sb);
                sb.Append("+");
                right.Print(sb);
                sb.Append(")");

            }
        }
    }
}
