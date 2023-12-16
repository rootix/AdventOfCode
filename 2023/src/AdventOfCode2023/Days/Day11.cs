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

        for (var row = 0; row < lines.Length; row++)
        {
            if (row == 0)
            {
                foreach (var i in Enumerable.Range(0, lines[row].Length))
                {
                    emptyCols.Add(i);
                }
            }

            var emptyRow = true;
            var line = new char[lines[row].Length];
            for (var col = 0; col < lines[row].Length; col++)
            {
                line[col] = lines[row][col];
                if (line[col] != '#') continue;

                stars.Add(new Coordinate2D(row, col));
                emptyRow = false;
                emptyCols.Remove(col);
            }

            if (emptyRow)
            {
                emptyRows.Add(row);
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

                var minCol = Math.Min(from.Col, to.Col);
                var maxCol = Math.Max(from.Col, to.Col);
                var minRow = Math.Min(from.Row, to.Row);
                var maxRow = Math.Max(from.Row, to.Row);

                long distance = maxCol - minCol + emptyColumns.Count(c => c > minCol && c < maxCol) * expansion;
                distance += maxRow - minRow + emptyRows.Count(r => r > minRow && r < maxRow) * expansion;

                shortestDistances += distance;
            }
        }

        return shortestDistances;
    }
}
