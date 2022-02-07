using System;
using System.Collections.Generic;
using System.Text;

namespace Creational.Builder.Pattern
{
    // A builder is a separate component for building an object
    // Can either give builder a constructor or return it via a static function
    // To make builder fluent, return this
    // Different facets of an object can be built with different builders working in tandem via a base class

    class HtmlElement
    {
        public string Name, Text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        private const int INDENT_SIZE = 2;

        public HtmlElement()
        {

        }

        public HtmlElement(string name, string text)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Text = text ?? throw new ArgumentNullException(paramName: nameof(name));
        }

        private string ToStringImp(int indent)
        {
            var sb = new StringBuilder();
            var i = new string(' ', INDENT_SIZE * indent);
            sb.Append($"{i}<{Name}>\n");

            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.Append(new string(' ', INDENT_SIZE * (indent + 1)));
                sb.AppendLine(Text);
            }

            foreach (var element in Elements)
            {
                sb.Append(element.ToStringImp(indent + 1));
            }

            sb.Append($"{i}</{Name}>\n");
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImp(0);
        }
    }

    class HtmlBuilder
    {
        private readonly string _rootName;

        HtmlElement root = new HtmlElement();

        public HtmlBuilder(string rootName)
        {
            _rootName = rootName;
            root.Name = _rootName;
        }

        // not fluent
        public void AddChild(string childName, string childText)
        {
            var element = new HtmlElement(childName, childText);
            root.Elements.Add(element);
        }

        public HtmlBuilder AddChildFluent(string childName, string childText)
        {
            var element = new HtmlElement(childName, childText);
            root.Elements.Add(element);
            return this;
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear()
        {
            root = new HtmlElement { Name = _rootName };
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // if you want to build a simple HTML paragraph using StringBuilder
            var hello = "hello";
            var sb = new StringBuilder();
            sb.Append("<p>");
            sb.Append(hello);
            sb.Append("</p>");
            Console.WriteLine(sb);

            // now I want an HTML list with 2 words in it
            var words = new[] { "hello", "world" };
            sb.Clear();
            sb.Append("<ul>");
            foreach (var word in words)
            {
                sb.AppendFormat("<li>{0}</li>", word);
            }
            sb.Append("</ul>");
            Console.WriteLine(sb);

            // ordinary non-fluent builder
            var htmlBuilder = new HtmlBuilder("ul");
            htmlBuilder.AddChild("li", "hello");
            htmlBuilder.AddChild("li", "world");
            Console.WriteLine(htmlBuilder);

            // fluent builder
            sb.Clear();
            htmlBuilder.Clear(); // disengage builder from the object it's building, then...
            htmlBuilder.AddChildFluent("li", "hello").AddChildFluent("li", "world");
            Console.WriteLine(htmlBuilder);
        }
    }
}
