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
                MatchTimeOut = TimeSpan.FromSeconds(1),
            });

            // this loads a backtracking-sensible regular expression and we'll attempt to match it with
            // a long string that should trigger the backtracking,
            string input = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa>";

            DateTime start = DateTime.UtcNow;
            var match = parser.ParseUserAgent(input);
            Assert.Equal(Parser.Other, match.Family);
            var duration = DateTime.UtcNow.Subtract(start);

            // without the match timeout in place, the regex will do massive backtracking and would run for
            // a very long time (at least on my machine). I will attempt to assert on the duration
            // even though I realize that this is potentially a brittle approach
            Assert.True(duration < TimeSpan.FromSeconds(3), $"The match takes longer than 3 seconds (took {duration}). The MatchTimeOut should have stopped it at 1 second, but this may just be a brittle test due to e.g. shared resources on a CI server");
        }
    }
}
