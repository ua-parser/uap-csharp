#region Apache License, Version 2.0
// 
// Copyright 2014 Atif Aziz
// Portions Copyright 2012 Søren Enemærke
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System.Security.Policy;

namespace UAParser
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    
    #endregion

    /// <summary>
    /// Represents the physical device the user agent is using
    /// </summary>
    public sealed class Device
    {
        /// <summary>
        /// Constructs a Device instance
        /// </summary>
        public Device(string family, string brand, string model)
        {
            Family = family.Trim();
            if (brand != null)
                Brand = brand.Trim();
            if (model != null)
                Model = model.Trim();
        }

        /// <summary>
        /// Returns true if the device is likely to be a spider or a bot device
        /// </summary>
        public bool IsSpider { get { return "Spider".Equals(Family, StringComparison.OrdinalIgnoreCase); } }
        /// <summary>
        ///The brand of the device 
        /// </summary>
        public string Brand { get; private set; }
        /// <summary>
        /// The family of the device, if available
        /// </summary>
        public string Family { get; private set; }
        /// <summary>
        /// The model of the device, if available
        /// </summary>
        public string Model { get; private set; }

        /// <summary>
        /// A readable description of the device
        /// </summary>
        public override string ToString()
        {
            return Family;
        }
    }

    /// <summary>
    /// Represents the operating system the user agent runs on
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class OS
    {
        /// <summary>
        /// Constructs an OS instance
        /// </summary>
        public OS(string family, string major, string minor, string patch, string patchMinor)
        {
            Family     = family;
            Major      = major;
            Minor      = minor;
            Patch      = patch;
            PatchMinor = patchMinor;
        }

        /// <summary>
        /// The familiy of the OS
        /// </summary>
        public string Family     { get; private set; }
        /// <summary>
        /// The major version of the OS, if available
        /// </summary>
        public string Major      { get; private set; }
        /// <summary>
        /// The minor version of the OS, if available
        /// </summary>
        public string Minor      { get; private set; }
        /// <summary>
        /// The patch version of the OS, if available
        /// </summary>
        public string Patch      { get; private set; }
        /// <summary>
        /// The minor patch version of the OS, if available
        /// </summary>
        public string PatchMinor { get; private set; }
        /// <summary>
        /// A readable description of the OS
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var version = VersionString.Format(Major, Minor, Patch, PatchMinor);
            return Family + (!string.IsNullOrEmpty(version) ? " " + version : null);
        }
    }

    /// <summary>
    /// Represents a user agent, commonly a browser
    /// </summary>
    public sealed class UserAgent
    {
        /// <summary>
        /// Construct a UserAgent instance 
        /// </summary>
        public UserAgent(string family, string major, string minor, string patch)
        {
            Family = family;
            Major  = major;
            Minor  = minor;
            Patch  = patch;
        }

        /// <summary>
        /// The family of user agent
        /// </summary>
        public string Family { get; private set; }
        /// <summary>
        /// Major version of the user agent, if available
        /// </summary>
        public string Major  { get; private set; }
        /// <summary>
        /// Minor version of the user agent, if available
        /// </summary>
        public string Minor  { get; private set; }
        /// <summary>
        /// Patch version of the user agent, if available
        /// </summary>
        public string Patch  { get; private set; }

        /// <summary>
        /// The user agent as a readbale string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var version = VersionString.Format(Major, Minor, Patch);
            return Family + (!string.IsNullOrEmpty(version) ? " " + version : null);
        }
    }

    static class VersionString
    {
        public static string Format(params string[] parts)
        {
            return string.Join(".", parts.Where(v => !String.IsNullOrEmpty(v)).ToArray());
        }
    }

    /// <summary>
    /// Representing the parse results. Structure of this class aligns with the 
    /// ua-parser-output WebIDL structure defined in this document: https://github.com/ua-parser/uap-core/blob/master/docs/specification.md
    /// </summary>
    public interface IUAParserOutput
    {
        /// <summary>
        /// The user agent string, the input for the UAParser
        /// </summary>
        string String { get; }

        /// <summary>
        /// The OS parsed from the user agent string
        /// </summary>
        // ReSharper disable once InconsistentNaming
        OS OS { get; }
        /// <summary>
        /// The Device parsed from the user agent string
        /// </summary>
        Device Device { get; }
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// The User Agent parsed from the user agent string
        /// </summary>
        UserAgent UA { get; }
    }

    /// <summary>
    /// Represents the user agent client information resulting from parsing
    /// a user agent string
    /// </summary>
    public class ClientInfo : IUAParserOutput
    {
        /// <summary>
        /// The user agent string, the input for the UAParser
        /// </summary>
        public string String { get; private set; }
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// The OS parsed from the user agent string
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public OS OS { get; private set; }

        /// <summary>
        /// The Device parsed from the user agent string
        /// </summary>
        public Device Device { get; private set; }
        /// <summary>
        /// The User Agent parsed from the user agent string
        /// </summary>
        public UserAgent UserAgent { get { return UA; } }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// The User Agent parsed from the user agent string
        /// </summary>
        public UserAgent UA { get; private set; }

        /// <summary>
        /// Constructs an instance of the ClientInfo with results of the user agent string parsing 
        /// </summary>
        public ClientInfo(string inputString, OS os, Device device, UserAgent userAgent)
        {
            String = inputString;
            OS = os;
            Device = device;
            UA = userAgent;
        }

        /// <summary>
        /// A readable description of the user agent client information
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", OS, Device, UA);
        }
    }

    /// <summary>
    /// Represents a parser of a user agent string
    /// </summary>
    public sealed class Parser
    {
        readonly Func<string, OS> _osParser;
        readonly Func<string, Device> _deviceParser;
        readonly Func<string, UserAgent> _userAgentParser;

        Parser(MinimalYamlParser yamlParser)
        {
            const string other = "Other";
            var defaultDevice = new Device(other, "", "");

            _userAgentParser = CreateParser(Read(yamlParser.ReadMapping("user_agent_parsers"), Config.UserAgent), new UserAgent(other, null, null, null));
            _osParser = CreateParser(Read(yamlParser.ReadMapping("os_parsers"), Config.OS), new OS(other, null, null, null, null));
            _deviceParser = CreateParser(Read(yamlParser.ReadMapping("device_parsers"), Config.Device), defaultDevice);
        }

        static IEnumerable<T> Read<T>(IEnumerable<Dictionary<string, string>> entries, Func<Func<string, string>, T> selector)
        {
            return from cm in entries select selector(cm.Find);
        }

        /// <summary>
        /// Returns a <see cref="Parser"/> instance based on the regex definitions in a yaml string
        /// </summary>
        /// <param name="yaml">a string containing yaml definitions of reg-ex</param>
        /// <returns>A <see cref="Parser"/> instance parsing user agent strings based on the regexes defined in the yaml string</returns>
        public static Parser FromYaml(string yaml) { return new Parser(new MinimalYamlParser(yaml)); }
        /// <summary>
        /// Returns a <see cref="Parser"/> instance based on the information in a yaml file
        /// </summary>
        /// <param name="path">the path to a yaml file containing regex definitions</param>
        /// <returns>A <see cref="Parser"/> instance parsing user agent strings based on the regexes defined in the yaml string</returns>
        public static Parser FromYamlFile(string path) { return new Parser(new MinimalYamlParser(File.ReadAllText(path))); }
        /// <summary>
        /// Returns a <see cref="Parser"/> instance based on the embedded regex definitions. 
        /// <remarks>The embedded regex definitions may be outdated. Consider passing in external yaml definitions using <see cref="Parser.FromYaml"/> or
        /// <see cref="Parser.FromYamlFile"/></remarks>
        /// </summary>
        /// <returns></returns>
        public static Parser GetDefault()
        {
            using (var stream = typeof(Parser).Assembly.GetManifestResourceStream("UAParser.regexes.yaml"))
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
                return new Parser(new MinimalYamlParser(reader.ReadToEnd()));
        }

        /// <summary>
        /// Parse a user agent string and obtain all client information
        /// </summary>
        public ClientInfo Parse(string uaString)
        {
            var os     = ParseOS(uaString);
            var device = ParseDevice(uaString);
            var ua     = ParseUserAgent(uaString);
            return new ClientInfo(uaString, os, device, ua);
        }

        /// <summary>
        /// Parse a user agent string and obtain the OS information
        /// </summary>
        public OS ParseOS(string uaString) { return _osParser(uaString); }
        /// <summary>
        /// Parse a user agent string and obtain the device information
        /// </summary>
        public Device ParseDevice(string uaString) { return _deviceParser(uaString); }
        /// <summary>
        /// Parse a user agent string and obtain the UserAgent information
        /// </summary>
        public UserAgent ParseUserAgent(string uaString) { return _userAgentParser(uaString); }

        static Func<string, T> CreateParser<T>(IEnumerable<Func<string, T>> parsers, T defaultValue) where T : class
        {
            return CreateParser(parsers, defaultValue, t => t);
        }

        static Func<string, TResult> CreateParser<T, TResult>(IEnumerable<Func<string, T>> parsers, T defaultValue, Func<T, TResult> selector) where T : class
        {
            parsers = parsers != null ? parsers.ToArray() : Enumerable.Empty<Func<string, T>>();
            return ua => selector(parsers.Select(p => p(ua)).FirstOrDefault(m => m != null) ?? defaultValue);
        }

        static class Config
        {
            // ReSharper disable once InconsistentNaming
            public static Func<string, OS> OS(Func<string, string> indexer)
            {
                var regex = Regex(indexer, "OS");
                var os = indexer("os_replacement");
                var v1 = indexer("os_v1_replacement");
                var v2 = indexer("os_v2_replacement");
                var v3 = indexer("os_v3_replacement");
                var v4 = indexer("os_v4_replacement");
                return Parsers.OS(regex, os, v1, v2, v3, v4);
            }

            public static Func<string, UserAgent> UserAgent(Func<string, string> indexer)
            {
                var regex = Regex(indexer, "User agent");
                var family = indexer("family_replacement");
                var v1 = indexer("v1_replacement");
                var v2 = indexer("v2_replacement");
                var v3 = indexer("v3_replacement");
                return Parsers.UserAgent(regex, family, v1, v2, v3);
            }

            public static Func<string, Device> Device(Func<string, string> indexer)
            {
                var regex = Regex(indexer, "Device", indexer("regex_flag"));
                var device = indexer("device_replacement");
                var brand = indexer("brand_replacement");
                var model = indexer("model_replacement");
                return Parsers.Device(regex, device, brand, model);
            }

            static Regex Regex(Func<string, string> indexer, string key, string regexFlag = null)
            {
                var pattern = indexer("regex");
                if (pattern == null)
                    throw new Exception(String.Format("{0} is missing regular expression specification.", key));

                // Some expressions in the regex.yaml file causes parsing errors 
                // in .NET such as the \_ token so need to alter them before 
                // proceeding.

                if (pattern.IndexOf(@"\_", StringComparison.Ordinal) >= 0)
                    pattern = pattern.Replace(@"\_", "_");

                // TODO: potentially allow parser to specify e.g. to use 
                // compiled regular expressions which are faster but increase 
                // startup time
                 RegexOptions options = RegexOptions.None;
                if ("i".Equals(regexFlag))
                    options |= RegexOptions.IgnoreCase;
                return new Regex(pattern, options);
            }
        }
        
        static class Parsers
        {
            // ReSharper disable once InconsistentNaming
            public static Func<string, OS> OS(Regex regex, string osReplacement, string v1Replacement, string v2Replacement, string v3Replacement, string v4Replacement)
            {
                return Create(regex, from family in Replace(osReplacement, "$1")
                                     from v1 in Replace(v1Replacement, "$2")
                                     from v2 in Replace(v2Replacement, "$3")
                                     from v3 in Replace(v3Replacement, "$4")
                                     from v4 in Replace(v4Replacement, "$5")
                                     select new OS(family, v1, v2, v3, v4));
            }

            public static Func<string, Device> Device(Regex regex, string familyReplacement, string brandReplacement, string modelReplacement)
            {
                return Create(regex, from family in ReplaceAll(familyReplacement)
                                     from brand in ReplaceAll(brandReplacement)
                                     from model in ReplaceAll(modelReplacement)
                                     select new Device(family, brand, model));
            }

            public static Func<string, UserAgent> UserAgent(Regex regex, string familyReplacement, string majorReplacement, string minorReplacement, string patchReplacement)
            {
                return Create(regex, from family in Replace(familyReplacement, "$1")
                                     from v1 in Replace(majorReplacement, "$2")
                                     from v2 in Replace(minorReplacement, "$3")
                                     from v3 in Replace(patchReplacement, "$4")
                                     select new UserAgent(family, v1, v2, v3));
            }

            static Func<Match, IEnumerator<int>, string> Replace(string replacement)
            {
                return replacement != null ? Select(_ => replacement) : Select();
            }

            static Func<Match, IEnumerator<int>, string> Replace(
                string replacement, string token)
            {
                return replacement != null && replacement.Contains(token)
                     ? Select(s => s != null ? replacement.ReplaceFirstOccurence(token, s) : replacement)
                     : Replace(replacement);
            }

            private static readonly string[] AllTokens = new string[]
            {
                "$1","$2","$3","$4","$5","$6","$7","$8","$91",
            };
            
            static Func<Match, IEnumerator<int>, string> ReplaceAll(
                string replacement)
            {
                if (replacement == null)
                    return Select();

                Func<string, string, string, string> replaceFunction = (replacementString, matchedGroup, token) =>
                {
                    return matchedGroup != null
                        ? replacementString.ReplaceFirstOccurence(token, matchedGroup)
                        : replacementString;
                };

                return (m, num) =>
                {
                    var finalString = replacement;
                    if (finalString.Contains("$"))
                    {
                        var groups = m.Groups;
                        for (int i = 0; i < AllTokens.Length; i++)
                        {
                            int tokenNumber = i + 1;
                            string token = AllTokens[i];
                            Group group;
                            if (finalString.Contains(token))
                            {
                                var replacementText = string.Empty;
                                if (tokenNumber <= groups.Count && (group = groups[tokenNumber]).Success)
                                    replacementText = group.Value;

                                finalString = replaceFunction(finalString, replacementText, token);
                            }
                            if (!finalString.Contains("$"))
                                break;
                        }
                    }
                    return finalString;
                };
            }

            static Func<Match, IEnumerator<int>, string> Select() { return Select(v => v); }

            static Func<Match, IEnumerator<int>, T> Select<T>(Func<string, T> selector)
            {
                return (m, num) =>
                {
                    if (!num.MoveNext()) throw new InvalidOperationException();
                    var groups = m.Groups; Group group;
                    return selector(num.Current <= groups.Count && (group = groups[num.Current]).Success
                                    ? group.Value : null);
                };
            }

            static Func<string, T> Create<T>(Regex regex, Func<Match, IEnumerator<int>, T> binder)
            {
                return input =>
                {
                    var m = regex.Match(input);
                    var num = Generate(1, n => n + 1);
                    return m.Success ? binder(m, num) : default(T);
                };
            }

            static IEnumerator<T> Generate<T>(T initial, Func<T, T> next)
            {
                for (var state = initial; ; state = next(state))
                    yield return state;
                // ReSharper disable once FunctionNeverReturns
            }
        }
    }

    static class RegexBinderBuilder
    {
        public static Func<Match, IEnumerator<int>, TResult> SelectMany<T1, T2, TResult>(
            this Func<Match, IEnumerator<int>, T1> binder,
            Func<T1, Func<Match, IEnumerator<int>, T2>> continuation,
            Func<T1, T2, TResult> projection)
        {
            return (m, num) =>
            {
                T1 bound = binder(m, num);
                T2 continued = continuation(bound)(m, num);
                TResult projected = projection(bound, continued);
                return projected;
            };
        }
    }

    static class StringExtensions
    {
        public static string ReplaceFirstOccurence(this string input, string search, string replacement)
        {
            if (input == null) throw new ArgumentNullException("input");
            var index = input.IndexOf(search, StringComparison.Ordinal);
            return index >= 0
                 ? input.Substring(0, index) + replacement + input.Substring(index + search.Length)
                 : input;
        }
    }

    static class DictionaryExtensions
    {
        public static TValue Find<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");
            TValue result;
            return dictionary.TryGetValue(key, out result) ? result : default(TValue);
        }
    }

    /// <summary>
    /// Just enough string parsing to recognize the regexes.yaml file format. Introduced to remove
    /// dependency on large Yaml parsing lib. Note that a unittest ensures compatibility
    /// by ensuring regexes and properties are read similar to using the full yaml lib
    /// </summary>
    internal class MinimalYamlParser
    {
        internal class Mapping
        {
            private Dictionary<string, string> m_lastEntry;

            public Mapping()
            {
                Sequences = new List<Dictionary<string, string>>();
            }

            public List<Dictionary<string, string>> Sequences { get; private set; }

            public void BeginSequence()
            {
                m_lastEntry = new Dictionary<string, string>();
                Sequences.Add(m_lastEntry);
            }

            public void AddToSequence(string key, string value)
            {
                m_lastEntry[key] = value;
            }
        }

        private readonly Dictionary<string, Mapping> m_mappings = new Dictionary<string, Mapping>();

        public MinimalYamlParser(string yamlString)
        {
            ReadIntoMappingModel(yamlString);
        }

        internal IDictionary<string, Mapping> Mappings { get { return m_mappings; } }

        private void ReadIntoMappingModel(string yamlInputString)
        {
            // line splitting using various splitting characters
            string[] lines = yamlInputString.Split(new[] { Environment.NewLine, "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int lineCount = 0;
            Mapping activeMapping = null;

            foreach (var line in lines)
            {
                lineCount++;
                if (line.Trim().StartsWith("#")) //skipping comments
                    continue;
                if (line.Trim().Length == 0)
                    continue;

                //is this a new mapping entity
                if (line[0] != ' ')
                {
                    int indexOfMappingColon = line.IndexOf(':');
                    if (indexOfMappingColon == -1)
                        throw new ArgumentException("YamlParsing: Expecting mapping entry to contain a ':', at line " + lineCount);
                    string name = line.Substring(0, indexOfMappingColon).Trim();
                    activeMapping = new Mapping();
                    m_mappings.Add(name, activeMapping);
                    continue;
                }

                //reading scalar entries into the active mapping
                if (activeMapping == null)
                    throw new ArgumentException("YamlParsing: Expecting mapping entry to contain a ':', at line " + lineCount);

                var seqLine = line.Trim();
                if (seqLine[0] == '-')
                {
                    activeMapping.BeginSequence();
                    seqLine = seqLine.Substring(1);
                }

                int indexOfColon = seqLine.IndexOf(':');
                if (indexOfColon == -1)
                    throw new ArgumentException("YamlParsing: Expecting scalar mapping entry to contain a ':', at line " + lineCount);

                string key = seqLine.Substring(0, indexOfColon).Trim();
                string value = ReadQuotedValue(seqLine.Substring(indexOfColon + 1).Trim());
                activeMapping.AddToSequence(key, value);
            }
        }

        private static string ReadQuotedValue(string value)
        {
            if (value.StartsWith("'") && value.EndsWith("'"))
                return value.Substring(1, value.Length - 2);
            if (value.StartsWith("\"") && value.EndsWith("\""))
                return value.Substring(1, value.Length - 2);
            return value;
        }

        public IEnumerable<Dictionary<string, string>> ReadMapping(string mappingName)
        {
            Mapping mapping;
            if (m_mappings.TryGetValue(mappingName, out mapping))
            {
                foreach (var s in mapping.Sequences)
                {
                    var temp = s;
                    yield return temp;
                }
            }
        }
    }

}
