using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Visitor
{
    class Exercise
    {
        public static void Test()
        {
            var e = new AdditionExpression(
                new Value(1),
                new AdditionExpression(
                    new Value(2),
                    new Value(3)
                    ));

            var e2 = new MultiplicationExpression(
                new AdditionExpression(
                    e,
                    new Value(4)
                    ),
                new MultiplicationExpression(
                    new Value(5),
                    new Value(6)
                    )
               );

            var ep = new ExpressionPrinter();
            ep.Visit(e2);
            Console.WriteLine(ep);
        }
    }

    public abstract class ExpressionVisitor
    {
        public abstract void Visit(Expression ex);
        public abstract void Visit(Value value);
        public abstract void Visit(AdditionExpression ae);
        public abstract void Visit(MultiplicationExpression me);
    }

    public abstract class Expression
    {
        public abstract void Accept(ExpressionVisitor ev);
    }

    public class Value : Expression
    {
        public readonly int TheValue;

        public Value(int value)
        {
            TheValue = value;
        }

        // todo

        public override void Accept(ExpressionVisitor ev)
        {
            ev.Visit(this);
        }
    }

    public class AdditionExpression : Expression
    {
        public readonly Expression LHS, RHS;

        public AdditionExpression(Expression lhs, Expression rhs)
        {
            LHS = lhs;
            RHS = rhs;
        }

        // todo

        public override void Accept(ExpressionVisitor ev)
        {
            ev.Visit(this);
        }
    }

    public class MultiplicationExpression : Expression
    {
        public readonly Expression LHS, RHS;

        public MultiplicationExpression(Expression lhs, Expression rhs)
        {
            LHS = lhs;
            RHS = rhs;
        }

        // todo

        public override void Accept(ExpressionVisitor ev)
        {
            ev.Visit(this);
        }
    }

    public class ExpressionPrinter : ExpressionVisitor
    {
        internal StringBuilder sb = new StringBuilder();

        public override void Visit(Value value)
        {
            sb.Append(value.TheValue);
        }

        public override void Visit(AdditionExpression ae)
        {
            sb.Append("(");
            ae.LHS.Accept(this);
            sb.Append("+");
            ae.RHS.Accept(this);
            sb.Append(")");
        }

        public override void Visit(MultiplicationExpression me)
        {
            me.LHS.Accept(this);
            sb.Append("*");
            me.RHS.Accept(this);
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        public override void Visit(Expression ex)
        {
            throw new NotImplementedException();
        }
    }
}
