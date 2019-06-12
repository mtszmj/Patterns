using System.Collections.Generic;
using System.Linq;
using System;

namespace Patterns.Solid
{
    // High level parts should not rely on low level parts
    // They should depend on abstraction.
    public class DependencyInversion
    {
        public void Test()
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Mary" };

            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Research(relationships);
        }
    }

    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public class Person
    {
        public string Name;
    }

    // low-level
    // better to implement interface with abstraction
    public class Relationships : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> relations
            = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            //foreach (var r in relations.Where(
            //    x => x.Item1.Name == name &&
            //    x.Item2 == Relationship.Parent
            //    ))
            //{
            //    yield return r.Item3;
            //}
            return relations.Where(
                x => x.Item1.Name == name
                && x.Item2 == Relationship.Parent
            ).Select(r => r.Item3);

        }

        // WRONG:
        //public List<(Person, Relationship, Person)> Relations => relations;
    }

    public class Research
    {
        // WRONG
        //public Research(Relationships relationships)
        //{
        //    var relations = relationships.Relations;
        //    foreach(var r in relations.Where(
        //        x => x.Item1.Name == "John" &&
        //        x.Item2 == Relationship.Parent
        //        ))
        //    {
        //        Console.WriteLine($"John has a child called {r.Item3.Name}");
        //    }
        //}

        public Research(IRelationshipBrowser browser)
        {
            foreach (var p in browser.FindAllChildrenOf("John"))
            {
                Console.WriteLine($"John has a child called {p.Name}");
            }
        }
    }

    // Better to define abstraction - interface to access data
    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }
}
