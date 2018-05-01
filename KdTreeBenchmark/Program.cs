using BenchmarkDotNet.Running;

namespace KdTreeBenchmark
{
	class Program
	{
		static void Main(string[] args)
		{
#if tru
            var b = new KdTreeBenchmark();
            b.RadialSearch();
#else
			BenchmarkRunner.Run<KdTreeBenchmark>();
#endif
		}
	}
}
