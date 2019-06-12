using System;

namespace Patterns.Creation
{
    public class FactoryMethod
    {
        public void Test()
        {
            var point = Point.NewPolarPoint(1.0, Math.PI / 2.0);
            Console.WriteLine(point);
        }
    }

    public class Point
    {
        // factory method pattern
        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
        // /factory method pattern

        private double x, y;

        private Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
        }
    }
}
