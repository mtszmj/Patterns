using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Visitor
{
    class V01_Instusive
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
            e.Print(sb);
            Console.WriteLine(sb);
        }

        public abstract class Expression
        {
            public abstract void Print(StringBuilder sb);
        }

        public class DoubleExpression : Expression
        {
            private double value;

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
            Expression left, right;

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
