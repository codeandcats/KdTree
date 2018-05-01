using System;
using System.Text;

namespace KdTree
{
	public partial class KdTree<TKey, TValue, TKeyBundle, TDimension, TNumerics, TMetrics>
	{
		[Serializable]
		public class Node
		{
			public Node(TKeyBundle point, TValue value)
			{
				Point = point;
				Value = value;
			}

			public TKeyBundle Point;
			public TValue Value = default(TValue);

			internal Node LeftChild = null;
			internal Node RightChild = null;

			internal ref Node this[int compare]
			{
				get
				{
					if (compare <= 0)
						return ref LeftChild;
					else
						return ref RightChild;
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
}