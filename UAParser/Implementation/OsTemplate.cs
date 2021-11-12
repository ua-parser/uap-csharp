using System.Text.RegularExpressions;
using UAParser.Abstraction;

namespace UAParser
{
    internal class OsTemplate : Template
    {
        public OsTemplate(Regex regex, string osNameReplacement, string majorVersionReplacement, string minorVersionReplacement,
            string patchVersionReplacement, string patchMinorVersionReplacement)
            : base(regex)
        {
            OsNameReplacement = osNameReplacement;
            MajorVersionReplacement = majorVersionReplacement;
            MinorVersionReplacement = minorVersionReplacement;
            PatchVersionReplacement = patchVersionReplacement;
            PatchMinorVersionReplacement = patchMinorVersionReplacement;
        }

        public string OsNameReplacement { get; }

        public string MajorVersionReplacement { get; }

        public string MinorVersionReplacement { get; }

        public string PatchVersionReplacement { get; }

        public string PatchMinorVersionReplacement { get; }
    }
}
