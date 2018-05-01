using System.Collections.Generic;

namespace KdTree
{
	public interface INumerics<T> : IEqualityComparer<T>, IComparer<T>
	{
		T MinValue { get; }
		T MaxValue { get; }
		T Add(T a, T b);
		T Subtract(T a, T b);
		T Multiply(T a, T b);
		T Zero { get; }
		T NegativeInfinity { get; }
		T PositiveInfinity { get; }
	}

	public static class NumericsExtensions
	{
		public static T Min<TNumerics, T>(this TNumerics @this, T a, T b)
			where TNumerics : INumerics<T>
			=> (@this.Compare(a, b) < 0) ? a : b;

		public static T Max<TNumerics, T>(this TNumerics @this, T a, T b)
			where TNumerics : INumerics<T>
			=> (@this.Compare(a, b) > 0) ? a : b;
	}
}
