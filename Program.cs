namespace Combinations;

/// <summary>
/// This program generates all possible pairs of names from a given list of name pairs.
/// The pairs will be sorted such that the second item of each pair is the first item of
/// the next pair, or the other way around, in case a matching pair could not be found.
/// </summary>
internal class Program
{
	/// <summary>
	/// List of name pairs to be used for generating combinations.
	/// </summary>
	static readonly List<ValueTuple<string, string>> pairs =
	[
		("Greeshma", "Hemant"),
		("Varsha", "Shishira"),
		("Sharat", "Vasant"),
	];

	/// <summary>
	/// Main method to generate and display all possible pairs of names.
	/// </summary>
	/// <param name="args"></param>
	static void Main(string[] args)
	{
		var pairs = new List<ValueTuple<string, string>>();
		var used = new List<ValueTuple<string, string>>();
		foreach (var outerPair in Program.pairs)
		{
			foreach (var innerPair in Program.pairs.Where(x => x != outerPair))
			{
				var candidates = new ValueTuple<string, string>[]
				{
					(innerPair.Item1, outerPair.Item1),
					(innerPair.Item1, outerPair.Item2),
					(innerPair.Item2, outerPair.Item1),
					(innerPair.Item2, outerPair.Item2)
				};
				foreach (var tuple in candidates)
				{
					if (!used.HasIgnoreOrder(tuple))
					{
						pairs.Add(tuple);
						used.Add(tuple);
					}
				}
			}
		}
		Console.WriteLine("Complete list:");
		Console.WriteLine("------------------------------------");
		foreach (var tuple in SortPairs([.. Program.pairs.Union(pairs)]))
		{
			Console.WriteLine($"{tuple.Item1} - {tuple.Item2}");
		}
	}

	/// <summary>
	/// Sorts the pairs such that the second item of each pair is the first item of the next pair.
	/// </summary>
	/// <param name="list">The list of pairs to be sorted.</param>
	/// <returns></returns>
	public static IEnumerable<ValueTuple<string, string>> SortPairs(IList<ValueTuple<string, string>> pairs)
	{
		if (pairs.Count < 2)
		{
			return pairs;
		}
		var sorted = new List<ValueTuple<string, string>>();
		var used = new List<ValueTuple<string, string>>();
		while (sorted.Count < pairs.Count)
		{
			var last = sorted.LastOrDefault();
			if (last == default)
			{
				last = pairs.First();
			}
			var next = pairs.FirstOrDefault(x => x.Item1 == last.Item2 && !used.HasIgnoreOrder(x));
			if (next == default)
			{
				next = pairs.FirstOrDefault(x => x.Item2 == last.Item2 && !used.HasIgnoreOrder(x));
				if (next != default)
				{
					next = (next.Item2, next.Item1);
				}
			}
			if (next == default)
			{
				next = pairs.FirstOrDefault(x => !used.HasIgnoreOrder(x));
				if (next.Item1 == last.Item1 || next.Item2 == last.Item2)
				{
					next = (next.Item2, next.Item1);
				}
				else
				{
					var first = sorted.FirstOrDefault();
					if (first != default && (next.Item1 == first.Item1 || next.Item2 == first.Item2))
					{
						next = (next.Item2, next.Item1);
					}
					sorted.Insert(0, next);
					used.Add(next);
					continue;
				}
			}
			sorted.Add(next);
			used.Add(next);
		}
		return sorted;
	}
}

/// <summary>
/// Extension methods for IEnumerable.
/// </summary>
public static class IEnumerableExtensions
{
	/// <summary>
	/// Checks if the source collection contains the tuple or its reverse.
	/// </summary>
	/// <param name="source"></param>
	/// <param name="tuple"></param>
	/// <returns></returns>
	public static bool HasIgnoreOrder(this IEnumerable<ValueTuple<string, string>> source, ValueTuple<string, string> tuple)
	{
		return source.Any(x => x == tuple || x == (tuple.Item2, tuple.Item1));
	}
}