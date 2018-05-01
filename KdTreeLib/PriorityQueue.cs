using System;

namespace KdTree
{
	public class PriorityQueue<TItem, TPriority, TNumerics>
		where TNumerics : struct, INumerics<TPriority>
	{
		struct ItemPriority
		{
			public TItem Item;
			public TPriority Priority;
			public ItemPriority(TItem item, TPriority priority) => (Item, Priority) = (item, priority);
		}

		private ItemPriority[] _items;

		public PriorityQueue()
			: this(4)
		{
		}

		public PriorityQueue(int capacity)
		{
			_items = new ItemPriority[capacity];
			Count = 0;
		}

		private bool IsHigherPriority(int left, int right)
		{
			return default(TNumerics).Compare(_items[left].Priority, _items[right].Priority) > 0;
		}

		private void Percolate(int index)
		{
			if (index >= Count || index < 0)
				return;
			var parent = (index - 1) / 2;
			if (parent < 0 || parent == index)
				return;

			if (IsHigherPriority(index, parent))
			{
				var temp = _items[index];
				_items[index] = _items[parent];
				_items[parent] = temp;
				Percolate(parent);
			}
		}

		private void Heapify()
		{
			Heapify(0);
		}

		private void Heapify(int index)
		{
			if (index >= Count || index < 0)
				return;

			var left = 2 * index + 1;
			var right = 2 * index + 2;
			var first = index;

			if (left < Count && IsHigherPriority(left, first))
				first = left;
			if (right < Count && IsHigherPriority(right, first))
				first = right;
			if (first != index)
			{
				var temp = _items[index];
				_items[index] = _items[first];
				_items[first] = temp;
				Heapify(first);
			}
		}

        public int Count { get; private set; }

        public TItem Peek()
		{
			if (Count == 0)
				throw new InvalidOperationException("HEAP is Empty");

			return _items[0].Item;
		}

		private void RemoveAt(int index)
		{
			_items[index] = _items[--Count];
			_items[Count] = default;
			Heapify();
			if (Count < _items.Length / 4)
			{
				var temp = _items;
				_items = new ItemPriority[_items.Length / 2];
				Array.Copy(temp, 0, _items, 0, Count);
			}
		}

		public TItem Dequeue()
		{
			var result = Peek();
			RemoveAt(0);
			return result;
		}

		public void Enqueue(TItem item, TPriority priority)
		{
			if (Count >= _items.Length)
			{
				var temp = _items;
				_items = new ItemPriority[_items.Length * 2];
				Array.Copy(temp, _items, temp.Length);
			}

			var index = Count++;
			_items[index] = new ItemPriority(item, priority);
			Percolate(index);
		}

		public TItem GetHighest()
		{
			if (Count == 0)
				throw new Exception("Queue is empty");
			else
				return _items[0].Item;
		}

		public TPriority GetHighestPriority()
		{
			if (Count == 0)
				throw new Exception("Queue is empty");
			else
				return _items[0].Priority;
		}
	}
}
