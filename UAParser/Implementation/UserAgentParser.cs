using System.Collections.Generic;
using UAParser.Abstraction;

namespace UAParser
{
    internal class UserAgentParser : AbstractParser<UserAgent, UserAgentTemplate>
    {
        public UserAgentParser(IEnumerable<Dictionary<string, string>> maps, ParserOptions options, UserAgent defaultValue = default)
            : base (maps, options, defaultValue)
        { }

        protected override UserAgentTemplate InitializeTemplate(IDictionary<string, string> map)
        {
            var regex = Regex(map, "User agent");
            var family = map.Find("family_replacement");
            var v1 = map.Find("v1_replacement");
            var v2 = map.Find("v2_replacement");
            var v3 = map.Find("v3_replacement");
            return new UserAgentTemplate(regex, family, "$1", v1, "$2", v2, "$3", v3, "$4");
        }

        protected override UserAgent Matcher(string input, UserAgentTemplate template)
        {
            var match = Match(input, template.Regex);
            if (match != null && match.Success)
            {
                using (var num = Generate(1, x => ++x))
                {
                    var family = Replace(match, num, template.FamilyReplacement, template.FamilyReplacementToken);
                    var major = Replace(match, num, template.MajorVersionReplacement, template.MajorVersionReplacementToken);
                    var minor = Replace(match, num, template.MinorVersionReplacement, template.MinorVersionReplacementToken);
                    var patch = Replace(match, num, template.PatchVersionReplacement, template.PatchVersionReplacementToken);
                    return new UserAgent(family, major, minor, patch);
                }
            }

            return default;
        }
    }
}
