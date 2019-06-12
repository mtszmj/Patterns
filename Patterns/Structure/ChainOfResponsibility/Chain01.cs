using System;

namespace Patterns.Structure.ChainOfResponsibility.C01
{
    class Chain01
    {
        public static void Test()
        {
            var goblin = new Creature("Goblin", 2, 2);
            Console.WriteLine(goblin);

            var root = new CreatureModifier(goblin);

            //root.Add(new NoBonusesModifier(goblin));

            Console.WriteLine("Let's double the goblin's attack");
            root.Add(new DoubleAttackModifier(goblin));

            Console.WriteLine("Let's increase the goblin's defense");
            root.Add(new IncreasedDefenseModifier(goblin));

            root.Handle();

            Console.WriteLine(goblin);
        }
    }

    public class Creature
    {
        public string Name;
        public int Attack, Defense;

        public Creature(string Name, int attack, int defense)
        {
            this.Name = Name ?? throw new ArgumentNullException(nameof(Name));
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Attack: {Attack}, Defense: {Defense}";
        }
    }

    public class CreatureModifier
    {
        protected Creature creature;
        protected CreatureModifier next; // linked list

        public CreatureModifier(Creature creature)
        {
            this.creature = creature;
        }

        public void Add(CreatureModifier cm)
        {
            if (next != null) next.Add(cm);
            else next = cm;
        }

        public virtual void Handle() => next?.Handle();
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine($"Doubling {creature.Name}'s attack");
            creature.Attack *= 2;
            base.Handle();
        }
    }

    public class IncreasedDefenseModifier : CreatureModifier
    {
        public IncreasedDefenseModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            Console.WriteLine($"Increasing {creature.Name}'s defense");
            creature.Defense += 3;
            base.Handle();
        }
    }

    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            // do nothing - stop the chain
        }
    }

}
