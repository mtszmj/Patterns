using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Flyweight
{
    class Exercise
    {
        public static void Test()
        {
            var sentence = new Sentence("hello world");
            sentence[1].Capitalize = true;
            Console.WriteLine(sentence);
        }
    }

    public class Sentence
    {
        private string plainText;
        private Dictionary<int, WordToken> wordTokens = 
            new Dictionary<int, WordToken>();

        public Sentence(string plainText)
        {
            this.plainText = plainText;
        }

        public WordToken this[int index]
        {
            get
            {
                if (wordTokens.ContainsKey(index))
                    return wordTokens[index];
                else
                {
                    var wt = new WordToken();
                    wordTokens.Add(index, wt);
                    return wt;
                }
            }
        }

        private bool IsCapitalized(int word)
        {
            if (!wordTokens.ContainsKey(word))
                return false;
            else
                return wordTokens[word].Capitalize;
        }

        public override string ToString()
        {
            var words = plainText.Split(' ');

            for (var idx = 0; idx < words.Length; idx++)
            {
                if (IsCapitalized(idx))
                    words[idx] = words[idx].ToUpper();
            }

            return string.Join(" ", words);
        }

        public class WordToken
        {
            public bool Capitalize;
        }
    }
}
