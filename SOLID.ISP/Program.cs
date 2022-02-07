using System;

namespace SOLID.ISP
{
    // ISP -> Interface Segregation Principle
    // Don't put too much into an interface; split into seperate interfaces
    // YAGNI - You Ain't Going to Need It

    class Document
    {

    }

    interface IMachine
    {
        void Print(Document document);
        void Scan(Document document);
        void Fax(Document document);
    }

    // ok if you need a multifunction machine
    class MultiFunctionPrinter : IMachine
    {
        public void Fax(Document document)
        {
            //
        }

        public void Print(Document document)
        {
            //
        }

        public void Scan(Document document)
        {
            //
        }
    }

    class OldFashionPrinter : IMachine
    {
        public void Fax(Document document)
        {
            // yep
        }

        public void Print(Document document)
        {
            throw new NotImplementedException();
        }

        public void Scan(Document document)
        {
            throw new NotImplementedException();
        }
    }

    interface IPrinter
    {
        void Print(Document document);
    }

    interface IScanner
    {
        void Scan(Document document);
    }

    class Printer : IPrinter
    {
        public void Print(Document d)
        {

        }
    }

    class Photocopier : IPrinter, IScanner
    {
        public void Print(Document document)
        {
            throw new NotImplementedException();
        }

        public void Scan(Document document)
        {
            throw new NotImplementedException();
        }
    }

    interface IMultiFunctionDevice : IScanner, IPrinter // ...
    {

    }

    struct MultiFunctionMachine : IMultiFunctionDevice
    {
        // compose this out of several modules
        private readonly IPrinter _printer;
        private readonly IScanner _scanner;

        public MultiFunctionMachine(IPrinter printer, IScanner scanner)
        {
            _printer = printer ?? throw new ArgumentNullException(paramName: nameof(printer));
            _scanner = scanner ?? throw new ArgumentNullException(paramName: nameof(scanner));
        }

        public void Print(Document d)
        {
            _printer.Print(d);
        }

        public void Scan(Document d)
        {
            _scanner.Scan(d);
        } // decorator pattern!
    }

    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
