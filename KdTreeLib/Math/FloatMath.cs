using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KdTree.Math
{
	public class FloatMath : TypeMath<float>
	{
		static FloatMath()
		{
			// Register this class as the ITypeMath class responsible for float types
			TypeMath<float>.Register(new FloatMath());
		}

		public override int Compare(float a, float b)
		{
			return a.CompareTo(b);
		}

		public override bool AreEqual(float a, float b)
		{
			return a == b;
		}

		public override float MinValue
		{
			get { return float.MinValue; }
		}

		public override float MaxValue
		{
			get { return float.MaxValue; }
		}

		public override float Zero
		{
			get { return 0; }
		}

		public override float NegativeInfinity { get { return float.NegativeInfinity; } }

		public override float PositiveInfinity { get { return float.PositiveInfinity; } }

		public override float Add(float a, float b)
		{
			return a + b;
		}

		public override float Subtract(float a, float b)
		{
			return a - b;
		}

		public override float Multiply(float a, float b)
		{
			return a * b;
		}
	}
}
