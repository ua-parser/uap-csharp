using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using YamlDotNet.Core;

namespace UAParser.Tests
{
    public class ParserTests
    {
        [Fact]
        public void can_get_default_parser()
        {
            Parser parser = Parser.GetDefault();
            Assert.NotNull(parser);
        }

        [Fact]
        public void can_get_parser_from_input()
        {
            string yamlContent = this.GetTestResources("UAParser.Tests.Regexes.regexes.yaml");
            Parser parser = Parser.FromYaml(yamlContent);
            Assert.NotNull(parser);
        }

        [Fact]
        public void can_utilize_regex_timeouts()
        {
            string yamlContent = this.GetTestResources("UAParser.Tests.Regexes.backtracking.yaml");
            Parser parser = Parser.FromYaml(yamlContent, new ParserOptions()
            {
                MaxTimeOut = TimeSpan.FromSeconds(1),
            });

            // this loads a backtracking-sensible regular expression and we'll attempt to match it with
            // a long string that should trigger the backtracking,
            string input = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa>";

            var match = parser.ParseUserAgent(input);
            Assert.Equal(Parser.Other, match.Family);
        }
    }
}
