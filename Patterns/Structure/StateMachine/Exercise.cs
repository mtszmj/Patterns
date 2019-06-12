using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.StateMachine
{
    class Exercise
    {
        public static void Test()
        {

        }
    }

    public class CombinationLock
    {
        public int[] combination;
        private int pointer = 0;


        public CombinationLock(int[] combination)
        {
            this.combination = combination;
            Status = "LOCKED";
        }

        // you need to be changing this on user input
        public string Status;

        public void EnterDigit(int digit)
        {
            if (combination[pointer] == digit)
            {
                if(pointer == 0)
                {
                    Status = digit.ToString();
                }
                else
                {
                    Status += digit.ToString();
                }
                pointer++;
                if (pointer >= combination.Length)
                {
                    Status = "OPEN";
                    pointer = 0;
                }
            }
            else
            {
                pointer = 0;
                Status = "ERROR";
            }
        }
    }
}
