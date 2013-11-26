using System;
using System.Collections.Generic;
using System.Linq;
using KdTree.Math;

namespace KdTree
{
	internal class PriorityQueue2<TItem, TPriority>
	{
		public PriorityQueue2(int capacity = 512)
		{
			priorityMath = TypeMath<TPriority>.GetMath();
			maxPriority = priorityMath.Max;
			Initialize(capacity);
		}

		public PriorityQueue2(ITypeMath<TPriority> priorityMath, int capacity = 512)
		{
			this.priorityMath = priorityMath;
			maxPriority = priorityMath.Max;
			Initialize(capacity);
		}

		private ITypeMath<TPriority> priorityMath;

		private TPriority maxPriority;

		private TItem[] items;
		private TPriority[] priorities;

		public int Count { get; private set; }

		private int capacity;

		private void Initialize(int capacity)
		{
			this.capacity = capacity;
			items = new TItem[capacity + 1];
			priorities = new TPriority[capacity + 1];
			priorities[0] = maxPriority;
			items[0] = default(TItem);
		}

		public void Add(TItem item, TPriority priority)
		{
			if (Count++ >= capacity)
			{
				ExpandCapacity();
			}

			priorities[Count] = priority;
			items[Count] = item;
			BubbleUp(Count);
		}
		
		public TItem Remove()
		{
			if (Count == 0)
				return default(TItem);

			TItem element = items[1];
			items[1] = items[Count];
			priorities[1] = priorities[Count];
			items[Count] = default(TItem);
			priorities[Count] = priorityMath.Zero;
			Count--;
			BubbleDown(1);

			return element;
		}

		public TItem GetFront()
		{
			return items[1];
		}

		public TPriority GetMaxPriority()
		{
			return priorities[1];
		}

		private void BubbleDown(int index)
		{
			TItem element = items[index];
			TPriority priority = priorities[index];
			int child;

			for (; index * 2 <= Count; index = child)
			{
				child = index * 2;
				
				if (child != Count)
					if (priorityMath.Compare(priorities[child], priorities[child + 1]) < 0)
						child++;

				if (priorityMath.Compare(priority, priorities[child]) < 0)
				{
					priorities[index] = priorities[child];
					items[index] = items[child];
				}
				else
				{
					break;
				}
			}
			priorities[index] = priority;
			items[index] = element;
		}

		private void BubbleUp(int index)
		{
			TItem element = items[index];
			TPriority priority = priorities[index];

			while (priorityMath.Compare(priorities[index / 2], priority) < 0)
			{
				priorities[index] = priorities[index / 2];
				items[index] = items[index / 2];
				index /= 2;
			}

			priorities[index] = priority;
			items[index] = element;
		}

		private void ExpandCapacity()
		{
			capacity = Count * 2;
			
			TItem[] newItems = new TItem[capacity + 1];
			TPriority[] newPriorities = new TPriority[capacity + 1];

			Array.Copy(items, 0, newItems, 0, items.Length);
			Array.Copy(priorities, 0, newPriorities, 0, priorities.Length);

			items = newItems;
			priorities = newPriorities;
		}

		public void Clear()
		{
			for (int i = 1; i < Count; i++)
				items[i] = default(TItem);

			Count = 0;
		}
	}
}
