using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Creation
{
    public class FactoryPointWrong
    {
        public void Test()
        {

        }
    }

    public enum CoordinateSystem
    {
        Cartesian,
        Polar
    }

    public class WrongPoint
    {
        private double x, y;

        // Introduces misunderstanding - do not know if 'a' is 'x' or 'y' and so on.
        /// <summary>
        /// Initialize a point from either cartesian or polar
        /// </summary>
        /// <param name="a">x if cartesian, rho if polar</param>
        /// <param name="b"></param>
        /// <param name="system"></param>
        public WrongPoint(double a, double b,
            CoordinateSystem system = CoordinateSystem.Cartesian)
        {
            switch (system)
            {
                case CoordinateSystem.Cartesian:
                    x = a;
                    y = b;
                    break;
                case CoordinateSystem.Polar:
                    x = a * Math.Cos(b);
                    y = a * Math.Sin(b);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(system), system, null);
            }
        }
    }
}
