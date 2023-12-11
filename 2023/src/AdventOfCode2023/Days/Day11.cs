namespace AdventOfCode2023.Days;

public class Day11 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var (stars, emptyRows, emptyColumns) = ParseInput();
        var shortestDistances = CalculateShortestDistances(stars, emptyRows, emptyColumns, 1);

        return new ValueTask<string>(shortestDistances.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var (stars, emptyRows, emptyColumns) = ParseInput();
        var shortestDistances = CalculateShortestDistances(stars, emptyRows, emptyColumns, 1_000_000 - 1);

        return new ValueTask<string>(shortestDistances.ToString());
    }

    private (Coordinate2D[] Stars, int[] EmptyRows, int[] EmptyColumns) ParseInput()
    {
        var lines = Input.Value.SplitByLine();
        var stars = new List<Coordinate2D>();
        var emptyRows = new HashSet<int>();
        var emptyCols = new HashSet<int>();

        for (var y = 0; y < lines.Length; y++)
        {
            if (y == 0)
            {
                foreach (var i in Enumerable.Range(0, lines[y].Length))
                {
                    emptyCols.Add(i);
                }
            }

            var emptyRow = true;
            var line = new char[lines[y].Length];
            for (var x = 0; x < lines[y].Length; x++)
            {
                line[x] = lines[y][x];
                if (line[x] != '#') continue;

                stars.Add(new Coordinate2D(y, x));
                emptyRow = false;
                emptyCols.Remove(x);
            }

            if (emptyRow)
            {
                emptyRows.Add(y);
            }
        }

        return ([.. stars], [.. emptyRows], [.. emptyCols]);
    }

    private static long CalculateShortestDistances(Coordinate2D[] stars, int[] emptyRows, int[] emptyColumns, int expansion)
    {
        long shortestDistances = 0;
        for (var i = 0; i < stars.Length; i++)
        {
            for (var j = i + 1; j < stars.Length; j++)
            {
                var from = stars[i];
                var to = stars[j];

                var minX = Math.Min(from.X, to.X);
                var maxX = Math.Max(from.X, to.X);
                var minY = Math.Min(from.Y, to.Y);
                var maxY = Math.Max(from.Y, to.Y);

                long distance = maxX - minX + emptyColumns.Count(c => c > minX && c < maxX) * expansion;
                distance += maxY - minY + emptyRows.Count(r => r > minY && r < maxY) * expansion;

                shortestDistances += distance;
            }
        }

        return shortestDistances;
    }
}
