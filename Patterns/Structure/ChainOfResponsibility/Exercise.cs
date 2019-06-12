using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Patterns.Structure.ChainOfResponsibility
{
    class Exercise
    {

    }

    public abstract class Creature
    {
        protected int _Attack;
        protected int _Defense;

        public virtual int Attack { get => _Attack; set => _Attack = value; }
        public virtual int Defense { get =>_Defense; set => _Defense = value; }
        protected Game _Game;

        public Creature(Game game, int attack, int defense)
        {
            _Game = game ?? throw new ArgumentNullException(nameof(game));
            Attack = attack;
            Defense = defense;
        }
    }

    public class Goblin : Creature
    {
        public Goblin(Game game) : this(game, 1, 1) { }

        protected Goblin(Game game, int attack, int defense)
            : base(game, attack, defense) { }

        public override int Attack
        {
            get
            {
                var q = new Query(this, typeof(GoblinKing)); // GoblinKing modifies attack +1
                return _Game.PerformQuery(q) + _Attack;
            }
            set => base.Attack = value;
        }

        public override int Defense
        {
            get
            {
                var q = new Query(this, typeof(Goblin)); // Every Goblin (and GoblinKing inheriting from Goblin) modify defense +1
                return _Game.PerformQuery(q) + _Defense;
            }
            set => base.Defense = value;

        }
    }

    public class GoblinKing : Goblin
    {
        public GoblinKing(Game game) : base(game, 3, 3) { }
    }

    public class Game
    {
        public IList<Creature> Creatures = new List<Creature>();

        public int PerformQuery(Query q)
        {
            int value = 0;
            foreach (var creature in Creatures)
            {
                if (q.What.IsAssignableFrom(creature.GetType())
                    && !ReferenceEquals(creature, q.Creature))
                {
                    Console.WriteLine($"{q.What} is assignable from {creature.GetType().Name} ({++value})");
                }
            }

            return value;
        }
    }

    public class Query
    {
        public Creature Creature;
        public Type What;

        public Query(Creature creature, Type what)
        {
            Creature = creature;
            What = what;
        }
    }

    [TestFixture]
    public class TestFixture
    {
        [Test]
        public void Test()
        {
            var game = new Game();
            var goblin = new Goblin(game);
            game.Creatures.Add(goblin);
            Assert.That(goblin.Attack, Is.EqualTo(1));
            Assert.That(goblin.Defense, Is.EqualTo(1));
        }

        [Test]
        public void Test2()
        {
            var game = new Game();
            var goblin = new Goblin(game);
            var goblin2 = new Goblin(game);
            var goblin3 = new Goblin(game);
            game.Creatures.Add(goblin);
            game.Creatures.Add(goblin2);
            game.Creatures.Add(goblin3);
            Assert.That(goblin.Attack, Is.EqualTo(1));
            Assert.That(goblin.Defense, Is.EqualTo(3));
            Assert.That(goblin2.Attack, Is.EqualTo(1));
            Assert.That(goblin2.Defense, Is.EqualTo(3));
            Assert.That(goblin3.Attack, Is.EqualTo(1));
            Assert.That(goblin3.Defense, Is.EqualTo(3));
        }

        [Test]
        public void Test3()
        {
            var game = new Game();
            var goblin = new Goblin(game);
            var goblin2 = new Goblin(game);
            var goblin3 = new Goblin(game);
            var goblinKing = new GoblinKing(game);
            game.Creatures.Add(goblin);
            game.Creatures.Add(goblin2);
            game.Creatures.Add(goblin3);
            game.Creatures.Add(goblinKing);
            Assert.That(goblin.Attack, Is.EqualTo(2));
            Assert.That(goblin.Defense, Is.EqualTo(4));
            Assert.That(goblin2.Attack, Is.EqualTo(2));
            Assert.That(goblin2.Defense, Is.EqualTo(4));
            Assert.That(goblin3.Attack, Is.EqualTo(2));
            Assert.That(goblin3.Defense, Is.EqualTo(4));
            Assert.That(goblinKing.Attack, Is.EqualTo(3));
            Assert.That(goblinKing.Defense, Is.EqualTo(6));
        }
    }

}
