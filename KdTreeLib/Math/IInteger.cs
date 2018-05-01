namespace KdTree
{
	public interface IInteger
	{
		int Value { get; }
	}

	public static class Integer
	{
		public struct _0 : IInteger { public int Value => 0; }
		public struct _1 : IInteger { public int Value => 1; }
		public struct _2 : IInteger { public int Value => 2; }
		public struct _3 : IInteger { public int Value => 3; }
		public struct _4 : IInteger { public int Value => 4; }
		public struct _5 : IInteger { public int Value => 5; }
	}
}
