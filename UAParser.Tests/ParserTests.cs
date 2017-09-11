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
  }
}
