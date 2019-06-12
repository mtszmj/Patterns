using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Creation.FactoryExercise
{
    public class FactoryExercise
    {
        public void Test()
        {
            var factory = new PersonFactory();
            var p1 = factory.CreatePerson("Mateusz");
            var p2 = factory.CreatePerson("Ania");
            Console.WriteLine(p1);
            Console.WriteLine(p2);
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
        }
    }

    public class PersonFactory
    {
        private static int id = 0;

        public Person CreatePerson(string name)
        {
            return new Person { Name = name, Id = id++ };
        }
    }
}
