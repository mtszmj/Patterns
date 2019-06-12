using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Creation.Singleton
{
    class Exercise
    {
    }

    public class SingletonTester
    {
        public static bool IsSingleton(Func<object> func)
        {
            var a = func();
            var b = func();

            return a == b;
        }
    }
}
