using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Patterns.Solid
{
    // If interface implements too much - split it into smaller interfaces.
    //
    // Wrong - IMachine is too large - if you have old fashioned printer you have to
    // implement methods that are not supported.
    //
    // Instead it should be split into smaller interfaces.
    public class InterfaceSegregation
    {
        public void Test()
        {
            WriteLine("If interface implements too much - split it into smaller interfaces.");
        }
    }

    public class Document
    {

    }

    // Wrong - IMachine is too large - if you have old fashioned printer you have to
    // implement methods that are not supported.

    public interface IMachine
    {
        void Print(Document d);
        void Scan(Document d);
        void Fax(Document d);
    }

    public class MultiFunctionPrinter : IMachine
    {
        public void Fax(Document d)
        {
           //
        }

        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }
    }

    public class OldFashionPrinter : IMachine
    {
        public void Fax(Document d)
        {
            throw new NotImplementedException(); // not supported - what to do?
        }

        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            throw new NotImplementedException(); // not supported - what to do?
        }
    }

    // Instead it should be split into smaller interfaces.
    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }

    public class Photocopier : IPrinter, IScanner
    {
        public void Print(Document d)
        {
            throw new NotImplementedException();
        }

        public void Scan(Document d)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMultiFunctionDevice : IPrinter, IScanner //..
    {

    }

    public class MultiFunctionMachine : IMultiFunctionDevice
    {
        public void Print(Document d)
        {
            printer.Print(d);
        } // decorator

        public void Scan(Document d)
        {
            scanner.Scan(d);
        } // decorator

        private IPrinter printer;
        private IScanner scanner;

        public MultiFunctionMachine(IPrinter printer, IScanner scanner)
        {
            this.printer = printer ?? throw new ArgumentNullException(paramName: nameof(printer));
            this.scanner = scanner ?? throw new ArgumentNullException(paramName: nameof(scanner));
        }
    }

}
