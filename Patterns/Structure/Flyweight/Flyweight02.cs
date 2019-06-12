using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Flyweight
{
    public class Flyweight02
    {
        public static void Test()
        {
            var ft = new FormatedText("This is a brave new world");
            ft.Capitalize(10, 15);
            Console.WriteLine(ft);

            var bft = new BetterFormatedText("This is a brave new world");
            bft.GetRange(10, 15).Capitalize = true;
            Console.WriteLine(bft);
        }
    }

    public class FormatedText
    {
        private readonly string plainText;
        private bool[] capitalized;

        public FormatedText(string plainText)
        {
            this.plainText = plainText;
            capitalized = new bool[plainText.Length];
        }

        public void Capitalize(int start, int end)
        {
            for(int i=start; i<=end; i++)
            {
                capitalized[i] = true;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for(var i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];
                sb.Append(capitalized[i] ? char.ToUpper(c) : c);
            }

            return sb.ToString();
        }
    }

    public class BetterFormatedText
    {
        private readonly string plainText;
        private List<TextRange> formatting = new List<TextRange>();


        public BetterFormatedText(string plainText)
        {
            this.plainText = plainText;
        }

        public TextRange GetRange(int start, int end)
        {
            var range = new TextRange { Start = start, End = end };
            formatting.Add(range);
            return range;
        }

        public class TextRange
        {
            public int Start, End;
            public bool Capitalize, Bold, Italic;

            public bool Covers(int position)
            {
                return position >= Start && position <= End;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];
                foreach(var range in formatting)
                {
                    if (range.Covers(i) && range.Capitalize)
                    {
                        c = char.ToUpper(c);
                    }
                }
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
