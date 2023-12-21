namespace AdventOfCode2023.Days;

public class Day21 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var (garden, startPosition) = ParseInput();
        var reachableGardenPlots = CalculateReachableGardenPlots(garden, startPosition, 64);
        return new ValueTask<string>(reachableGardenPlots.ToString());
    }

    // Part 2 is copied from https://pastebin.com/u/scibuff
    public override ValueTask<string> Solve_2()
    {
        var (garden, startPosition) = ParseInput();
        long[] values = [
            CalculateReachableGardenPlots(garden, startPosition, 65),
            CalculateReachableGardenPlots(garden, startPosition, 65 + 131),
            CalculateReachableGardenPlots(garden, startPosition, 65 + 131 * 2)
        ];

        var poly = GetSimplifiedLagrange(values);
        var target = (26501365 - 65) / 131;
        var result = poly.A * target * target + poly.B * target + poly.C;
        return new ValueTask<string>(result.ToString());
    }

    private (char[][] Garden, Coordinate2D StartPosition) ParseInput()
    {
        var rowStrings = Input.Value.SplitByLine();
        var garden = new char[rowStrings.Length][];
        Coordinate2D startPosition = null!;
        for (var row = 0; row < rowStrings.Length; row++)
        {
            var rowString = rowStrings[row];
            var cols = new char[rowString.Length];
            for (var col = 0; col < rowString.Length; col++)
            {
                if (rowString[col] == 'S')
                {
                    startPosition = new Coordinate2D(row, col);
                }

                cols[col] = rowString[col];
            }

            garden[row] = cols;
        }

        return (garden, startPosition);
    }

    private static long CalculateReachableGardenPlots(char[][] garden, Coordinate2D startPosition, long steps)
    {
        Dictionary<Coordinate2D, long> visited = [];
        Queue<(Coordinate2D Position, long Step)> work = new();
        work.Enqueue((startPosition, 0));

        while (work.Count > 0)
        {
            var s = work.Dequeue();
            if (s.Step > steps) continue;
            if (!IsValidStep(s.Position) || visited.ContainsKey(s.Position)) continue;

            visited[s.Position] = s.Step % 2;

            work.Enqueue((s.Position.Move(Direction.Up), s.Step + 1));
            work.Enqueue((s.Position.Move(Direction.Right), s.Step + 1));
            work.Enqueue((s.Position.Move(Direction.Down), s.Step + 1));
            work.Enqueue((s.Position.Move(Direction.Left), s.Step + 1));
        }

        return visited.Values.Aggregate(0, (p, n) => n == steps % 2 ? p + 1 : p);

        bool IsValidStep(Coordinate2D position)
        {
            var wrappedPos = GetWrappedPosition(position, garden.Length);
            return wrappedPos.Row >= 0 &&
                   wrappedPos.Row < garden.Length &&
                   wrappedPos.Col >= 0 &&
                   wrappedPos.Col < garden[wrappedPos.Row].Length &&
                   garden[wrappedPos.Row][wrappedPos.Col] != '#';
        }
    }

    private static Coordinate2D GetWrappedPosition(Coordinate2D position, int gridLength)
    {
        var row = position.Row % gridLength;
        var col = position.Col % gridLength;
        return new Coordinate2D(row >= 0 ? row : row + gridLength, col >= 0 ? col : col + gridLength);
    }

    private static (double A, double B, double C) GetSimplifiedLagrange(long[] values)
    {
        return (
            values[0] / 2 - values[1] + values[2] / 2,
            -3 * (values[0] / 2) + 2 * values[1] - values[2] / 2,
            values[0]);
    }
}
