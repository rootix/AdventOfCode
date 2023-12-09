namespace AdventOfCode2023;

public static class Extensions
{
    public static int Multiply(this IEnumerable<int> items) => items.Aggregate(1, (a, b) => a * b);

    public static string[] SplitByLine(this string input) => input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    public static string[] SplitByWhitespace(this string input) => input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    public static string[] SplitByGroup(this string input) => input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries);
}
