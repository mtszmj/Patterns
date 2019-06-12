using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Iterator
{
    class ArrayBackedProperties
    {
        public static void Test()
        {

        }

    }

    public class Creature : IEnumerable<int>
    {
        private int[] stats = new int[3];

        private const int strength = 0;

        public int Strength
        {
            get => stats[strength];
            set => stats[strength] = value;
        }
        public int Agility
        {
            get => stats[1];
            set => stats[1] = value;
        }
        public int Intelligence
        {
            get => stats[2];
            set => stats[2] = value;
        }

        public double AverageStat => stats.Average();

        public IEnumerator<int> GetEnumerator()
        {
            return stats.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this[int index]
        {
            get { return stats[index]; }
            set { stats[index] = value; }
        }
    }
}
