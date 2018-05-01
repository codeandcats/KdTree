using BenchmarkDotNet.Attributes;
using KdTree;
using KdTree.Math;
using System;

namespace KdTreeBenchmark
{
    class FloatChebyshevMath : FloatMath
    {
        public override float DistanceSquaredBetweenPoints(float[] a, float[] b)
        {
            var max = 0.0f;
            for (int i = 0; i < a.Length; i++)
            {
                var diff = a[i] - b[i];
                var squared = diff * diff;
                if (max < squared) max = squared;
            }
            return max;
        }
    }

    [MemoryDiagnoser]
    public class KdTreeBenchmark
    {
        private const int NumItems = 10000;
        private const int NumSearchIteration = 1000;
        private const float Min = -1000;
        private const float Max = 1000;

        private static float Next(Random rand, float min, float max) => (float)((max - min) * rand.NextDouble() + min);
        private static float Next(Random rand) => Next(rand, Min, Max);

        [Benchmark]
        public void RadialSearch()
        {
            var rand = new Random(1);
            var tree = new KdTree<float, int>(2, new FloatChebyshevMath());

            for (int i = 0; i < NumItems; i++)
            {
                var x = Next(rand);
                var y = Next(rand);

                tree.Add(new[] { x, y }, i);
            }

            for (int i = 0; i < NumSearchIteration; i++)
            {
                var x = Next(rand);
                var y = Next(rand);
                var radius = Next(rand, 5, 100);

                var result = tree.RadialSearch(new[] { x, y }, radius);
            }
        }
    }
}
