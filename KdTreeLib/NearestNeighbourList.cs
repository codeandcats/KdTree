using System.Collections.Generic;
using System.Linq;

namespace KdTree
{
	public partial class NearestNeighbourList<TItem, TDistance, TNumerics>
		where TNumerics : struct, INumerics<TDistance>
	{
		private const int DefaultCapacity = 32;

		public interface INearestNeighbourList
		{
			bool Add(TItem item, TDistance distance);
			TDistance FurtherestDistance { get; }
			bool IsFull { get; }
		}

		public class UnlimitedList : INearestNeighbourList
		{
			List<(TItem, TDistance)> _items;

			public UnlimitedList() : this(DefaultCapacity) { }
			public UnlimitedList(int capacity) => _items = new List<(TItem, TDistance)>(capacity);

			public TDistance FurtherestDistance => default(TNumerics).Zero;

			public bool IsFull => false;

			public bool Add(TItem item, TDistance distance)
			{
				_items.Add((item, distance));
				return true;
			}

			public TItem[] GetSortedArray() => _items.OrderBy(x => x.Item2, numerics).Select(x => x.Item1).ToArray();

			private static readonly INumerics<TDistance> numerics = default(TNumerics);
		}

		public class List : INearestNeighbourList
		{
			public List(int maxCount, int capacity)
			{
				MaxCount = maxCount;
				queue = new PriorityQueue<TItem, TDistance, TNumerics>(capacity);
			}

			public List(int maxCount) : this(maxCount, DefaultCapacity) { }
			public List() : this(int.MaxValue, DefaultCapacity) { }

			private PriorityQueue<TItem, TDistance, TNumerics> queue;
			public int MaxCount { get; }

			public int Count { get { return queue.Count; } }

			public bool Add(TItem item, TDistance distance)
			{
				if (queue.Count >= MaxCount)
				{
					// If the distance of this item is less than the distance of the last item
					// in our neighbour list then pop that neighbour off and push this one on
					// otherwise don't even bother adding this item
					if (default(TNumerics).Compare(distance, queue.GetHighestPriority()) < 0)
					{
						queue.Dequeue();
						queue.Enqueue(item, distance);
						return true;
					}
					else
						return false;
				}
				else
				{
					queue.Enqueue(item, distance);
					return true;
				}
			}

			public TDistance FurtherestDistance => queue.GetHighestPriority();
			public bool IsFull => Count == MaxCount;

			public TItem RemoveFurtherest()
			{
				return queue.Dequeue();
			}

			public TItem[] GetSortedArray()
			{
				var count = Count;
				var neighbourArray = new TItem[count];

				for (var index = 0; index < count; index++)
				{
					var n = RemoveFurtherest();
					neighbourArray[count - index - 1] = n;
				}

				return neighbourArray;
			}
		}
	}

	public partial class KdTree<TKey, TValue, TKeyBundle, TDimension, TNumerics, TMetrics>
	{
		public static NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.List CreateNearestNeighbourList()
			=> new NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.List();

		public static NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.List CreateNearestNeighbourList(int maxCount)
			=> new NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.List(maxCount);

		public static NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.List CreateNearestNeighbourList(int maxCount, int capacity)
			=> new NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.List(maxCount, capacity);

		public static NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.UnlimitedList CreateUnlimitedList()
			=> new NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.UnlimitedList();

		public static NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.UnlimitedList CreateUnlimitedList(int capacity)
			=> new NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.UnlimitedList(capacity);
	}
}