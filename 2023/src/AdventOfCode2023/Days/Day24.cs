using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

// I solved part 1 with a simpler intersection method. But after having no idea for part 2, i copied the solution from https://github.com/kevinbrechbuehl/advent-of-code/blob/main/2023/24/puzzle.ts
// But since Kevins intersection method was different due to the offset handling, i had to rewrite my part 1 to work with his approach. This day was way over the top for me. Seems like 99% of people used Z3 or Mathematica anyways.
public class Day24 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var min = IsSample ? 7 : 200000000000000;
        var max = IsSample ? 27 : 400000000000000;

        var hailstones = ParseHailStones();
        var collisions = 0;

        for (var i = 0; i < hailstones.Count - 1; i++)
        {
            var ha = hailstones[i];
            for (var j = i + 1; j < hailstones.Count; j++)
            {
                var hb = hailstones[j];
                var intersection = GetIntersectionPoint(ha, hb, (0, 0));
                if (!intersection.HasValue) continue;
                if (IsWithinRange(intersection.Value.X, intersection.Value.Y) && IsInFuture(ha, intersection.Value.X, intersection.Value.Y) && IsInFuture(hb, intersection.Value.X, intersection.Value.Y))
                {
                    collisions++;
                }
            }
        }

        return new ValueTask<string>(collisions.ToString());

        bool IsWithinRange(double x, double y)
        {
            return x >= min && y <= max && y >= min && y <= max;
        }

        bool IsInFuture(Hailstone h, double x, double y)
        {
            return x - h.X > 0 == h.VX > 0 &&
                   y - h.Y > 0 == h.VY > 0;
        }
    }

    public override ValueTask<string> Solve_2()
    {
        const int range = 300;
        var hailstones = ParseHailStones();

        for (var x = -range; x < range; x++)
        {
            for (var y = -range; y < range; y++)
            {
                var offset = (x, y);

                var intersect1 = GetIntersectionPoint(hailstones[1], hailstones[0], offset);
                var intersect2 = GetIntersectionPoint(hailstones[2], hailstones[0], offset);
                var intersect3 = GetIntersectionPoint(hailstones[3], hailstones[0], offset);

                if (intersect1 == null || intersect2 == null || intersect3 == null) continue;

                if (!IsSamePosition(intersect1.Value, intersect2.Value) ||
                    !IsSamePosition(intersect1.Value, intersect3.Value) ||
                    !IsSamePosition(intersect2.Value, intersect3.Value)) continue;

                for (var z = -range; z < range; z++)
                {
                    var intersectZ1 = hailstones[1].Z + intersect1.Value.Time * (hailstones[1].VZ + z);
                    var intersectZ2 = hailstones[2].Z + intersect2.Value.Time * (hailstones[2].VZ + z);
                    var intersectZ3 = hailstones[3].Z + intersect3.Value.Time * (hailstones[3].VZ + z);

                    if (intersectZ1 != intersectZ2 || intersectZ1 != intersectZ3 || intersectZ2 != intersectZ3) continue;

                    return new ValueTask<string>((intersect1.Value.X + intersect1.Value.Y + intersectZ1).ToString());
                }
            }
        }

        return new ValueTask<string>("0");

        static bool IsSamePosition((double X, double Y, double Time) a, (double X, double Y, double Time) b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
    }

    private List<Hailstone> ParseHailStones()
    {
        var regex = new Regex(@"^(\d+),\s*(\d+),\s*(\d+)\s*@\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)$");
        var hailstoneLines = Input.Value.SplitByLine();

        return hailstoneLines.Select(hailstoneLine => regex.Match(hailstoneLine)).Select(match =>
                new Hailstone(long.Parse(match.Groups[1].Value),
                    long.Parse(match.Groups[2].Value),
                    long.Parse(match.Groups[3].Value),
                    long.Parse(match.Groups[4].Value),
                    long.Parse(match.Groups[5].Value),
                    long.Parse(match.Groups[6].Value)))
            .ToList();
    }

    private static (double X, double Y, double Time)? GetIntersectionPoint(Hailstone ha, Hailstone hb, (int x, int y) offset)
    {
        double x1 = ha.X;
        double y1 = ha.Y;
        double x2 = ha.X + ha.VX + offset.x;
        double y2 = ha.Y + ha.VY + offset.y;
        double x3 = hb.X;
        double y3 = hb.Y;
        double x4 = hb.X + hb.VX + offset.x;
        double y4 = hb.Y + hb.VY + offset.y;

        if ((x1 == x2 && y1 == y2) || (x3 == x4 && y3 == y4))
        {
            return null;
        }

        var denominator = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);

        if (denominator == 0)
        {
            return null;
        }

        var ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / denominator;

        var x = x1 + ua * (x2 - x1);
        var y = y1 + ua * (y2 - y1);

        return (x, y, ua);
    }

    private sealed record Hailstone(long X, long Y, long Z, long VX, long VY, long VZ);
}
