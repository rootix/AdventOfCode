namespace AdventOfCode2023.Days;

public class Day13 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var patterns = Input.Value.SplitByGroup();
        var sum = patterns.Sum(pattern => FindReflectionValue(pattern, false));

        return new ValueTask<string>(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var patterns = Input.Value.SplitByGroup();
        var sum = patterns.Sum(pattern => FindReflectionValue(pattern, true));

        return new ValueTask<string>(sum.ToString());
    }

    private static int FindReflectionValue(string pattern, bool smudge)
    {
        var patternLines = pattern.SplitByLine();
        for (var col = 0; col < patternLines[0].Length - 1; col++)
        {
            var leftIndex = col;
            var rightIndex = col + 1;
            var diff = 0;
            while (leftIndex >= 0 && rightIndex < patternLines[0].Length)
            {
                diff += patternLines.Count(row => row[leftIndex] != row[rightIndex]);

                leftIndex -= 1;
                rightIndex += 1;
            }

            if (diff == (smudge ? 1 : 0))
            {
                return col + 1;
            }
        }

        for (var row = 0; row < patternLines.Length - 1; row++)
        {
            var diff = 0;
            var upperIndex = row;
            var lowerIndex = row + 1;

            while (upperIndex >= 0 && lowerIndex < patternLines.Length)
            {
                var upperRow = patternLines[upperIndex];
                var lowerRow = patternLines[lowerIndex];

                for (var col = 0; col < upperRow.Length; col++)
                {
                    if (upperRow[col] != lowerRow[col])
                    {
                        diff += 1;
                    }
                }

                upperIndex -= 1;
                lowerIndex += 1;
            }

            if (diff == (smudge ? 1 : 0))
            {
                return (row + 1) * 100;
            }
        }

        return 0;
    }
}
