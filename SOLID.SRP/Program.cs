using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace SOLID.SRP
{
    // SRP -> Single Responsibility Principle
    // A class should only have one reason to change
    // Separation of concerns - different classes handling different, independent tasks/problems

    // just stores a couple of journal entries and ways of working with them
    public class Journal
    {
        private readonly List<string> _entries = new List<string>();
        private static int _count = 0;

        public int AddEntry(string text)
        {
            _entries.Add($"{++_count}: {text}");
            return _count; // memento pattern!
        }

        public void RemoveEntry(int index)
        {
            _entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, _entries);
        }

        // breaks single responsibility principle
        //public void Save(string filename)
        //{
        //    File.WriteAllText(filename, ToString());
        //}

        //public static Journal Load(string filename)
        //{

        //}

        //public void Load(Uri uri)
        //{

        //}
    }

    // handles the responsibility of persisting objects
    public class Persistence
    {
        public void SaveToFile(Journal journal, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
            {
                File.WriteAllText(filename, journal.ToString());
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var j = new Journal();
            j.AddEntry("I cried today");
            j.AddEntry("I ate a bug");
            WriteLine(j);

            var p = new Persistence();
            var filename = @"./jounal.txt";
            p.SaveToFile(j, filename, true);
        }
    }
}
