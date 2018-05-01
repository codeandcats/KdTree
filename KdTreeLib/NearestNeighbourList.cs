using System;

namespace KdTree
{
	public interface INearestNeighbourList<TItem, TDistance>
	{
		bool Add(TItem item, TDistance distance);
		TItem GetFurtherest();
		TItem RemoveFurtherest();

		int MaxCapacity { get; }
		int Count { get; }
	}

	public class NearestNeighbourList<TItem, TDistance, TNumerics> : INearestNeighbourList<TItem, TDistance>
		where TNumerics : struct, INumerics<TDistance>
	{
		public NearestNeighbourList(int maxCapacity)
		{
			this.maxCapacity = maxCapacity;

			queue = new PriorityQueue<TItem, TDistance, TNumerics>(maxCapacity);
		}

		public NearestNeighbourList()
		{
			this.maxCapacity = int.MaxValue;

			queue = new PriorityQueue<TItem, TDistance, TNumerics>();
		}

		private PriorityQueue<TItem, TDistance, TNumerics> queue;

		private int maxCapacity;
		public int MaxCapacity { get { return maxCapacity; } }

		public int Count { get { return queue.Count; } }

		public bool Add(TItem item, TDistance distance)
		{
			if (queue.Count >= maxCapacity)
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

		public TItem GetFurtherest()
		{
			if (Count == 0)
				throw new Exception("List is empty");
			else
				return queue.GetHighest();
		}

		public TDistance GetFurtherestDistance()
		{
			if (Count == 0)
				throw new Exception("List is empty");
			else
				return queue.GetHighestPriority();
		}

		public TItem RemoveFurtherest()
		{
			return queue.Dequeue();
		}

		public bool IsCapacityReached
		{
			get { return Count == MaxCapacity; }
		}
	}
}
