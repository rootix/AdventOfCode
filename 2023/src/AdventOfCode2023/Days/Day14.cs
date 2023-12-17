namespace AdventOfCode2023.Days;

public class Day14 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var platform = Input.Value.ToGrid();
        TiltNorth(platform);
        var sum = CalculateLoad(platform);

        return new ValueTask<string>(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var platform = Input.Value.ToGrid();
        var cycleDetector = new Dictionary<string, int>();
        var cycleDetected = false;

        const int CYCLES = 1_000_000_000;
        for (var cycle = 0; cycle < CYCLES; cycle++)
        {
            TiltNorth(platform);
            TiltWest(platform);
            TiltSouth(platform);
            TiltEast(platform);

            if (cycleDetected) continue;

            var cycleString = string.Join(string.Empty, platform.Select(s => new string(s)));
            if (cycleDetector.TryGetValue(cycleString, out var cycleStart))
            {
                cycleDetected = true;
                cycle = CYCLES - (CYCLES - cycleStart) % (cycle - cycleStart);
            }

            cycleDetector[cycleString] = cycle;
        }

        var sum = CalculateLoad(platform);

        return new ValueTask<string>(sum.ToString());
    }

    private static void TiltNorth(char[][] platform)
    {
        for (var col = 0; col < platform[0].Length; col++)
        {
            var nextEmptyRow = 0;
            for (var row = 0; row < platform.Length; row++)
            {
                switch (platform[row][col])
                {
                    case 'O':
                        platform[row][col] = '.';
                        platform[nextEmptyRow][col] = 'O';
                        nextEmptyRow++;
                        break;
                    case '#':
                        nextEmptyRow = row + 1;
                        break;
                }
            }
        }
    }

    private static void TiltWest(char[][] platform)
    {
        foreach (var row in platform)
        {
            var nextEmptyCol = 0;
            for (var col = 0; col < platform[0].Length; col++)
            {
                switch (row[col])
                {
                    case 'O':
                        row[col] = '.';
                        row[nextEmptyCol] = 'O';
                        nextEmptyCol++;
                        break;
                    case '#':
                        nextEmptyCol = col + 1;
                        break;
                }
            }
        }
    }

    private static void TiltSouth(char[][] platform)
    {
        for (var col = 0; col < platform[0].Length; col++)
        {
            var nextEmptyRow = platform.Length - 1;
            for (var row = platform.Length - 1; row >= 0; row--)
            {
                switch (platform[row][col])
                {
                    case 'O':
                        platform[row][col] = '.';
                        platform[nextEmptyRow][col] = 'O';
                        nextEmptyRow--;
                        break;
                    case '#':
                        nextEmptyRow = row - 1;
                        break;
                }
            }
        }
    }

    private static void TiltEast(char[][] platform)
    {
        foreach (var row in platform)
        {
            var nextEmptyCol = platform[0].Length - 1;
            for (var col = platform[0].Length - 1; col >= 0; col--)
            {
                switch (row[col])
                {
                    case 'O':
                        row[col] = '.';
                        row[nextEmptyCol] = 'O';
                        nextEmptyCol--;
                        break;
                    case '#':
                        nextEmptyCol = col - 1;
                        break;
                }
            }
        }
    }

    private static int CalculateLoad(char[][] platform)
    {
        var sum = 0;
        for (var i = 0; i < platform.Length; i++)
        {
            var row = platform[i];
            sum += row.Count(c => c == 'O') * (platform.Length - i);
        }

        return sum;
    }
}
