public class Tuple<T1, T2>
{
    /// Gets the first.
    public T1 First { get; private set; }
    /// Gets the second.
    public T2 Second { get; private set; }

    /// Initializes a new instance of the <see cref="Tuple{T1, T2}"/> class.
    /// <param name="first">The first.</param>
    /// <param name="second">The second.</param>
    internal Tuple(T1 first, T2 second)
	{
		First = first;
		Second = second;
	}
}