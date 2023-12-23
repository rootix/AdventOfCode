namespace AdventOfCode2023.Days;

public class Day21 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var garden = ParseInput();
        var reachableGardenPlots = CalculateReachableGardenPlots(garden, 64);
        return new ValueTask<string>(reachableGardenPlots.ToString());
    }

    // Part 2 is copied from https://pastebin.com/u/scibuff
    public override ValueTask<string> Solve_2()
    {
        var garden = ParseInput();
        long[] values = [
            CalculateReachableGardenPlots(garden, 65),
            CalculateReachableGardenPlots(garden, 65 + 131),
            CalculateReachableGardenPlots(garden, 65 + 131 * 2)
        ];

        var poly = GetSimplifiedLagrange(values);
        var target = (26501365 - 65) / 131;
        var result = poly.A * target * target + poly.B * target + poly.C;
        return new ValueTask<string>(result.ToString());
    }

    private Grid<char> ParseInput()
    {
        return new GridBuilder<char>().WithStartPosition('S').ExcludeValues(['#']).Build(Input.Value);
    }

    private static long CalculateReachableGardenPlots(Grid<char> garden, long steps)
    {
        Dictionary<Coordinate2D, long> visited = [];
        Queue<(Coordinate2D Position, long Step)> work = new();
        work.Enqueue((garden.StartPosition, 0));

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
            var wrappedPos = GetWrappedPosition(position, garden.RowCount);
            return garden.IsIncluded(wrappedPos);
        }
    }

    private static Coordinate2D GetWrappedPosition(Coordinate2D position, long gridLength)
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
