namespace AdventOfCode2023;

public static class Extensions
{
    public static int Multiply(this IEnumerable<int> items) => items.Aggregate(1, (a, b) => a * b);

    public static string[] SplitByLine(this string input) => input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    public static string[] SplitByWhitespace(this string input) => input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    public static string[] SplitByGroup(this string input) => input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries);

    public static char[][] ToGrid(this string input) => input.SplitByLine().Select(s => s.ToCharArray()).ToArray();

    public static int[][] ToIntGrid(this string input) => input.SplitByLine().Select(s => s.ToCharArray().Where(char.IsDigit).Select(c => c - '0').ToArray()).ToArray();

    public static string ReplaceFirstOccurence(this string input, char toReplace, char replaceWith)
    {
        var index = input.IndexOf(toReplace);
        return index < 0 ? input : input.Remove(index, 1).Insert(index, replaceWith.ToString());
    }
}
