using BenchmarkDotNet.Attributes;
using System;
using System.IO;
using System.Text;

namespace UAParser.Tests.Benchmark
{
    [MemoryDiagnoser]
    public class UAParserTests
    {
        private static readonly string _yamlString = GetTestResources("UAParser.Tests.Benchmark.Regexes.regexes.yaml");
        private static readonly Parser _parser = Parser.GetDefault();

        [Params(
            "Mozilla/5.0 (Linux; Android 8.0.0; SM-G960F Build/R16NW) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.84 Mobile Safari/537.36",
            "Mozilla/5.0 (Linux; Android 7.1.1; G8231 Build/41.2.A.0.219; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/59.0.3071.125 Mobile Safari/537.36",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/69.0.3497.105 Mobile/15E148 Safari/605.1")]
        public string UserAgentString { get; set; }

        [Benchmark]
        public ClientInfo UAParseTest()
        {
            return _parser.Parse(UserAgentString);
        }

        [Benchmark]
        public bool ReadYaml()
        {
            var yamlParser = new MinimalYamlParser(_yamlString);

            return yamlParser != null;
        }

        [Benchmark]
        public Parser CreateParser()
        {
            return Parser.GetDefault();
        }

        internal static string GetTestResources(string name)
        {
            using (var s = typeof(UAParserTests).Assembly.GetManifestResourceStream(name))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not locate an embedded test resource with name: " + name);
                using (var sr = new StreamReader(s, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
