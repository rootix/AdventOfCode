using System.Diagnostics;

namespace AdventOfCode2023.Days;

public class Day22 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var bricks = ParseBricks();
        Settle(bricks);
        var zCache = BuildVerticalCache(bricks);
        var safeToRemove = bricks.Count(b => CanBeRemoved(b, zCache));
        return new ValueTask<string>(safeToRemove.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var bricks = ParseBricks();
        Settle(bricks);
        var zCache = BuildVerticalCache(bricks);
        var fallingBricksSum = bricks.Select(b => GetAffectedBrickCountByRemoval(b, zCache)).Sum();
        return new ValueTask<string>(fallingBricksSum.ToString());
    }

    private Brick[] ParseBricks()
    {
        var bricks = new List<Brick>();
        var brickLines = Input.Value.SplitByLine();
        for (var i = 0; i < brickLines.Length; i++)
        {
            var startAndEnd = brickLines[i].Split('~');
            var startCoordinates = startAndEnd[0].Split(',').Select(int.Parse).ToArray();
            var endCoordinates = startAndEnd[1].Split(',').Select(int.Parse).ToArray();
            var brick = new Brick(i + 1,
                new Coordinate3D(startCoordinates[0], startCoordinates[1], startCoordinates[2]),
                new Coordinate3D(endCoordinates[0], endCoordinates[1], endCoordinates[2]));
            bricks.Add(brick);
        }

        return [.. bricks.OrderBy(b => b.From.Z)];
    }

    private static void Settle(Brick[] bricks)
    {
        var highest = new Dictionary<(int X, int Y), int>();
        foreach (var brick in bricks)
        {
            var brickHeight = brick.To.Z - brick.From.Z + 1;
            for (var x = brick.From.X; x <= brick.To.X; x++)
            {
                for (var y = brick.From.Y; y <= brick.To.Y; y++)
                {
                    var currentHighest = highest.GetValueOrDefault((x, y), 0);
                    highest[(x, y)] = currentHighest + brickHeight;
                }
            }

            var possibleZ = 0;
            for (var x = brick.From.X; x <= brick.To.X; x++)
            {
                for (var y = brick.From.Y; y <= brick.To.Y; y++)
                {
                    var h = highest[(x, y)];
                    possibleZ = Math.Max(possibleZ, h);
                }
            }

            for (var x = brick.From.X; x <= brick.To.X; x++)
            {
                for (var y = brick.From.Y; y <= brick.To.Y; y++)
                {
                    highest[(x, y)] = possibleZ;
                }
            }

            var fallHeight = brick.To.Z - possibleZ;
            brick.From.Z -= fallHeight;
            brick.To.Z -= fallHeight;
        }
    }

    private static Dictionary<int, HashSet<Brick>> BuildVerticalCache(Brick[] bricks)
    {
        var zCache = new Dictionary<int, HashSet<Brick>>();
        foreach (var brick in bricks)
        {
            for (var z = brick.From.Z; z <= brick.To.Z; z++)
            {
                if (!zCache.TryGetValue(z, out var value))
                {
                    value = [];
                    zCache.Add(z, value);
                }

                value.Add(brick);
            }
        }

        // Expanded bounds to prevent overflows
        zCache[0] = [];
        zCache[zCache.Keys.Max() + 1] = [];

        return zCache;
    }

    private static bool CanBeRemoved(Brick brick, Dictionary<int, HashSet<Brick>> zCache)
    {
        foreach (var above in zCache[brick.To.Z + 1].Where(b => b != brick))
        {
            if (!above.IsSupportedBy(brick)) continue;
            var hasOtherSupport = zCache[above.From.Z - 1].Any(lower => lower != brick && above.IsSupportedBy(lower));
            if (!hasOtherSupport) return false;
        }

        return true;
    }

    private static int GetAffectedBrickCountByRemoval(Brick brick, Dictionary<int, HashSet<Brick>> zCache)
    {
        var fallingBricks = new HashSet<Brick>();
        foreach (var above in zCache[brick.To.Z + 1].Where(b => b != brick && b.IsSupportedBy(brick)))
        {
            CheckIfAffected(above, brick, zCache, fallingBricks);
        }

        return fallingBricks.Count;
    }

    private static void CheckIfAffected(Brick brick, Brick removedBrick, Dictionary<int, HashSet<Brick>> zCache, HashSet<Brick> fallingBricks)
    {
        if (fallingBricks.Contains(brick)) return;
        var hasOtherSupport = zCache[brick.From.Z - 1].Any(lower => lower != removedBrick && brick.IsSupportedBy(lower) && !fallingBricks.Contains(lower));
        if (hasOtherSupport) return;

        fallingBricks.Add(brick);
        foreach (var above in zCache[brick.To.Z + 1].Where(b => b != brick))
        {
            CheckIfAffected(above, brick, zCache, fallingBricks);
        }
    }

    [DebuggerDisplay("Id = {Id}")]
    public class Brick(int id, Coordinate3D from, Coordinate3D to)
    {
        public int Id { get; } = id;
        public Coordinate3D From { get; } = from;
        public Coordinate3D To { get; } = to;

        public bool IsSupportedBy(Brick brick)
        {
            return From.X <= brick.To.X && To.X >= brick.From.X &&
               From.Y <= brick.To.Y && To.Y >= brick.From.Y &&
               From.Z == brick.To.Z + 1;
        }
    }

    public class Coordinate3D(int x, int y, int z)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public int Z { get; set; } = z;
    }
}
