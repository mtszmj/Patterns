using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Iterator.Exercise
{
    class Exercise
    {
        public static void Test()
        {

            var root0 = new Node<char>('F');
            var node1 = new Node<char>('B');
            var node2 = new Node<char>('A');
            var node3 = new Node<char>('D');
            var node4 = new Node<char>('C');
            var node5 = new Node<char>('E');
            var node6 = new Node<char>('G');
            var node7 = new Node<char>('I');
            var node8 = new Node<char>('H');

            root0.Left = node1;
            root0.Right = node6;

            node1.Left = node2;
            node1.Right = node3;

            node3.Left = node4;
            node3.Right = node5;

            node6.Right = node7;
            node7.Left = node8;

            foreach (var v in root0.PreOrder)
            {
                Console.WriteLine(v);
            }

        }
    }

    public class Node<T>
    {
        public T Value;
        public Node<T> Left, Right;
        public Node<T> Parent;

        public Node(T value)
        {
            Value = value;
        }

        public Node(T value, Node<T> left, Node<T> right)
        {
            Value = value;
            Left = left;
            Right = right;

            left.Parent = right.Parent = this;
        }

        public IEnumerable<T> PreOrder
        {
            get
            {
                yield return Value;

                if(Left != null)
                {
                    foreach(var value in Left.PreOrder)
                    {
                        yield return value;
                    }
                }

                if (Right != null)
                {
                    foreach(var value in Right.PreOrder)
                    {
                        yield return value;
                    }
                }
            }
        }
    }
}
