using System.Collections.Generic;
using System.Linq;

namespace Patterns.Structure.Memento
{
    class Exercise
    {
        public class Token
        {
            public int Value = 0;

            public Token(int value)
            {
                this.Value = value;
            }
        }

        public class Memento
        {
            public List<int> Tokens { get; } = new List<int>();

            public Memento(IEnumerable<Token> tokens)
            {
                Tokens.AddRange(tokens.Select(t => t.Value));

                foreach(var t in tokens)
                {
                    Tokens.Add(t.Value);
                }
            }
        }

        public class TokenMachine
        {
            public List<Token> Tokens = new List<Token>();

            public Memento AddToken(int value)
            {
                Tokens.Add(new Token(value));
                return new Memento(Tokens);
            }

            public Memento AddToken(Token token)
            {
                Tokens.Add(token);
                return new Memento(Tokens);
            }

            public void Revert(Memento m)
            {
                Tokens.Clear();
                foreach (int v in m.Tokens)
                {
                    Tokens.Add(new Token(v));
                }
            }
        }
    }
}
