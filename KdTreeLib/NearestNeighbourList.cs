using System.Collections.Generic;
using System.Linq;

namespace KdTree
{
	public interface INearestNeighbourList<TItem, TDistance>
	{
		bool Add(TItem item, TDistance distance);
		TDistance FurtherestDistance { get; }
		bool IsFull { get; }
	}

	public class UnlimitedList<TItem, TDistance, TNumerics> : INearestNeighbourList<TItem, TDistance>
		where TNumerics : struct, INumerics<TDistance>
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

		public TItem[] ToSortedArray() => _items.OrderBy(x => x.Item2, new TNumerics()).Select(x => x.Item1).ToArray();
	}

	public class NearestNeighbourList<TItem, TDistance, TNumerics> : INearestNeighbourList<TItem, TDistance>
		where TNumerics : struct, INumerics<TDistance>
	{
		public NearestNeighbourList(int maxCapacity)
		{
			MaxCapacity = maxCapacity;
			queue = maxCapacity == int.MaxValue
				? new PriorityQueue<TItem, TDistance, TNumerics>()
				: new PriorityQueue<TItem, TDistance, TNumerics>(maxCapacity);
		}

		public NearestNeighbourList()
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
	}
}
