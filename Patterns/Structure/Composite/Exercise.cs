using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Composite.Exercise
{
    public class Exercise
    {
        public static void Test()
        {
            var single = new SingleValue { Value = 9 };
            Console.WriteLine($"single: {single.Sum()}");

            var many = new ManyValues { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Console.WriteLine($"single: {many.Sum()}");
        }
    }

    public interface IValueContainer : IEnumerable<int>
    {

    }

    public class SingleValue : IValueContainer
    {
        public int Value;

        public IEnumerator<int> GetEnumerator()
        {
            yield return Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ManyValues : List<int>, IValueContainer
    {

    }

    public static class ExtensionMethods
    {
        public static int Sum(this List<IValueContainer> containers)
        {
            int result = 0;
            foreach (var c in containers)
                foreach (var i in c)
                    result += i;
            return result;
        }
    }
}
