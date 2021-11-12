using System.Text.RegularExpressions;

namespace UAParser.Abstraction
{
    internal abstract class Template
    {
        protected Template(Regex regex)
        {
            Regex = regex;
        }

        public Regex Regex { get; }
    }
}
