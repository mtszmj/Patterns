using MoreLinq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;

namespace Patterns.Creation.Singleton.S01
{
    public class Singleton
    {
        public static void Test()
        {
            var db = SingletonDatabase.Instance;
            //var db = new OrdinaryDatabase();
            Console.WriteLine("After instance, before read db");
            var city = "Tokyo";
            Console.WriteLine($"{city} has population of {db.GetPopulation(city)}.");

        }
    }

    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    public class SingletonDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;
        private static int instanceCount; // 0
        public static int Count => instanceCount;

        private SingletonDatabase()
        {
            instanceCount++;
            Console.WriteLine("Initializing database.");

            capitals = File.ReadAllLines(       //"C:\\Users\\RWSwiss\\source\\repos\\Patterns\\Patterns\\bin\\Debug\\Creation\\Singleton\\capitals.txt")
                Path.Combine(new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, "Creation\\Singleton\\capitals.txt")
                )
                .Batch(2)
                .ToDictionary(
                list => list.ElementAt(0).Trim(),
                list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }

        private static Lazy<SingletonDatabase> instance =
            new Lazy<SingletonDatabase>(() => new SingletonDatabase());

        public static SingletonDatabase Instance => instance.Value;
    }

    public class OrdinaryDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;

        public OrdinaryDatabase()
        {
            Console.WriteLine("Initializing database.");

            capitals = File.ReadAllLines(       //"C:\\Users\\RWSwiss\\source\\repos\\Patterns\\Patterns\\bin\\Debug\\Creation\\Singleton\\capitals.txt")
                Path.Combine(new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, "Creation\\Singleton\\capitals.txt")
                )
                .Batch(2)
                .ToDictionary(
                list => list.ElementAt(0).Trim(),
                list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }
    }

    public class SingletonRecordFinder
    {
        public int GetTotalPopulation(IEnumerable<string> names)
        {
            var result = 0;
            foreach (var name in names)
            {
                result += SingletonDatabase.Instance.GetPopulation(name);
            }
            return result;
        }
    }

    public class ConfigurableRecordFinder
    {
        private IDatabase database;

        public ConfigurableRecordFinder(IDatabase database)
        {
            this.database = database ?? throw new ArgumentNullException(paramName: nameof(database));
        }

        public int GetTotalPopulation(IEnumerable<string> names)
        {
            var result = 0;
            foreach (var name in names)
            {
                result += database.GetPopulation(name);
            }
            return result;
        }
    }

    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string name)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3
            }[name];
        }
    }

    [TestFixture]
    public class SingletonTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var db = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;
            Assert.That(db, Is.SameAs(db2));
            Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
        }

        // Problem z testowaniem polega na tym, ze mamy hardcodowana referencje do singletonu w SingletonRecordFinder.
        // Jezeli dzialamy na prawdziwej bazie danych to po usunieciu np. Mexico City z bazy, test przestaje dzialac
        // plus dostep do bazy danych moze byc kosztowny co niekoniecznie jest dobre na testy. Rozwiazaniem jest 
        // ConfigurableRecordFinder
        [Test]
        public void SingletonTotalPopulationTest()
        {
            var rf = new SingletonRecordFinder();
            var names = new[] { "Seoul", "Mexico City" };
            int tp = rf.GetTotalPopulation(names);
            Assert.That(tp, Is.EqualTo(17500000 + 17400000));
        }

        [Test]
        public void ConfigurablePopulationTest()
        {
            var rf = new ConfigurableRecordFinder(new DummyDatabase());
            var names = new[] { "alpha", "gamma" };
            int tp = rf.GetTotalPopulation(names);
            Assert.That(tp, Is.EqualTo(4));
        }

        [Test]
        public void DummyDependencyInjectionPopulationTest()
        {
            // Autofac jest uzywany - nuget package
            var cb = new ContainerBuilder();
            //cb.RegisterType<OrdinaryDatabase>()
            cb.RegisterType<DummyDatabase>()
                .As<IDatabase>()
                .SingleInstance();
            cb.RegisterType<ConfigurableRecordFinder>();
            using (var c = cb.Build())
            {
                var rf = c.Resolve<ConfigurableRecordFinder>();
                var names = new[] { "alpha", "gamma" };
                int tp = rf.GetTotalPopulation(names);
                Assert.That(tp, Is.EqualTo(4));
            }
        }

        [Test]
        public void OrdinaryDependencyInjectionPopulationTest()
        {
            // Autofac jest uzywany - nuget package
            var cb = new ContainerBuilder();
            cb.RegisterType<OrdinaryDatabase>()
                .As<IDatabase>()
                .SingleInstance();
            cb.RegisterType<ConfigurableRecordFinder>();
            using (var c = cb.Build())
            {
                var rf = c.Resolve<ConfigurableRecordFinder>();
                var names = new[] { "Seoul", "Mexico City" };
                int tp = rf.GetTotalPopulation(names);
                Assert.That(tp, Is.EqualTo(17500000 + 17400000));
            }
        }
    }
}
