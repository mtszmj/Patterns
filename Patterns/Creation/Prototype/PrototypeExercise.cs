using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Creation.Prototype.Exercise
{
    public static class PrototypeExercise
    {
        public static void Test()
        {
            var line = new Line {
                Start = new Point { X = 1, Y = 2 },
                End = new Point { X = 3, Y = 4 }
            };

            var line2 = line.DeepCopy();
            line2.Start.X = 8;
            line2.End.Y = 7;

            Console.WriteLine(line);
            Console.WriteLine(line2);
        }
    }

    public class Point
    {
        public int X, Y;

        public Point DeepCopy()
        {
            return new Point { X = this.X, Y = this.Y };
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }
    }

    public class Line
    {
        public Point Start, End;

        public Line DeepCopy()
        {
            return new Line
            {
                Start = this.Start.DeepCopy(),
                End = this.End.DeepCopy()
            };
        }

        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}";
        }
    }
}
