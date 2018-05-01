using System.Collections.Generic;
using System.Linq;

namespace KdTree
{
	public partial class NearestNeighbourList<TItem, TDistance, TNumerics>
		where TNumerics : struct, INumerics<TDistance>
	{
		public interface INearestNeighbourList
		{
			bool Add(TItem item, TDistance distance);
			TDistance FurtherestDistance { get; }
			bool IsFull { get; }
		}

		public class UnlimitedList : INearestNeighbourList
		{
			List<(TItem, TDistance)> _items = new List<(TItem, TDistance)>();

			public TDistance FurtherestDistance { get; private set; }

			public bool IsFull => false;

			public bool Add(TItem item, TDistance distance)
			{
				if (default(TNumerics).Compare(FurtherestDistance, distance) < 0) FurtherestDistance = distance;
				_items.Add((item, distance));
				return true;
			}

			public TItem[] GetSortedArray() => _items.OrderBy(x => x.Item2, numerics).Select(x => x.Item1).ToArray();

			private static readonly INumerics<TDistance> numerics = default(TNumerics);
		}

		public class List : INearestNeighbourList
		{
			public List(int maxCapacity)
			{
				MaxCapacity = maxCapacity;
				queue = maxCapacity == int.MaxValue
					? new PriorityQueue<TItem, TDistance, TNumerics>()
					: new PriorityQueue<TItem, TDistance, TNumerics>(maxCapacity);
			}

			public List()
			{
				MaxCapacity = int.MaxValue;
				queue = new PriorityQueue<TItem, TDistance, TNumerics>();
			}

			private PriorityQueue<TItem, TDistance, TNumerics> queue;
			public int MaxCapacity { get; }

			public int Count { get { return queue.Count; } }

			public bool Add(TItem item, TDistance distance)
			{
				if (queue.Count >= MaxCapacity)
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
			public bool IsFull => Count == MaxCapacity;

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

		public static NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.List CreateNearestNeighbourList(int maxCapacity)
			=> new NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.List(maxCapacity);

		public static NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.UnlimitedList CreateUnlimitedList()
			=> new NearestNeighbourList<(TKeyBundle Key, TValue Value), TKey, TNumerics>.UnlimitedList();
	}
}