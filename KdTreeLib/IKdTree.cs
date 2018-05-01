using System.Collections.Generic;

namespace KdTree
{
	public interface IKdTree<TKey, TValue, TKeyBundle> : IEnumerable<KdTreeNode<TKey, TValue, TKeyBundle>>
		where TKeyBundle : IBundle<TKey>
	{
		bool Add(TKeyBundle point, TValue value);

		bool TryFindValueAt(TKeyBundle point, out TValue value);

		TValue FindValueAt(TKeyBundle point);

		bool TryFindValue(TValue value, out TKeyBundle point);

		TKeyBundle FindValue(TValue value);

		KdTreeNode<TKey, TValue, TKeyBundle>[] RadialSearch(TKeyBundle center, TKey radius, int count);

		void RemoveAt(TKeyBundle point);

		void Clear();

		KdTreeNode<TKey, TValue, TKeyBundle>[] GetNearestNeighbours(TKeyBundle point, int count = int.MaxValue);

		int Count { get; }
	}
}
