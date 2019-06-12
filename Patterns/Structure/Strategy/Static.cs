using System;
using System.Collections.Generic;
using System.Text;

namespace Patterns.Structure.Strategy.Static
{
    class Static
    {
        public static void Test()
        {
            // jedno rozwiazanie DI
            // cb.Register<MarkdownListStrategy>().As<IListStrategy>()


            // inne - <T>
            var tp = new TextProcessor<MarkdownListStrategy>();
            tp.AppendList(new[] { "foo", "bar", "baz" });
            Console.WriteLine(tp);

            var tp2 = new TextProcessor<HtmlListStrategy>();
            tp2.AppendList(new[] { "foo", "bar", "baz" });
            Console.WriteLine(tp2);
        }

        public enum OutputFormat
        {
            Markdown,
            Html
        }

        // html: <ul><li>foo</li></ul>
        public interface IListStrategy
        {
            void Start(StringBuilder sb);
            void End(StringBuilder sb);
            void AddListItem(StringBuilder sb, string item);
        }

        public class HtmlListStrategy : IListStrategy
        {
            public void AddListItem(StringBuilder sb, string item)
            {
                sb.AppendLine($"    <li>{item}</li>");
            }

            public void End(StringBuilder sb)
            {
                sb.AppendLine("</ul>");
            }

            public void Start(StringBuilder sb)
            {
                sb.AppendLine("<ul>");
            }
        }

        public class MarkdownListStrategy : IListStrategy
        {
            public void AddListItem(StringBuilder sb, string item)
            {
                sb.AppendLine($" * {item}");
            }

            public void End(StringBuilder sb)
            {
            }

            public void Start(StringBuilder sb)
            {
            }
        }

        public class TextProcessor<LS> where LS: IListStrategy, new()
        {
            StringBuilder sb = new StringBuilder();
            IListStrategy listStrategy = new LS();

            public void AppendList(IEnumerable<string> items)
            {
                listStrategy.Start(sb);
                foreach (var item in items)
                {
                    listStrategy.AddListItem(sb, item);
                }
                listStrategy.End(sb);
            }

            public StringBuilder Clear()
            {
                return sb.Clear();
            }

            public override string ToString()
            {
                return sb.ToString();
            }
        }
    }


}
