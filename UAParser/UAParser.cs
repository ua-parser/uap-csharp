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

namespace UAParser
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using UAParser.Abstraction;

    /// <summary>
    /// Represents a parser of a user agent string
    /// </summary>
    public sealed class Parser
    {
        /// <summary>
        /// The constant string value used to signal an unknown match for a given property or value. This
        /// is by default the string "Other".
        /// </summary>
        public const string Other = "Other";

        private readonly IParser<Device> _deviceParser;
        private readonly IParser<UserAgent> _userAgentParser;
        private readonly IParser<OS> _osParser;

        //private readonly Func<string, OS> _osParser;

        private Parser(MinimalYamlParser yamlParser, ParserOptions options)
        {
            if (options == null) options = new ParserOptions();

            _osParser = new OsParser(yamlParser.ReadMapping("os_parsers"), options, new OS(Other, null, null, null, null));
            _deviceParser = new DeviceParser(yamlParser.ReadMapping("device_parsers"), options, new Device(Other, string.Empty, string.Empty));
            _userAgentParser = new UserAgentParser(yamlParser.ReadMapping("user_agent_parsers"), options, new UserAgent(Other, null, null, null));
        }

        /// <summary>
        /// Returns a <see cref="Parser"/> instance based on the regex definitions in a yaml string
        /// </summary>
        /// <param name="yaml">a string containing yaml definitions of reg-ex</param>
        /// <param name="parserOptions">specifies the options for the parser</param>
        /// <returns>A <see cref="Parser"/> instance parsing user agent strings based on the regexes defined in the yaml string</returns>
        public static Parser FromYaml(string yaml, ParserOptions parserOptions = null)
        {
            return new Parser(new MinimalYamlParser(yaml), parserOptions);
        }

        /// <summary>
        /// Returns a <see cref="Parser"/> instance based on the embedded regex definitions.
        /// <remarks>The embedded regex definitions may be outdated. Consider passing in external yaml definitions using <see cref="Parser.FromYaml"/></remarks>
        /// </summary>
        /// <param name="parserOptions">specifies the options for the parser</param>
        /// <returns></returns>
        public static Parser GetDefault(ParserOptions parserOptions = null)
        {
            using (var stream = typeof(Parser)
#if INTROSPECTION_EXTENSIONS
                                    .GetTypeInfo()
#endif
                                    .Assembly.GetManifestResourceStream("UAParser.regexes.yaml"))
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
                return new Parser(new MinimalYamlParser(reader), parserOptions);
        }

        /// <summary>
        /// Parse a user agent string and obtain all client information
        /// </summary>
        public ClientInfo Parse(string uaString)
        {
            var os = ParseOS(uaString);
            var device = ParseDevice(uaString);
            var ua = ParseUserAgent(uaString);
            return new ClientInfo(uaString, os, device, ua);
        }

        /// <summary>
        /// Parse a user agent string and obtain the OS information
        /// </summary>
        public OS ParseOS(string uaString)
            => _osParser.Parse(uaString);

        /// <summary>
        /// Parse a user agent string and obtain the device information
        /// </summary>
        public Device ParseDevice(string uaString)
            => _deviceParser.Parse(uaString);

        /// <summary>
        /// Parse a user agent string and obtain the UserAgent information
        /// </summary>
        public UserAgent ParseUserAgent(string uaString)
            => _userAgentParser.Parse(uaString);
    }

    internal static class VersionString
    {
        public static string Format(params string[] parts)
        {
            return string.Join(".", parts.Where(v => !string.IsNullOrEmpty(v)).ToArray());
        }
    }
}
