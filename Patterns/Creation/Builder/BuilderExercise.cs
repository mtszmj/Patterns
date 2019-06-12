using System;
using System.Collections.Generic;
using System.Text;

namespace Patterns.Creation
{
    public class BuilderExercise
    {
        public void Test()
        {
            var cb = new CodeBuilder("Person").AddField("Name", "string").AddField("Age", "int");
            Console.WriteLine(cb);
        }
    }

    public class CodeBuilder
    {
        protected Code code = new Code();

        public CodeBuilder(string className)
        {
            code.ClassName = className;
        }

        public CodeBuilder AddField(string name, string type)
        {
            code.AddField(name, type);
            return this;
        }

        public override string ToString()
        {
            return code.ToString();
        }
    }

    public class Code
    {
        public string ClassName { get; set; }
        Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public void AddField(string name, string type)
        {
            Properties.Add(name, type);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"public class {ClassName}");
            sb.AppendLine("{");
            foreach (var p in Properties)
            {
                sb.AppendLine($"  public {p.Value} {p.Key};");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
