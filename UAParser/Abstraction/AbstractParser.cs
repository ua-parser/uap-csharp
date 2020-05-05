using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UAParser.Abstraction
{
    internal abstract class AbstractParser<TOut, TTemplate> : IParser<TOut>
        where TOut : Part
        where TTemplate : Template
    {
        protected static readonly string[] _allReplacementTokens = new string[]
            { "$1","$2","$3","$4","$5","$6","$7","$8","$91" };

        private readonly ParserOptions _options;

        protected AbstractParser(IEnumerable<Dictionary<string, string>> maps, ParserOptions options, TOut defaultValue)
        {
            _options = options;
            DefaultValue = defaultValue;
            Templates = maps.Select(InitializeTemplate).ToList();
        }

        protected List<TTemplate> Templates { get; }

        protected TOut DefaultValue { get; }

        public virtual TOut Parse(string input)
        {
            foreach (var template in Templates)
            {
                var device = Matcher(input, template);
                if (device != default)
                    return device;
            }

            return DefaultValue;
        }

        protected abstract TOut Matcher(string input, TTemplate template);

        protected virtual Match Match(string input, Regex regex)
        {
#if REGEX_MATCHTIMEOUT
            try
            {
                return regex.Match(input);
            }
            catch (RegexMatchTimeoutException)
            {
                // we'll simply swallow this exception and return the default (non-matched)
                return default;
            }
#else
                    return regex.Match(input);
#endif
        }

        protected abstract TTemplate InitializeTemplate(IDictionary<string, string> map);

        protected Regex Regex(IDictionary<string, string> indexer, string key, string regexFlag = null)
        {
            var pattern = indexer.Find("regex");
            if (pattern == null)
                throw new Exception($"{key} is missing regular expression specification.");

            // Some expressions in the regex.yaml file causes parsing errors
            // in .NET such as the \_ token so need to alter them before
            // proceeding.

            if (pattern.IndexOf(@"\_", StringComparison.Ordinal) >= 0)
                pattern = pattern.Replace(@"\_", "_");

            //Singleline: User agent strings do not contain newline characters. RegexOptions.Singleline improves performance.
            //CultureInvariant: The interpretation of a user agent never depends on the current locale.
            var options = RegexOptions.Singleline | RegexOptions.CultureInvariant;

            if ("i".Equals(regexFlag))
            {
                options |= RegexOptions.IgnoreCase;
            }

#if REGEX_COMPILATION
                if (_options.UseCompiledRegex)
                {
                    options |= RegexOptions.Compiled;
                }
#endif

#if REGEX_MATCHTIMEOUT

                return new Regex(pattern, options, _options.MatchTimeOut);
#else
            return new Regex(pattern, options);
#endif
        }

        protected string Replace(Match m, IEnumerator<int> num, string replacement)
        {
            return replacement != null
                ? Select(_ => replacement, m, num)
                : Select(x => x, m, num);
        }

        protected string Replace(Match m, IEnumerator<int> num, string replacement, string token)
        {
            return replacement != null && replacement.Contains(token)
                 ? Select(s => s != null ? replacement.ReplaceFirstOccurence(token, s) : replacement, m, num)
                 : Replace(m, num, replacement);
        }

        protected string Select(Func<string, string> selector, Match m, IEnumerator<int> num)
        {
            if (!num.MoveNext()) throw new InvalidOperationException();
            var groups = m.Groups; Group group;
            return selector(
                num.Current <= groups.Count && (group = groups[num.Current]).Success
                            ? group.Value.Trim()
                            : null);
        }

        protected IEnumerator<T> Generate<T>(T initial, Func<T, T> next)
        {
            for (var state = initial; ; state = next(state))
                yield return state;
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
