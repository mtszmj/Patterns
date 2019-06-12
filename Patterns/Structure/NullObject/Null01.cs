using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImpromptuInterface;
using System.Dynamic;

namespace Patterns.Structure.NullObject
{
    class Null01
    {
        public static void Test()
        {
            var log = new ConsoleLog();
            var ba = new BankAccount(log);
            
            ba.Deposit(100);

            // jesli nie chcesz logowac musisz podac null a to da wyjatek lub dodac '?.',
            // ale to wymaga zaplanowania z gory, ze moze byc null i spamowania '?.'

            //var cb = new ContainerBuilder();
            //cb.Register(ctx => new BankAccount(null));
            //using (var c = cb.Build())
            //{
            //    var ba2 = c.Resolve<BankAccount>();
            //}

            // lepiej dodac klase NullLog
            Console.WriteLine("null log: ");
            var cb = new ContainerBuilder();
            cb.RegisterType<BankAccount>();
            cb.RegisterType<NullLog>().As<ILog>();
            using (var c = cb.Build())
            {
                var ba3 = c.Resolve<BankAccount>();
                ba3.Deposit(100);
            }
            Console.WriteLine("after null log.");

            // dynamic - performance hit (DLR)
            var nulllog = Null<ILog>.Instance;
            nulllog.Info("sdsdasds");
            var baa = new BankAccount(nulllog);
            baa.Deposit(100);
        }
    }

    public interface ILog
    {
        void Info(string msg);
        void Warn(string msg);
    }

    class ConsoleLog : ILog
    {
        public void Info(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Warn(string msg)
        {
            Console.WriteLine("WARNING!!! " + msg);
        }
    }

    public class BankAccount
    {
        private ILog log;
        private int balance;

        public BankAccount(ILog log)
        {
            this.log = log; //?? throw new ArgumentNullException(nameof(log));
        }

        public void Deposit(int amount)
        {
            balance += amount;
            log?.Info($"Deposited {amount}, balance is now {balance}");
        }
    }

    public class NullLog : ILog
    {
        public void Info(string msg)
        {

        }

        public void Warn(string msg)
        {

        }
    }

    public class Null<TInterface> : DynamicObject where TInterface : class
    {
        public static TInterface Instance =>
                new Null<TInterface>().ActLike<TInterface>();


        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = Activator.CreateInstance(binder.ReturnType);
            return true;
        }
    }
}