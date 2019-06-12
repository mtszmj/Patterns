using System;
using System.Collections.Generic;

namespace Patterns.Creation.FactoryAbstract01
{
    // this version violates Open-Closed principle by using enum. If you want to add
    // new drink you need to modify HotDrinkMachine and its inner enum.
    public class FactoryAbstract01
    {
        public void Test()
        {
            var machine = new HotDrinkMachine();
            var tea = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Tea, 100);
            tea.Consume();
            var coffee = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Coffee, 150);
            coffee.Consume();
        }
    }

    public interface IHotDrink
    {
        void Consume();
    }

    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("This tea is nice.");
        }
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("This coffee is great.");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }

    internal class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Tea is prepared with complicated process - {amount} ml.");
            return new Tea();
        }
    }

    internal class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Coffee is prepared with complicated process - {amount} ml.");
            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        public enum AvailableDrink
        {
            Coffee, Tea
        }

        private Dictionary<AvailableDrink, IHotDrinkFactory> factories =
            new Dictionary<AvailableDrink, IHotDrinkFactory>();

        public HotDrinkMachine()
        {
            foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
            {
                var factory = (IHotDrinkFactory)Activator.CreateInstance(
                    Type.GetType("Patterns.Creation.FactoryAbstract01." +
                    Enum.GetName(typeof(AvailableDrink), drink) +
                    "Factory")
                    );
                factories.Add(drink, factory);
            }
        }

        public IHotDrink MakeDrink(AvailableDrink drink, int amount)
        {
            return factories[drink].Prepare(amount);
        }

    }
}
