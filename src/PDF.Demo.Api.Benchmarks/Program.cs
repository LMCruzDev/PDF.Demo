using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace PDF.Demo.Api.Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PdfBenchmarkTests>(
                ManualConfig
                    .Create(DefaultConfig.Instance)
                    .With(ConfigOptions.DisableOptimizationsValidator));
        }
    }
}