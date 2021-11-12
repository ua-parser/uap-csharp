using System.Text.RegularExpressions;
using UAParser.Abstraction;

namespace UAParser
{
    internal class DeviceTemplate : Template
    {
        public DeviceTemplate(Regex regex, string deviceReplacement, string brandReplacement, string modelReplacement)
            : base (regex)
        {
            DeviceReplacement = deviceReplacement;
            BrandReplacement = brandReplacement;
            ModelReplacement = modelReplacement;
        }

        public string DeviceReplacement { get; }

        public string BrandReplacement { get; }

        public string ModelReplacement { get; }
    }
}
