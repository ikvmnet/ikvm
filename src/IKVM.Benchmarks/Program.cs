using BenchmarkDotNet.Running;

namespace IKVM.Benchmarks
{

    public static class Program
    {

        public static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(Program).Assembly);
        }

    }

}
