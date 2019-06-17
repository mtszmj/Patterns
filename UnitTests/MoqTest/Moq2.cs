using Moq;
using Moq.Protected;
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

    public class Consumer
    {
        private IFoo foo;

        public Consumer(IFoo foo)
        {
            this.foo = foo;
        }

        public void Hello()
        {
            foo.DoSomething("ping");
            var name = foo.Name;
            foo.SomeOtherProperty = 123;
        }
    }

    public delegate void AlienAbductionEventHandler(int galaxy, bool returned);

    public interface IAnimal
    {
        event EventHandler FallsIll;

        void Stumble();

        event AlienAbductionEventHandler AbductedByAliens;
    }

    public class Doctor
    {
        public int TimesCured;
        public int AbductionObserved;

        public Doctor(IAnimal animal)
        {
            animal.FallsIll += (sender, args) =>
            {
                Console.WriteLine("I will cure you!");
                TimesCured++;
            };

            animal.AbductedByAliens += (galaxy, returned) => ++AbductionObserved;
        }
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

        [Test]
        public void EventTest()
        {
            var mock = new Mock<IAnimal>();
            var doctor = new Doctor(mock.Object);

            mock.Raise(
                a => a.FallsIll += null, // wskaz event dla ktorego trzeba mockowac
                new EventArgs()
                );

            Assert.That(doctor.TimesCured, Is.EqualTo(1));

            mock.Setup(a => a.Stumble()) // wiemy, ze kiedy sie potknie staje sie chore
                .Raises(a => a.FallsIll += null,
                new EventArgs());

            mock.Object.Stumble();
            Assert.That(doctor.TimesCured, Is.EqualTo(2));

            mock.Raise(a => a.AbductedByAliens += null,
                42, true);
            Assert.That(doctor.AbductionObserved, Is.EqualTo(1));
        }

        [Test]
        public void CallbackTest()
        {
            var mock = new Mock<IFoo>();

            int x = 0;
            mock.Setup(foo => foo.DoSomething("ping"))
                .Returns(true)
                .Callback(() => x++);   // wykonaj po kazdym wykonaniu

            mock.Object.DoSomething("ping");
            Assert.That(x, Is.EqualTo(1));

            // wykorzystaj argument metody w callbacku
            mock.Setup(foo => foo.DoSomething(It.IsAny<string>()))
                .Returns(true)
                .Callback((string s) => x += s.Length);

            // generycznie
            mock.Setup(foo => foo.DoSomething(It.IsAny<string>()))
                .Returns(true)
                .Callback<string>(s => x += s.Length);

            // callback przed wywolaniem metody
            mock.Setup(foo => foo.DoSomething("pong"))
                .Callback(() => Console.WriteLine("before pong"))
                .Returns(false)
                .Callback(() => Console.WriteLine("after pong"));

            mock.Object.DoSomething("pong");
        }

        [Test]
        public void VerificationTest()
        {
            var mock = new Mock<IFoo>();
            var consumer = new Consumer(mock.Object);

            consumer.Hello();

            mock.Verify(foo => foo.DoSomething("ping"), Times.AtLeastOnce);

            mock.Verify(foo => foo.DoSomething("pong"), Times.Never);

            mock.VerifyGet(foo => foo.Name);

            mock.VerifySet(foo => foo.SomeOtherProperty = It.IsInRange(100, 200, Range.Inclusive));


        }

        [Test]
        public void MockBehaviour()
        {
            var mock = new Mock<IFoo>(MockBehavior.Strict); // strict - wyjatek jesli nieokreslone wywolanie

            //mock.Object.DoSomething("abc"); // wyjatek

            mock.Setup(f => f.DoSomething("abc"))
                .Returns(true);

            mock.Object.DoSomething("abc");
        }

        [Test]
        public void RecursiveMock()
        {
            var mock = new Mock<IFoo>
            {
                DefaultValue = DefaultValue.Mock
            };

            var baz = mock.Object.SomeBaz;
            var bazMock = Mock.Get(baz);

            bazMock.SetupGet(f => f.Name).Returns("abc");

            var mockRepository = new MockRepository(MockBehavior.Strict)
            {
                DefaultValue = DefaultValue.Mock
            };

            var fooMock = mockRepository.Create<IFoo>();
            var otherMock = mockRepository.Create<IBaz>(MockBehavior.Loose);

            mockRepository.Verify();
        }
        

        public abstract class Person
        {
            protected int SSN { get; set; }
            protected abstract void Execute(string cmd);
        }
        [Test]
        public void ProtectedMembers()
        {
            var mock = new Mock<Person>();

            mock.Protected().SetupGet<int>("SSN").Returns(42);

            mock.Protected().Setup<string>("Execute", ItExpr.IsAny<string>());
        }
    }
}
