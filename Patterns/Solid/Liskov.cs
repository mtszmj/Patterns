using System;

namespace Patterns.Solid
{
    public class Liskov
    {
        /* Liskov substitution principle.
         * Calling Square as Rectangle goes to methods / properties from square when
         * override is used. If 'New' is used it does not work correctly (goes to Rect.)
         */
        public void Test()
        {
            Rectangle rc = new Rectangle(2, 3);
            Console.WriteLine($"{rc} has area {Area(rc)}");

            Rectangle sq = new Square();
            sq.Width = 4;
            Console.WriteLine($"{sq} has area {Area(sq)}");
        }

        static public int Area(Rectangle r) => r.Width * r.Height;

        public class Rectangle
        {
            public virtual int Width { get; set; }
            public virtual int Height { get; set; }

            public Rectangle()
            {

            }

            public Rectangle(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public override string ToString()
            {
                return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
            }
        }

        public class Square : Rectangle
        {
            public override int Width
            {
                set { base.Width = base.Height = value; }
            }

            public override int Height
            {
                set { base.Width = base.Height = value; }
            }

        }

    }
}
