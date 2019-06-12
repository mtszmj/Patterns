using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Observer
{
    public class Exercise
    {
        public static void Test()
        {
            var g = new Game();
            var rat1 = new Rat(g);
            Console.WriteLine($"rat1: {rat1.Attack}");
            var rat2 = new Rat(g);
            Console.WriteLine($"rat1: {rat1.Attack}, rat2: {rat2.Attack}");
            var rat3 = new Rat(g);
            Console.WriteLine($"rat1: {rat1.Attack}, rat2: {rat2.Attack}, rat3: {rat3.Attack}");
            rat3.Dispose();
            Console.WriteLine($"rat1: {rat1.Attack}, rat2: {rat2.Attack}");
        }

        public class Game
        {
            public event EventHandler RatEnters, RatDies;
            public event EventHandler<Rat> NotifyRat;

            public void FireRatEnters(object sender)
            {
                RatEnters?.Invoke(sender, EventArgs.Empty);
            }

            public void FireRatDies(object sender)
            {
                RatDies?.Invoke(sender, EventArgs.Empty);
            }

            public void FireNotifyRat(object sender, Rat whichRat)
            {
                NotifyRat?.Invoke(sender, whichRat);
            }
        }

        public class Rat : IDisposable
        {
            private readonly Game game;
            public int Attack = 1;

            public Rat(Game game)
            {
                this.game = game;
                game.RatEnters += (sender, args) =>
                {
                    if (sender != this)
                    {
                        ++Attack;
                        game.FireNotifyRat(this, (Rat)sender);
                    }
                };
                game.NotifyRat += (sender, rat) =>
                {
                    if (rat == this) ++Attack;
                };
                game.RatDies += (sender, args) => --Attack;
                game.FireRatEnters(this);
            }


            public void Dispose()
            {
                game.FireRatDies(this);
            }
        }
    }
}
