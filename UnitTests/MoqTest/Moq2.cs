using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.MoqTest
{
    public class Bar : IEquatable<Bar>
    {
        public string Name { get; set; }

        public bool Equals(Bar other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Bar)obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public static bool operator ==(Bar left, Bar right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Bar left, Bar right)
        {
            return Equals(left, right);
        }
    }

    public interface IBaz
    {
        string Name { get; }
    }

    public interface IFoo
    {
        bool DoSomething(string value);
        string ProcessString(string value);
        bool TryParse(string value, out string outputValue);
        bool Submit(ref Bar bar);
        int GetCount();
        bool Add(int amount);

        string Name { get; set; }
        IBaz SomeBaz { get; }
        int SomeOtherProperty { get; set; }
    }

    [TestFixture]
    public class MethodSamples
    {
        [Test]
        public void OrdinaryMethodCalls()
        {
            var mock = new Mock<IFoo>();

            mock.Setup(foo => foo.DoSomething("ping")).Returns(true);
            mock.Setup(foo => foo.DoSomething(It.IsIn("pong", "foo"))).Returns(false);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(mock.Object.DoSomething("ping"));
                Assert.IsFalse(mock.Object.DoSomething("pong"));
                Assert.IsFalse(mock.Object.DoSomething("foo"));
                Assert.IsFalse(mock.Object.DoSomething("ssfsdfsdfsdf")); // unspecified -> default()
            });
        }

        [Test]
        public void ArgumentDependentMatching()
        {
            var mock = new Mock<IFoo>();

            // dla wszystkich stringow
            mock.Setup(foo => foo.DoSomething(It.IsAny<string>()))
                .Returns(false);

            // z dodatkowym warunkiem - dla parzystych intow
            mock.Setup(foo => foo.Add(It.Is<int>(x => x % 2 == 0)))
                .Returns(true);

            // zakres
            mock.Setup(foo => foo.Add(It.IsInRange<int>(1, 10, Range.Inclusive)))
                .Returns(false);

            // regex
            mock.Setup(foo => foo.DoSomething(It.IsRegex("[a-z]+")))
                .Returns(false);

            mock.Object.DoSomething("abc");

        }

        [Test]
        public void OutAndRefArguments()
        {
            var mock = new Mock<IFoo>();
            var requiredOutput = "ok";

            mock.Setup(foo => foo.TryParse("ping", out requiredOutput))
                .Returns(true);

            string result;
            Assert.Multiple(() =>
            {
                Assert.IsTrue(mock.Object.TryParse("ping", out result));
                Assert.That(result, Is.EqualTo(requiredOutput));

                var thisShouldBeFalse = mock.Object.TryParse("pong", out result);
                Console.WriteLine(thisShouldBeFalse);
                Console.WriteLine(result);
            });


            var bar = new Bar() { Name = "abc" };
            mock.Setup(foo => foo.Submit(ref bar)).Returns(true);

            Assert.That(mock.Object.Submit(ref bar), Is.EqualTo(true));

            var someOtherBar = new Bar() { Name = "abc" };
            Assert.IsFalse(mock.Object.Submit(ref someOtherBar)); // mock zawsze uzywa ReferenceEquals - nawet przy interfejsie IEquatable<> (nic jego implementacja nie da)

        }

        [Test]
        public void ReturnValues()
        {
            var mock = new Mock<IFoo>();
            mock.Setup(foo => foo.ProcessString(It.IsAny<string>()))
                .Returns((string s) => s.ToLowerInvariant());

            Assert.Multiple(() =>
            {
                Assert.That(mock.Object.ProcessString("ABC"), Is.EqualTo("abc"));
            });
        }

        [Test]
        public void DifferentReturnValues()
        {
            var mock = new Mock<IFoo>();
            var calls = 0;
            mock.Setup(foo => foo.GetCount())
                .Returns(() => calls)
                .Callback(() => ++calls); // wywolane po wykonaniu metody GetCount()

            mock.Object.GetCount();
            mock.Object.GetCount(); 

            Assert.Multiple(() =>
            {
                Assert.That(mock.Object.GetCount(), Is.EqualTo(2));
            });
        }

        [Test]
        public void ExceptionsTests()
        {
            var mock = new Mock<IFoo>();

            mock.Setup(foo => foo.DoSomething("kill"))
                .Throws<InvalidOperationException>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                mock.Object.DoSomething("kill");
            });
        }

        [Test]
        public void ExceptionsTests2()
        {
            var mock = new Mock<IFoo>();

            mock.Setup(foo => foo.DoSomething("null"))
                .Throws(new ArgumentException("cmd"));

            Assert.Throws<ArgumentException>(() =>
            {
                mock.Object.DoSomething("null");
            }, "cmd");
        }

        [Test]
        public void PropertiesTest()
        {
            var mock = new Mock<IFoo>();

            mock.Setup(foo => foo.Name)
                .Returns("bar");

            mock.Object.Name = "this will not be assigned"; // nie zadziala w ten sposob

            Assert.That(mock.Object.Name, Is.EqualTo("bar"));

            // zagniezdzone properties
            mock.Setup(foo => foo.SomeBaz.Name).Returns("hello");
            Assert.That(mock.Object.SomeBaz.Name, Is.EqualTo("hello"));

            // setter

            bool setterCalled = false;
            mock.SetupSet(foo =>
            {
                foo.Name = It.IsAny<string>();
            }).Callback<string>(value =>
            {
                setterCalled = true;
            });

            mock.Object.Name = "def"; // tutaj setowanie juz zadzial bo zdefiniowany jest setter
            mock.VerifySet(foo =>   // to sprawdza jedynie wywolanie settera ale nie zmienia wartosci foo.Name
            {
                foo.Name = "def";
            }, Times.AtLeastOnce());
            Console.WriteLine(mock.Object.Name); // "bar"
        }

        [Test]
        public void PropertiesWithSetter()
        {
            var mock = new Mock<IFoo>();

            //mock.SetupAllProperties();
            mock.SetupProperty(f => f.Name);

            IFoo foo = mock.Object;
            foo.Name = "abc";

            Assert.That(mock.Object.Name, Is.EqualTo("abc"));

            foo.Name = "abcd";
            Assert.That(mock.Object.Name, Is.EqualTo("abcd"));

            mock.SetupAllProperties();
            foo.SomeOtherProperty = 123;
            Assert.That(mock.Object.SomeOtherProperty, Is.EqualTo(123));
        }
    }
}
