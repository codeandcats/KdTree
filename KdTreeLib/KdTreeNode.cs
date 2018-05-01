using System;
using System.Text;

namespace KdTree
{
	[Serializable]
	public class KdTreeNode<TKey, TValue, TKeyBundle>
		where TKeyBundle : IBundle<TKey>
	{
		public KdTreeNode()
		{
		}

		public KdTreeNode(TKeyBundle point, TValue value)
		{
			Point = point;
			Value = value;
		}

		public TKeyBundle Point;
		public TValue Value = default(TValue);

		internal KdTreeNode<TKey, TValue, TKeyBundle> LeftChild = null;
		internal KdTreeNode<TKey, TValue, TKeyBundle> RightChild = null;

		internal KdTreeNode<TKey, TValue, TKeyBundle> this[int compare]
		{
			get
			{
				if (compare <= 0)
					return LeftChild;
				else
					return RightChild;
			}
			set
			{
				if (compare <= 0)
					LeftChild = value;
				else
					RightChild = value;
			}
		}

		public bool IsLeaf
		{
			get
			{
				return (LeftChild == null) && (RightChild == null);
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			for (var dimension = 0; dimension < Point.Length; dimension++)
			{
				sb.Append(Point[dimension].ToString() + "\t");
			}

			if (Value == null)
				sb.Append("null");
			else
				sb.Append(Value.ToString());

			return sb.ToString();
		}
	}
}