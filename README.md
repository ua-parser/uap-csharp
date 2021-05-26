ua_parser C# Library
======================

This is the CSharp implementation of [ua-parser](https://github.com/tobie/ua-parser). You can find the latest binaries on NuGet [here](https://www.nuget.org/packages/UAParser/).

[![Build status](https://ci.appveyor.com/api/projects/status/ery4ydoxwtokgjkm?svg=true)](https://ci.appveyor.com/project/enemaerke/uap-csharp)

The implementation uses the shared regex patterns and overrides from regexes.yaml (found in [uap-core](https://github.com/ua-parser/uap-core)). The assembly embeds the latest regex patterns (enabled through a git submodule) which are loaded into the default parser. You can create a parser with more updated regex patterns by using the static methods on `Parser` to pass in specific patterns in yaml format.

Build and Run Tests:
------
Make sure you pull down the submodules that includes the yaml files (otherwise you won't be able to compile):

	git submodule update --init --recursive

You can then build and run the tests by invoking the `build.bat` script

    .\build.bat

Update the embedded regexes
------
To pull the latest regexes into the project:

	cd uap-core
	git pull origin master


Usage:
--------
```csharp
  using UAParser;

...

  string uaString = "Mozilla/5.0 (iPhone; CPU iPhone OS 5_1_1 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Version/5.1 Mobile/9B206 Safari/7534.48.3";

  // get a parser with the embedded regex patterns
  var uaParser = Parser.GetDefault();

  // get a parser using externally supplied yaml definitions
  // var uaParser = Parser.FromYaml(yamlString);

  ClientInfo c = uaParser.Parse(uaString);

  Console.WriteLine(c.UA.Family); // => "Mobile Safari"
  Console.WriteLine(c.UA.Major);  // => "5"
  Console.WriteLine(c.UA.Minor);  // => "1"

  Console.WriteLine(c.OS.Family);        // => "iOS"
  Console.WriteLine(c.OS.Major);         // => "5"
  Console.WriteLine(c.OS.Minor);         // => "1"

  Console.WriteLine(c.Device.Family);    // => "iPhone"
```

Authors:
-------

  * Søren Enemærke [@sorenenemaerke](https://twitter.com/sorenenemaerke) / [github](https://github.com/enemaerke)
  * Atif Aziz [@raboof](https://twitter.com/raboof) / [github](https://github.com/atifaziz)
