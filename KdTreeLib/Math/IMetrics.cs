using System.Collections.Generic;

namespace KdTree
{
	public interface IMetrics<T, TBundle> : IEqualityComparer<TBundle>
		where TBundle : IBundle<T>
	{
		T DistanceSquaredBetweenPoints(TBundle a, TBundle b);
	}
}
