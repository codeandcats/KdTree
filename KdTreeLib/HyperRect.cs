namespace KdTree
{
	public struct HyperRect<T, TBundle, TNumerics>
		where TBundle : IBundle<T>
		where TNumerics : struct, INumerics<T>
	{
		public TBundle MinPoint;

		public TBundle MaxPoint;

		public static HyperRect<T, TBundle, TNumerics> Infinite(int dimensions)
		{
			var rect = new HyperRect<T, TBundle, TNumerics>();

			for (var dimension = 0; dimension < dimensions; dimension++)
			{
				rect.MinPoint[dimension] = default(TNumerics).NegativeInfinity;
				rect.MaxPoint[dimension] = default(TNumerics).PositiveInfinity;
			}

			return rect;
		}

		public TBundle GetClosestPoint(TBundle toPoint)
		{
			TBundle closest = default(TBundle);

			for (var dimension = 0; dimension < toPoint.Length; dimension++)
			{
				if (default(TNumerics).Compare(MinPoint[dimension], toPoint[dimension]) > 0)
				{
					closest[dimension] = MinPoint[dimension];
				}
				else if (default(TNumerics).Compare(MaxPoint[dimension], toPoint[dimension]) < 0)
				{
					closest[dimension] = MaxPoint[dimension];
				}
				else
					// Point is within rectangle, at least on this dimension
					closest[dimension] = toPoint[dimension];
			}

			return closest;
		}

		public HyperRect<T, TBundle, TNumerics> Clone()
		{
			var rect = new HyperRect<T, TBundle, TNumerics>
			{
				MinPoint = MinPoint,
				MaxPoint = MaxPoint
			};
			return rect;
		}
	}
}
