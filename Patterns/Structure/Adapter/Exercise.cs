using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Adapter
{
    class Exercise
    {
    }

    public class Square
    {
        public int Side;
    }

    public interface IRectangle
    {
        int Width { get; }
        int Height { get; }
    }

    public static class ExtensionMethods
    {
        public static int Area(this IRectangle rc)
        {
            return rc.Width * rc.Height;
        }
    }

    public class SquareToRectangleAdapter : IRectangle
    {
        private Square _Square;

        public SquareToRectangleAdapter(Square square)
        {
            _Square = square;
        }
        public int Width => _Square.Side;
        public int Height => Width;
    }
}
