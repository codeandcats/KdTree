namespace KdTree
{
	public interface IBundle<TKey>
	{
		TKey this[int index] { get; set; }
		int Length { get; }
	}
}