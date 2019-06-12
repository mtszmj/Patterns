using System;

namespace Patterns.Structure.Proxy
{
    public class Exercise
    {
        public class Person : IPerson
        {
            public int Age { get; set; }

            public string Drink()
            {
                return "drinking";
            }

            public string Drive()
            {
                return "driving";
            }

            public string DrinkAndDrive()
            {
                return "driving while drunk";
            }
        }

        public class ResponsiblePerson : IPerson
        {
            private IPerson _Person;

            public ResponsiblePerson(IPerson person)
            {
                _Person = person ?? throw new ArgumentNullException(nameof(person));
            }

            public int Age { get; set; }

            public string Drink()
            {
                if (Age < 18)
                    return "too young";
                else
                    return _Person.Drink();
            }

            public string DrinkAndDrive()
            {
                return "dead";
            }

            public string Drive()
            {
                if (Age < 16)
                    return "too young";
                else
                    return _Person.Drive();
            }
        }

        public interface IPerson
        {
            int Age { get; set; }
            string Drink();
            string Drive();
            string DrinkAndDrive();

        }

    }
}
