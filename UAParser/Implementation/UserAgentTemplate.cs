using System.Text.RegularExpressions;
using UAParser.Abstraction;

namespace UAParser
{
    internal class UserAgentTemplate : Template
    {
        public UserAgentTemplate(Regex regex, string familyReplacement, string familyReplacementToken, string majorVersionReplacement,
            string majorVersionReplacementToken, string minorVersionReplacement, string minorVersionReplacementToken, string pathVersionReplacement,
            string pathVersionReplacementToken)
            : base(regex)
        {
            FamilyReplacement = familyReplacement;
            FamilyReplacementToken = familyReplacementToken;
            MajorVersionReplacement = majorVersionReplacement;
            MajorVersionReplacementToken = majorVersionReplacementToken;
            MinorVersionReplacement = minorVersionReplacement;
            MinorVersionReplacementToken = minorVersionReplacementToken;
            PatchVersionReplacement = pathVersionReplacement;
            PatchVersionReplacementToken = pathVersionReplacementToken;
        }

        public string FamilyReplacement { get; }

        public string FamilyReplacementToken { get; }

        public string MajorVersionReplacement { get; }

        public string MajorVersionReplacementToken { get; }

        public string MinorVersionReplacement { get; }

        public string MinorVersionReplacementToken { get; }

        public string PatchVersionReplacement { get; }

        public string PatchVersionReplacementToken { get; }
    }
}
