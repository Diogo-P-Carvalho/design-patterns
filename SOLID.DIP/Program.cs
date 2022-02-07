using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID.DIP
{
    // DIP -> Dependency Inversion Principle
    // High-level modules should not depend upon low-level ones; use abstractions

    // high-level modules should not depend on low-level; both should depend on abstractions
    // abstractions should not depend on details; details should depend on abstractions

    enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    class Person
    {
        public string Name;
        // public DateTime DateOfBirth;
    }

    interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    // low-level
    class Relationsips : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)> ();

        public void AddParentChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        // public List<(Person, Relationship, Person)> Relations => relations;

        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return relations
                .Where(x => x.Item1.Name == name && x.Item2 == Relationship.Parent)
                .Select(r => r.Item3);
        }
    }

    class Research
    {
        // high-level: find all of john's children
        //public Research(Relationsips relationsips)
        //{
        //    var relations = relationsips.Relations;
        //    foreach (var rel in relations.Where(r => r.Item1.Name == "John" && r.Item2 == Relationship.Parent))
        //    {
        //        Console.WriteLine($"John has a child called {rel.Item3.Name}");
        //    }
        //}

        public Research(IRelationshipBrowser browser)
        {
            foreach (var p in browser.FindAllChildrenOf("John"))
            {
                Console.WriteLine($"John has a child called {p.Name}");
            }
        }

        static void Main(string[] args)
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Mary" };

            // low-level module
            var relationships = new Relationsips();
            relationships.AddParentChild(parent, child1);
            relationships.AddParentChild(parent, child2);

            new Research(relationships);
        }
    }
}
