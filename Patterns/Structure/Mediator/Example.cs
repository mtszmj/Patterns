using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Mediator
{
    class Example
    {
        public static void Test()
        {
            var m = new Mediator();
            var p1 = new Participant(m);
            var p2 = new Participant(m);

            p1.Say(3);
            Console.WriteLine($"{p1.Value} / {p2.Value}");

            p2.Say(2);
            Console.WriteLine($"{p1.Value} / {p2.Value}");
        }
    }

    public class Participant
    {
        public int Value { get; set; }
        private Mediator _Mediator;

        public Participant(Mediator mediator)
        {
            mediator.AddParticipant(this);
            _Mediator = mediator;
        }

        public void Say(int n)
        {
            _Mediator?.Say(this, n);
        }

        public void Receive(int n)
        {
            Value += n;
        }

    }

    public class Mediator
    {
        List<Participant> Participants = new List<Participant>();

        public void AddParticipant(Participant p)
        {
            Participants.Add(p);
        }

        public void Say(Participant participant, int n)
        {
            foreach(var p in Participants)
            {
                if (!p.Equals(participant))
                    p.Receive(n);
            }
            
        }

        // todo
    }
}
