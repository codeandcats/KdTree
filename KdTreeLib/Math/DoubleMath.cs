using System.Runtime.CompilerServices;

namespace KdTree.Math
{
	public struct DoublePair : IBundle<double>
	{
		public double X;
		public double Y;

		public DoublePair(double x, double y) => (X, Y) = (x, y);

		public double this[int index]
		{
			get => Unsafe.Add(ref Unsafe.As<DoublePair, double>(ref this), index);
			set => Unsafe.Add(ref Unsafe.As<DoublePair, double>(ref this), index) = value;
		}

		public int Length => 2;

		public bool Equals(DoublePair other) => X == other.X && Y == other.Y;
		public static bool operator ==(DoublePair a, DoublePair b) => a.X == b.X && a.Y == b.Y;
		public static bool operator !=(DoublePair a, DoublePair b) => a.X != b.X || a.Y != b.Y;
		public override bool Equals(object obj) => obj is DoublePair other && Equals(other);
		public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
	}

	public struct DoubleEuclideanMetic : IMetrics<double, DoublePair>
	{
		public double DistanceSquaredBetweenPoints(DoublePair a, DoublePair b)
		{
			double distance = 0;
			int dimensions = a.Length;

			// Return the absolute distance bewteen 2 hyper points
			for (var dimension = 0; dimension < dimensions; dimension++)
			{
				double distOnThisAxis = a[dimension] - b[dimension];
				double distOnThisAxisSquared = distOnThisAxis * distOnThisAxis;

				distance = distance + distOnThisAxisSquared;
			}

			return distance;
		}

		public bool Equals(DoublePair x, DoublePair y) => x.X == y.X && x.Y == y.Y;
		public int GetHashCode(DoublePair obj) => obj.GetHashCode();
	}

	public struct DoubleMath : INumerics<double>
	{
		public int Compare(double a, double b) => a.CompareTo(b);

		public double MinValue => double.MinValue;

		public double MaxValue => double.MaxValue;

		public double Zero => 0;

		public double NegativeInfinity => double.NegativeInfinity;

		public double PositiveInfinity => double.PositiveInfinity;

		public double Add(double a, double b) => a + b;

		public double Subtract(double a, double b) => a - b;

		public double Multiply(double a, double b) => a * b;

		public bool Equals(double x, double y) => x == y;
		public int GetHashCode(double obj) => obj.GetHashCode();
	}
}
