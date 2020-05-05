using System.Collections.Generic;
using System.Text.RegularExpressions;
using UAParser.Abstraction;

namespace UAParser
{
    internal class OsParser : AbstractParser<OS, OsTemplate>
    {
        public OsParser(IEnumerable<Dictionary<string, string>> maps, ParserOptions options, OS defaultValue = default)
            : base (maps, options, defaultValue)
        { }

        protected override OsTemplate InitializeTemplate(IDictionary<string, string> map)
        {
            var regex = Regex(map, "OS");
            var os = map.Find("os_replacement");
            var v1 = map.Find("os_v1_replacement");
            var v2 = map.Find("os_v2_replacement");
            var v3 = map.Find("os_v3_replacement");
            var v4 = map.Find("os_v4_replacement");
            return new OsTemplate(regex, os, v1, v2, v3, v4);
        }

        protected override OS Matcher(string input, OsTemplate template)
        {
            var match = Match(input, template.Regex);
            if (match != null && match.Success)
            {
                string family = null, major = null, minor = null, patch = null, patchMinor = null;
                using (var num = Generate(1, x => ++x))
                {
                    // For variable replacements to be consistent the order of the linq statements are important ($1
                    // is only available to the first 'from X in Replace(..)' and so forth) so a a bit of conditional
                    // is required to get the creations to work. This is backed by unit tests
                    if (template.MajorVersionReplacement == "$1")
                    {
                        if (template.MinorVersionReplacement == "$2")
                        {
                            major = Replace(match, num, template.MajorVersionReplacement, "$1");
                            minor = Replace(match, num, template.MinorVersionReplacement, "$2");
                            patch = Replace(match, num, template.PatchVersionReplacement, "$3");
                            patchMinor = Replace(match, num, template.PatchMinorVersionReplacement, "$4");
                            family = Replace(match, num, template.OsNameReplacement, "$5");

                            return new OS(family, major, minor, patch, patchMinor);
                        }

                        major = Replace(match, num, template.MajorVersionReplacement, "$1");
                        family = Replace(match, num, template.OsNameReplacement, "$2");
                        minor = Replace(match, num, template.MinorVersionReplacement, "$3");
                        patch = Replace(match, num, template.PatchVersionReplacement, "$4");
                        patchMinor = Replace(match, num, template.PatchMinorVersionReplacement, "$5");

                        return new OS(family, major, minor, patch, patchMinor);
                    }

                    family = Replace(match, num, template.OsNameReplacement, "$1");
                    major = Replace(match, num, template.MajorVersionReplacement, "$2");
                    minor = Replace(match, num, template.MinorVersionReplacement, "$3");
                    patch = Replace(match, num, template.PatchVersionReplacement, "$4");
                    patchMinor = Replace(match, num, template.PatchMinorVersionReplacement, "$5");

                    return new OS(family, major, minor, patch, patchMinor);
                }
            }

            return default;
        }
    }
}
