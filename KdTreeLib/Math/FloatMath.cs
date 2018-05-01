using System;
using System.Runtime.CompilerServices;

namespace KdTree.Math
{
	public struct FloatPair : IBundle<float>, IEquatable<FloatPair>
	{
		public float X;
		public float Y;

		public FloatPair(float x, float y) => (X, Y) = (x, y);

		public float this[int index]
		{
			get => Unsafe.Add(ref Unsafe.As<FloatPair, float>(ref this), index);
			set => Unsafe.Add(ref Unsafe.As<FloatPair, float>(ref this), index) = value;
		}

		public int Length => 2;

		public bool Equals(FloatPair other) => X == other.X && Y == other.Y;
		public static bool operator ==(FloatPair a, FloatPair b) => a.X == b.X && a.Y == b.Y;
		public static bool operator !=(FloatPair a, FloatPair b) => a.X != b.X || a.Y != b.Y;
		public override bool Equals(object obj) => obj is FloatPair other && Equals(other);
		public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
	}

	public struct FloatEuclideanMetic : IMetrics<float, FloatPair>
	{
		public float DistanceSquaredBetweenPoints(FloatPair a, FloatPair b)
		{
			float distance = 0f;
			int dimensions = a.Length;

			// Return the absolute distance bewteen 2 hyper points
			for (var dimension = 0; dimension < dimensions; dimension++)
			{
				float distOnThisAxis = a[dimension] - b[dimension];
				float distOnThisAxisSquared = distOnThisAxis * distOnThisAxis;

				distance = distance + distOnThisAxisSquared;
			}

			return distance;
		}

		public bool Equals(FloatPair x, FloatPair y) => x.X == y.X && x.Y == y.Y;
		public int GetHashCode(FloatPair obj) => obj.GetHashCode();
	}

	public struct FloatMath : INumerics<float>
	{
		public int Compare(float a, float b) => a.CompareTo(b);

		public float MinValue => float.MinValue;

		public float MaxValue => float.MaxValue;

		public float Zero => 0;

		public float NegativeInfinity => float.NegativeInfinity;

		public float PositiveInfinity => float.PositiveInfinity;

		public float Add(float a, float b) => a + b;

		public float Subtract(float a, float b) => a - b;

		public float Multiply(float a, float b) => a * b;

		public bool Equals(float x, float y) => x == y;
		public int GetHashCode(float obj) => obj.GetHashCode();
	}
}
