using System;
using System.Text;

namespace Patterns.Structure.Visitor
{
    class V04_DynamicVisitorDLR
    {
        public static void Test()
        {
            Expression e = new AdditionalExpression(
                new DoubleExpression(1),
                new AdditionalExpression(
                    new DoubleExpression(2),
                    new DoubleExpression(3)
                    ));

            var ep = new ExpressionPrinter();
            var sb = new StringBuilder();
            ep.Print((dynamic)e, sb);
            Console.WriteLine(sb);
        }


        public class ExpressionPrinter
        {
            public void Print(AdditionalExpression ae, StringBuilder sb)
            {
                sb.Append("(");
                Print((dynamic)ae.left, sb);
                sb.Append("+");
                Print((dynamic)ae.right, sb);
                sb.Append(")");
            }

            public void Print(DoubleExpression de, StringBuilder sb)
            {
                sb.Append(de.value);
            }
        }

        public abstract class Expression
        {
        }

        public class DoubleExpression : Expression
        {
            // zmienione na internal
            internal double value;

            public DoubleExpression(double value)
            {
                this.value = value;
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
        }
    }
}
