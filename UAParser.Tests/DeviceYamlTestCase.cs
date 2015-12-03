using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UAParser.Tests
{
  public class DeviceYamlTestCase : YamlTestCase
  {
    public static DeviceYamlTestCase ReadFromMap(Dictionary<string, string> map)
    {
      DeviceYamlTestCase tc = new DeviceYamlTestCase()
      {
        UserAgent = map["user_agent_string"],
        Family = map["family"],

      };
      return tc;
    }

    public string Family { get; set; }

    public override void Verify(UAParserOutput uaParserOutput)
    {
      Assert.NotNull(uaParserOutput);
      AssertMatch(Family,uaParserOutput.Device.Family,"Family");
    }
  }

  public class OSYamlTestCase : YamlTestCase
  {
    public static OSYamlTestCase ReadFromMap(Dictionary<string, string> map)
    {
      OSYamlTestCase tc = new OSYamlTestCase()
      {
        UserAgent = map["user_agent_string"],
        Family = map["family"],
        Major = map["major"],
        Minor = map["minor"],
        Patch = map["patch"],
        PatchMinor = map["patch_minor"]
      };
      return tc;
    }

    public string Family { get; set; }
    public string Major { get; set; }
    public string Minor { get; set; }
    public string Patch { get; set; }
    public string PatchMinor { get; set; }

    public override void Verify(UAParserOutput uaParserOutput)
    {
      Assert.NotNull(uaParserOutput);
      AssertMatch(Family, uaParserOutput.OS.Family, "Family");
      AssertMatch(Major, uaParserOutput.OS.Major, "Major");
      AssertMatch(Minor, uaParserOutput.OS.Minor, "Minor");
      AssertMatch(Patch, uaParserOutput.OS.Patch, "Patch");
      AssertMatch(PatchMinor, uaParserOutput.OS.PatchMinor, "PatchMinor");
      
    }
  }

  public class UserAgentYamlTestCase : YamlTestCase
  {
    public static UserAgentYamlTestCase ReadFromMap(Dictionary<string, string> map)
    {
      UserAgentYamlTestCase tc = new UserAgentYamlTestCase()
      {
        UserAgent = map["user_agent_string"],
        Family = map["family"],
        Major = map["major"],
        Minor = map["minor"],
        Patch = map["patch"],
      };
      return tc;
    }

    public string Family { get; set; }
    public string Major { get; set; }
    public string Minor { get; set; }
    public string Patch { get; set; }

    public override void Verify(UAParserOutput uaParserOutput)
    {
      Assert.NotNull(uaParserOutput);
      AssertMatch(Family, uaParserOutput.UA.Family, "Family");
      AssertMatch(Major, uaParserOutput.UA.Major, "Major");
      AssertMatch(Minor, uaParserOutput.UA.Minor, "Minor");
      AssertMatch(Patch, uaParserOutput.UA.Patch, "Patch");

    }
  }
}
