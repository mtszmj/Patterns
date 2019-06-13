using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImpromptuInterface;

namespace UnitTests
{
    public interface ILog
    {
        bool Write(string msg);
    }

    public class ConsoleLog : ILog
    {
        public bool Write(string msg)
        {
            Console.WriteLine(msg);
            return true;
        }
    }

    public class BAccount
    {
        public int Balance { get; set; }
        private readonly ILog log;

        public BAccount(ILog log)
        {
            this.log = log;
        }

        public void Deposit(int amount)
        {
            if(log.Write($"Depositing {amount}")) { 
                Balance += amount;
            }
        }
    }

    [TestFixture]
    public class BAccountTests
    {
        private BAccount ba;

        [Test]
        public void DepositIntegrationTest()
        {
            ba = new BAccount(new ConsoleLog()) { Balance = 100 };
            ba.Deposit(100);
            Assert.That(ba.Balance, Is.EqualTo(200));
        }

        [Test]
        public void DepositUnitTestWithFake()
        {
            var log = new NullLog();
            ba = new BAccount(log) { Balance = 100 };
            ba.Deposit(100);
            Assert.That(ba.Balance, Is.EqualTo(200));
        }

        [Test]
        public void DepositUnitTestWithDynamicFake()
        {
            var log = Null<ILog>.Instance;
            ba = new BAccount(log) { Balance = 100 };
            ba.Deposit(100);
            Assert.That(ba.Balance, Is.EqualTo(200));
        }

        [Test]
        public void DepositUnitTestWithStub()
        {
            var log = new NullLogWithResult(true);
            ba = new BAccount(log) { Balance = 100 };
            ba.Deposit(100);
            Assert.That(ba.Balance, Is.EqualTo(200));
        }

        [Test]
        public void DepositUnitTestWithMock()
        {
            var log = new LogMock(true);
            ba = new BAccount(log) { Balance = 100 };
            ba.Deposit(100);
            Assert.Multiple(() => { 
                Assert.That(ba.Balance, Is.EqualTo(200));
                Assert.That(log.MethodCallCount[nameof(LogMock.Write)], Is.EqualTo(1));
            });
        }
    }

    // Static fake
    public class NullLog : ILog
    {
        public bool Write(string msg)
        { return true; }
    }

    // Dynamic fake with ImpromptuInterface
    public class Null<T> : DynamicObject where T : class
    {
        public static T Instance
        {
            get
            {
                return new Null<T>().ActLike<T>();
            }
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = Activator.CreateInstance(typeof(T).GetMethod(binder.Name).ReturnType);

            return true;
        }
    }

    // Stub
    public class NullLogWithResult : ILog
    {
        private readonly bool expectedResult;

        public NullLogWithResult(bool expectedResult)
        {
            this.expectedResult = expectedResult;
        }

        public bool Write(string msg)
        {
            return expectedResult;
        }
    }

    // Mock
    public class LogMock : ILog
    {
        private bool expectedResult;
        public Dictionary<string, int> MethodCallCount;

        public LogMock(bool expectedResult)
        {
            this.expectedResult = expectedResult;
            MethodCallCount = new Dictionary<string, int>();
        }

        private void AddOrIncrement(string methodName)
        {
            if (MethodCallCount.ContainsKey(methodName))
                MethodCallCount[methodName]++;
            else
                MethodCallCount.Add(methodName, 1);
        }

        public bool Write(string msg)
        {
            AddOrIncrement(nameof(Write));
            return expectedResult;
        }
    }
}
