using System.Collections.Generic;

namespace KdTree
{
	public interface IKdTree<TKey, TValue, TKeyBundle> : IEnumerable<(TKeyBundle Point, TValue Value)>
		where TKeyBundle : IBundle<TKey>
	{
		bool Add(TKeyBundle point, TValue value);

		bool TryFindValueAt(TKeyBundle point, out TValue value);

		TValue FindValueAt(TKeyBundle point);

		bool TryFindValue(TValue value, out TKeyBundle point);

		TKeyBundle FindValue(TValue value);

		void RadialSearch(TKeyBundle center, TKey radius, INearestNeighbourList<(TKeyBundle Key, TValue Value), TKey> results);

		void RemoveAt(TKeyBundle point);

		void Clear();

		void GetNearestNeighbours(TKeyBundle point, INearestNeighbourList<(TKeyBundle Key, TValue Value), TKey> results);

		int Count { get; }
	}
}
