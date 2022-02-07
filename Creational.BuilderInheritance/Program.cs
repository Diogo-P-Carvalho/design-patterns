using System;

namespace Creational.Builder.Inheritance
{
    class Person
    {
        public string Name;
        public string Position;
        public DateTime DateOfBirth;

        public class Builder : PersonBirthDateBuilder<Builder>
        {
            internal Builder() { }
        }

        public static Builder New => new Builder();

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }
    }

    abstract class PersonBuilder
    {
        protected Person person = new Person();

        public Person Build() 
        { 
            return person; 
        }
    }
    class PersonInfoBuilder<Self> : PersonBuilder where Self : PersonInfoBuilder<Self>
    {        
        public Self Called(string name)
        {
            person.Name = name;
            return (Self)this;
        }
    }

    class PersonJobBuilder<Self> : PersonInfoBuilder<PersonJobBuilder<Self>> where Self : PersonJobBuilder<Self>
    {
        public Self WorkAsA(string position)
        {
            person.Position = position;
            return (Self)this;
        }
    }

    // here's another inheritance level
    // note there's no PersonInfoBuilder<PersonJobBuilder<PersonBirthDateBuilder<SELF>>>!
    class PersonBirthDateBuilder<Self> : PersonJobBuilder<PersonBirthDateBuilder<Self>> where Self : PersonBirthDateBuilder<Self>
    {
        public Self Born(DateTime dateOfBirth)
        {
            person.DateOfBirth = dateOfBirth;
            return (Self)this;
        }
    }

    internal class Program
    {
        class SomeBuilder : PersonBirthDateBuilder<SomeBuilder>
        {

        }

        static void Main(string[] args)
        {
            var p = Person.New
                        .Called("dimitri")
                        .WorkAsA("quant")
                        .Born(DateTime.Now)
                        .Build();

            Console.WriteLine(p);
        }
    }
}
