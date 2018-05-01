using BenchmarkDotNet.Running;

namespace KdTreeBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
#if tre
            var b = new KdTreeBenchmark();
            b.A();
#else
            BenchmarkRunner.Run<KdTreeBenchmark>();
#endif
        }
    }
}
