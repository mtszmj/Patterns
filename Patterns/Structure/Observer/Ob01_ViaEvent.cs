using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Observer
{
    class Ob01_ViaEvent
    {
        public static void Test()
        {
            var person = new Person();

            person.FallsIll += CallDoctor;

            person.CatchACold();

            person.FallsIll -= CallDoctor;


        }

        private static void CallDoctor(object sender, FallsIllEventArgs eventArgs)
        {
            Console.WriteLine($"Call doctor to {eventArgs.Address}");
        }
    }

    public class FallsIllEventArgs : EventArgs
    {
        public string Address;
    }

    public class Person
    {
        public event EventHandler<FallsIllEventArgs> FallsIll;

        public void CatchACold()
        {
            FallsIll?.Invoke(this, new FallsIllEventArgs { Address = "123 London Road" });
        }
    }


}
