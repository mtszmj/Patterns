using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Patterns.Bonus
{
    public static class ContinuationPassingStyle
    {
        public static void Test()
        {
            var solver = new QuadraticEquationSolver();
            var flag = solver.Start(1, 2, 3, out var result);
        }
    }

    public enum WorkflowResult
    {
        Success,
        Failure
    }

    public class QuadraticEquationSolver
    {
        public WorkflowResult Start(double a, double b, double c, out Tuple<Complex, Complex> result)
        {
            var disc = b * b - 4 * a * c;
            if (disc < 0)
            {
                //return SolveComplex(a, b, c, disc);
                result = null;
                return WorkflowResult.Failure;
            }
            else
            {
                return SolveSimple(a, b, c, disc, out result);
            }
        }

        //private WorkflowResult SolveComplex(double a, double b, double c, double disc, out Tuple<Complex, Complex> result)
        //{
        //    var rootDisc = Complex.Sqrt(new Complex(disc, 0));
        //    result =  Tuple.Create(
        //        (-b + rootDisc) / (2 * a),
        //        (-b - rootDisc) / (2 * a)
        //        );
        //    return WorkflowResult.Success;
        //}

        private WorkflowResult SolveSimple(double a, double b, double c, double disc, out Tuple<Complex, Complex> result)
        {
            var rootDisc = Math.Sqrt(disc);
            result = Tuple.Create(
                new Complex((-b + rootDisc) / (2 * a), 0),
                new Complex((-b - rootDisc) / (2 * a), 0)
                );
            return WorkflowResult.Success;
        }
    }
}
