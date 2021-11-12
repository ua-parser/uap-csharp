using BenchmarkDotNet.Running;

namespace UAParser.Tests.Benchmark
{
    class Program
    {
        static void Main(string[] args)
            => BenchmarkRunner.Run<UAParserTests>();
    }
}
