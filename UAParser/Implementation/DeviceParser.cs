using System.Collections.Generic;
using System.Text.RegularExpressions;
using UAParser.Abstraction;

namespace UAParser
{
    internal class DeviceParser : AbstractParser<Device, DeviceTemplate>
    {
        public DeviceParser(IEnumerable<Dictionary<string, string>> maps, ParserOptions options, Device defaultValue = default)
            : base(maps, options, defaultValue)
        { }

        protected override DeviceTemplate InitializeTemplate(IDictionary<string, string> map)
        {
            var regex = Regex(map, "Device", map.Find("regex_flag"));
            var device = map.Find("device_replacement");
            var brand = map.Find("brand_replacement");
            var model = map.Find("model_replacement");
            return new DeviceTemplate(regex, device, brand, model);
        }

        protected override Device Matcher(string input, DeviceTemplate template)
        {
            var match = Match(input, template.Regex);
            if (match != null && match.Success)
            {
                using (var num = Generate(1, x => ++x))
                {
                    var device = ReplaceAll(match, template.DeviceReplacement, num);
                    var brand = ReplaceAll(match, template.BrandReplacement, num);
                    var model = ReplaceAll(match, template.ModelReplacement, num);
                    return new Device(device, brand, model);
                }                    
            }

            return default;
        }

        private string ReplaceAll(Match m, string replacement, IEnumerator<int> num)
        {
            if (replacement == null)
                return Select(x => x, m, num);

            var finalString = replacement;
            if (finalString.Contains("$"))
            {
                var groups = m.Groups;
                for (var i = 0; i < _allReplacementTokens.Length; i++)
                {
                    var tokenNumber = i + 1;
                    var token = _allReplacementTokens[i];
                    if (finalString.Contains(token))
                    {
                        var replacementText = string.Empty;
                        Group group;
                        if (tokenNumber <= groups.Count && (group = groups[tokenNumber]).Success)
                            replacementText = group.Value;

                        finalString = ReplaceFunction(finalString, replacementText, token);
                    }
                    if (!finalString.Contains("$"))
                        break;
                }
            }

            return finalString.Trim();
        }

        private static string ReplaceFunction(string replacementString, string matchedGroup, string token)
        {
            return matchedGroup != null
                ? replacementString.ReplaceFirstOccurence(token, matchedGroup)
                : replacementString;
        }
    }
}
