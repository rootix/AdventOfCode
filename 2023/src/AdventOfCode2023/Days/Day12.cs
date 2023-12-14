using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

/// <summary>
/// I had no idea how to tackle this and took the code from https://www.reddit.com/r/adventofcode/comments/18ge41g/comment/kd0u7ej/
/// I researched how i could use Memoization in C# to not have a local dictionary. But it is quite cumbersome when there are multiple parameters, therefore i took the simple route with the local dictionary.
/// </summary>
public class Day12 : DayBase
{
    private readonly Dictionary<string, long> _cache = [];

    public override ValueTask<string> Solve_1()
    {
        var conditionRecords = Input.Value.SplitByLine();
        long possibleArrangements = 0;

        foreach (var conditionRecordString in conditionRecords)
        {
            var split = conditionRecordString.SplitByWhitespace();
            var springs = split[0];
            var groups = split[1].Split(',').Select(int.Parse).ToArray();

            possibleArrangements += CalculatePossibleArrangementUncached(springs, groups);
        }

        return new ValueTask<string>(possibleArrangements.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var conditionRecords = Input.Value.SplitByLine();
        long possibleArrangements = 0;

        foreach (var conditionRecordString in conditionRecords)
        {
            var split = conditionRecordString.SplitByWhitespace();
            var springs = string.Join('?', Enumerable.Repeat(split[0], 5));
            var groups = Enumerable.Repeat(split[1].Split(',').Select(int.Parse), 5).SelectMany(g => g).ToArray();

            possibleArrangements += CalculatePossibleArrangements(springs, groups);
        }

        return new ValueTask<string>(possibleArrangements.ToString());
    }

    private long CalculatePossibleArrangements(string springs, int[] groups)
    {
        var cacheKey = $"{springs},{string.Join(',', groups)}";
        if (_cache.TryGetValue(cacheKey, out var value))
        {
            return value;
        }

        value = CalculatePossibleArrangementUncached(springs, groups);
        _cache[cacheKey] = value;

        return value;
    }

    private long CalculatePossibleArrangementUncached(string springs, int[] groups)
    {
        while (true)
        {
            if (groups.Length == 0)
            {
                return springs.Contains('#') ? 0 : 1; // No more groups to match: if there are no springs left, we have a match
            }

            if (string.IsNullOrEmpty(springs))
            {
                return 0; // No more springs to match, although we still have groups to match
            }

            if (springs.StartsWith('.'))
            {
                springs = springs.Trim('.'); // Remove all dots from the beginning
                continue;
            }

            if (springs.StartsWith('?'))
            {
                return CalculatePossibleArrangements("." + springs[1..], groups) +
                       CalculatePossibleArrangements("#" + springs[1..], groups); // Try both options recursively
            }

            if (!springs.StartsWith('#')) return 0; // Start of a group

            if (groups.Length == 0)
            {
                return 0; // No more groups to match, although we still have a spring in the input
            }

            if (springs.Length < groups[0])
            {
                return 0; // Not enough characters to match the group
            }

            if (springs[..groups[0]].Contains('.'))
            {
                return 0; // Group cannot contain dots for the given length
            }

            if (groups.Length > 1)
            {
                if (springs.Length < groups[0] + 1 || springs[groups[0]] == '#')
                {
                    return 0; // Group cannot be followed by a spring, and there must be enough characters left
                }

                springs = springs[(groups[0] + 1)..]; // Skip the character after the group - it's either a dot or a question mark
                groups = groups[1..];
                continue;
            }

            springs = springs[groups[0]..]; // Last group, no need to check the character after the group
            groups = groups[1..];
        }
    }
}
