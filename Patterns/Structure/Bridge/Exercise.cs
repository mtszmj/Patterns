using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Bridge.Exercise
{
    public class Exercise
    {
        public static void Test()
        {

        }
    }

    public abstract class Shape
    {
        public string Name { get; set; }
        protected IRenderer renderer;

        public Shape(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public override string ToString() => $"Drawing {Name} as {renderer.WhatToRenderAs}";
    }

    public class Triangle : Shape
    {
        public Triangle(IRenderer renderer) : base(renderer)
        {
            Name = nameof(Triangle);
        }
    }

    public class Square : Shape
    {
        public Square(IRenderer renderer) : base(renderer)
        {
            Name = nameof(Square);
        }
    }

    public interface IRenderer
    {
        string WhatToRenderAs { get; }
    }

    public class VectorRenderer : IRenderer
    {
        public string WhatToRenderAs => "lines";
    }

    public class RasterRenderer : IRenderer
    {
        public string WhatToRenderAs => "pixels";
    }

    [TestFixture]
    public class Tests
    {
        [Test]
        public void TriangleVectorTest()
        {
            var renderer = new VectorRenderer();
            var shape = new Triangle(renderer);

            Assert.That(shape.ToString(), Is.EqualTo("Drawing Triangle as lines"));
        }

        [Test]
        public void TriangleRasterTest()
        {
            var renderer = new RasterRenderer();
            var shape = new Triangle(renderer);

            Assert.That(shape.ToString(), Is.EqualTo("Drawing Triangle as pixels"));
        }

        [Test]
        public void SquareVectorTest()
        {
            var renderer = new VectorRenderer();
            var shape = new Square(renderer);

            Assert.That(shape.ToString(), Is.EqualTo("Drawing Square as lines"));
        }

        [Test]
        public void SquareRasterTest()
        {
            var renderer = new RasterRenderer();
            var shape = new Square(renderer);

            Assert.That(shape.ToString(), Is.EqualTo("Drawing Square as pixels"));
        }
    }
}
