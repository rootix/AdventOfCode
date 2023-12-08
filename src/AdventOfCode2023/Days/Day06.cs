namespace AdventOfCode2023.Days;

public class Day06 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var parts = Input.Value.SplitByLine();
        var times = parts[0].SplitByWhitespace()
            .Where(t => t.All(char.IsDigit))
            .Select(long.Parse)
            .ToArray();
        var distances = parts[1].SplitByWhitespace()
            .Where(t => t.All(char.IsDigit))
            .Select(long.Parse)
            .ToArray();

        var result = times.Select((t, idx) => GetWinningCount(t, distances[idx])).Multiply();

        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var parts = Input.Value.SplitByLine();
        var time = long.Parse(parts[0].Replace(" ", string.Empty).Split(':')[1]);
        var distance = long.Parse(parts[1].Replace(" ", string.Empty).Split(':')[1]);

        var winningCount = GetWinningCount(time, distance);

        return new ValueTask<string>(winningCount.ToString());
    }

    private static int GetWinningCount(long time, long distance)
    {
        var winningCount = 0;
        for (var j = 0; j < time; j++)
        {
            var distanceTraveled = j * (time - j);
            if (distanceTraveled > distance)
            {
                winningCount++;
            }
        }

        return winningCount;
    }
}
