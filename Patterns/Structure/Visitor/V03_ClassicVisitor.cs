using System;
using System.Text;

namespace Patterns.Structure.Visitor
{
    class V03_ClassicVisitor
    {
        public static void Test()
        {
            var e = new AdditionalExpression(
                new DoubleExpression(1),
                new AdditionalExpression(
                    new DoubleExpression(2),
                    new DoubleExpression(3)
                    ));
            var ep = new ExpressionPrinter();
            ep.Visit(e);
            Console.WriteLine(ep);

            var ec = new ExpressionCalculator();
            ec.Visit(e);
            Console.WriteLine(ec.Result);
        }

        // another type of visitor:
        public class ExpressionCalculator : IExpressionVisitor
        {
            public double Result;

            public void Visit(DoubleExpression de)
            {
                Result = de.value;
            }

            public void Visit(AdditionalExpression ae)
            {
                ae.left.Accept(this);
                var a = Result;
                ae.right.Accept(this);
                var b = Result;
                Result = a + b;
            }
        }

        public class ExpressionPrinter : IExpressionVisitor
        {
            StringBuilder sb = new StringBuilder();

            public void Visit(DoubleExpression de)
            {
                sb.Append(de.value);
            }

            public void Visit(AdditionalExpression ae)
            {
                sb.Append("(");
                ae.left.Accept(this);
                sb.Append("+");
                ae.right.Accept(this);
                sb.Append(")");
            }

            public override string ToString()
            {
                return sb.ToString();
            }
        }

        public interface IExpressionVisitor
        {
            void Visit(DoubleExpression de);
            void Visit(AdditionalExpression ae);
        }

        public abstract class Expression
        {
            public abstract void Accept(IExpressionVisitor visitor);
        }

        public class DoubleExpression : Expression
        {
            // zmienione na internal
            internal double value;

            public DoubleExpression(double value)
            {
                this.value = value;
            }

            public override void Accept(IExpressionVisitor visitor)
            {
                visitor.Visit(this);
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

            public override void Accept(IExpressionVisitor visitor)
            {
                visitor.Visit(this);
            }
        }
    }
}
