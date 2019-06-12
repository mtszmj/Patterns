using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Creation.FactoryInner
{
    // in order to have private constructor (to not allow wrong object creation), 
    // PointFactory goes as inner class in Point
    class FactoryInner
    {
        public void Test()
        {
            var point = Point.Factory.NewPolarPoint(1.0, Math.PI / 2.0);
            Console.WriteLine(point);

            var point2 = Point.Factory2.NewPolarPoint(1.0, Math.PI / 2.0);
            Console.WriteLine(point2);

            // Problem - constructor is public
            // var point = new Point(x, y);
        }
    }

    public class Point
    {
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

        public static class Factory
        {
            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }

        // non static version:
        public static NonStaticFactory Factory2 => new NonStaticFactory();
        public class NonStaticFactory
        {
            public Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }
    }
}
