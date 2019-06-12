using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Decorator.Exercise
{
    public class Exercise
    {
    }

    public class Bird
    {
        public int Age { get; set; }

        public string Fly()
        {
            return (Age < 10) ? "flying" : "too old";
        }
    }

    public class Lizard
    {
        public int Age { get; set; }

        public string Crawl()
        {
            return (Age > 1) ? "crawling" : "too young";
        }
    }

    public class Dragon // no need for interfaces
    {
        private Bird _Bird = new Bird();
        private Lizard _Lizard = new Lizard();
        private int _Age;

        public int Age
        {
            get
            {
                return _Age;
            }
            set
            {
                _Age = value;
                _Bird.Age = value;
                _Lizard.Age = value;
            }
        }

        public string Fly()
        {
            return _Bird.Fly();
        }

        public string Crawl()
        {
            return _Lizard.Crawl();
        }
    }
}
