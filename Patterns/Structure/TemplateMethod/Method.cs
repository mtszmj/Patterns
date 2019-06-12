using System;

namespace Patterns.Structure.TemplateMethod
{
    class Method
    {
        public static void Test()
        {
            var chess = new Chess();
            chess.Run();
        }
    }

    public abstract class Game
    {
        public Game(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
        }

        public void Run()
        {
            Start();
            while (!HaveWinner)
                TakeTurn();

            Console.WriteLine($"Player {WinningPlayer} wins");
        }

        protected int currentPlayer;
        protected readonly int numberOfPlayers;
        protected abstract void Start();
        protected abstract void TakeTurn();
        protected abstract bool HaveWinner { get; }
        protected abstract int WinningPlayer { get; }
    }

    public class Chess : Game
    {
        public Chess() : base(2)
        {

        }

        protected override bool HaveWinner => turn == maxTurns;

        protected override int WinningPlayer => currentPlayer;

        protected override void Start()
        {
            Console.WriteLine($"Starting a game of chess with {numberOfPlayers} players.");
        }

        protected override void TakeTurn()
        {
            Console.WriteLine($"Turn {turn++} taken by player {currentPlayer}.");
            currentPlayer = (currentPlayer + 1) % numberOfPlayers;
        }

        private int turn = 1;
        private int maxTurns = 10;
    }
}
