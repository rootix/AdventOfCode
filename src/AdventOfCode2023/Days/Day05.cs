namespace AdventOfCode2023.Days;

public class Day05 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var (seeds, maps) = ParseAlmanac();

        return new ValueTask<string>(LookupMinLocation(seeds, maps).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var (seeds, maps) = ParseAlmanac();
        var validSeeds = maps.SelectMany((map, idx) =>
                map.MapLines
                    .Select(line => LookupSeed(maps.Take(idx + 1), line.Destination))
                    .Where(seed => IsValidSeed(seed, seeds)))
            .ToArray();

        return new ValueTask<string>(LookupMinLocation(validSeeds, maps).ToString());
    }

    private (long[] Seeds, Map[] Maps) ParseAlmanac()
    {
        var groups = Input.Value.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries);
        var seeds = groups[0]
            .Split(' ', StringSplitOptions.TrimEntries)
            .Where(s => s.All(char.IsDigit))
            .Select(long.Parse).ToArray();

        var maps = new List<Map>();
        foreach (var map in groups.Skip(1))
        {
            var mapLines = map.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var mapLinesList = new List<MapLine>();
            foreach (var mapLine in mapLines.Skip(1))
            {
                var numbers = mapLine
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToArray();
                var destination = numbers[0];
                var source = numbers[1];
                var range = numbers[2];

                mapLinesList.Add(new MapLine(destination, source, range));
            }

            maps.Add(new Map([.. mapLinesList]));
        }

        return (seeds, [.. maps]);
    }

    private static long LookupMinLocation(IEnumerable<long> seeds, Map[] maps)
    {
        var minLocation = long.MaxValue;
        foreach (var seed in seeds)
        {
            var location = LookupLocation(seed, maps);
            minLocation = Math.Min(minLocation, location);
        }

        return minLocation;
    }

    private static long LookupLocation(long seed, IEnumerable<Map> maps)
    {
        var currentLocation = seed;
        foreach (var map in maps)
        {
            foreach (var mapLine in map.MapLines)
            {
                if (currentLocation < mapLine.Source || currentLocation > mapLine.Source + mapLine.Range) continue;
                currentLocation = mapLine.Destination + currentLocation - mapLine.Source;
                break;
            }
        }

        return currentLocation;
    }

    private static long LookupSeed(IEnumerable<Map> maps, long location)
    {
        return maps.Reverse().Aggregate(location, (curr, mappings) =>
        {
            foreach (var m in mappings.MapLines)
            {
                if (curr >= m.Destination && curr <= m.Destination + m.Range)
                {
                    return m.Source + (curr - m.Destination);
                }
            }

            return curr;
        });
    }

    private static bool IsValidSeed(long seed, IReadOnlyList<long> seeds)
    {
        for (var i = 0; i < seeds.Count; i += 2)
        {
            if (seed >= seeds[i] && seed <= seeds[i] + seeds[i + 1])
            {
                return true;
            }
        }

        return false;
    }

    private sealed record Map(MapLine[] MapLines);

    private sealed record MapLine(long Destination, long Source, long Range);
}
