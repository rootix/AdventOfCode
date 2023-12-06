namespace AdventOfCode2023;

public static class Extensions
{
    public static int Multiply (this IEnumerable<int> items)
    {
        return items.Aggregate(1, (a, b) => a * b);
    }
}
