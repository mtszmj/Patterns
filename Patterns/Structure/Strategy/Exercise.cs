using System;
using System.Numerics;

namespace Patterns.Structure.Strategy
{
    public interface IDiscriminantStrategy
    {
        double CalculateDiscriminant(double a, double b, double c);
    }

    public class OrdinaryDiscriminantStrategy : IDiscriminantStrategy
    {
        // todo
        public double CalculateDiscriminant(double a, double b, double c)
        {
            return b * b - (4 * a * c);
        }
    }

    public class RealDiscriminantStrategy : IDiscriminantStrategy
    {
        // todo (return NaN on negative discriminant!)
        public double CalculateDiscriminant(double a, double b, double c)
        {
            var x = b * b - (4 * a * c);
            return x < 0 ? double.NaN : x;
        }
    }

    public class QuadraticEquationSolver
    {
        private readonly IDiscriminantStrategy strategy;

        public QuadraticEquationSolver(IDiscriminantStrategy strategy)
        {
            this.strategy = strategy;
        }

        public Tuple<Complex, Complex> Solve(double a, double b, double c)
        {
            var discriminant = strategy.CalculateDiscriminant(a, b, c);
            if (discriminant == double.NaN)
                return new Tuple<Complex, Complex>(new Complex(double.NaN, 0), new Complex(double.NaN, 0));
            else
            {
                var c1 = (-b + System.Numerics.Complex.Sqrt(discriminant)) / (2 * a);
                var c2 = (-b - System.Numerics.Complex.Sqrt(discriminant)) / (2 * a);

                return new Tuple<Complex, Complex>(c1, c2);
            }
        }
    }
}
