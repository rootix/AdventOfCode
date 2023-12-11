using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

public class Day10 : DayBase
{
    private const string PIPE_CHARACTERS = "|-LJ7F";

    public override ValueTask<string> Solve_1()
    {
        var (_, pipes, sPosition) = ParseMap();
        var length = 0;
        foreach (var startPipe in PIPE_CHARACTERS)
        {
            var result = FollowPipe(startPipe, sPosition, pipes);
            if (!result.IsLoop) continue;
            length = result.Length / 2;
            break;
        }

        return new ValueTask<string>(length.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var (map, pipes, sPosition) = ParseMap();
        var enclosed = 0;
        foreach (var startPipe in PIPE_CHARACTERS)
        {
            var result = FollowPipe(startPipe, sPosition, pipes);
            if (!result.IsLoop) continue;
            enclosed = CountEnclosed(map, result.Path.ToHashSet());
            break;
        }

        return new ValueTask<string>(enclosed.ToString());
    }

    private (char[][] Map, IDictionary<Coordinate2D, char> Pipes, Coordinate2D SPosition) ParseMap()
    {
        var sPosition = new Coordinate2D(0, 0);
        var lines = Input.Value.SplitByLine();
        var map = new char[lines.Length][];
        var pipes = new Dictionary<Coordinate2D, char>();
        for (var y = 0; y < lines.Length; y++)
        {
            var line = new char[lines[y].Length];
            for (var x = 0; x < lines[y].Length; x++)
            {
                var posChar = lines[y][x];
                line[x] = posChar;

                if (posChar != '.')
                {
                    pipes.Add(new Coordinate2D(y, x), posChar);
                }

                if (posChar == 'S')
                {
                    sPosition = new Coordinate2D(y, x);
                }
            }

            map[y] = line;
        }

        return (map, pipes, sPosition);
    }

    private static (bool IsLoop, int Length, Coordinate2D[] Path) FollowPipe(char startPipe, Coordinate2D startPosition, IDictionary<Coordinate2D, char> pipes)
    {
        var length = 0;
        pipes[startPosition] = startPipe;
        var currentPosition = startPosition;
        Direction? lastDirection = null;
        var visited = new List<Coordinate2D>();
        while (true)
        {
            visited.Add(currentPosition);
            if (length > 0 && currentPosition == startPosition)
            {
                return (true, length, visited.ToArray());
            }

            length += 1;

            var currentPipe = pipes[currentPosition];
            if (currentPipe is '|' or 'L' or 'J' && lastDirection is not Direction.South)
            {
                var northCoordinate = currentPosition.GetAboveCoordinate();
                if (pipes.ContainsKey(northCoordinate))
                {
                    lastDirection = Direction.North;
                    currentPosition = northCoordinate;
                    continue;
                }
            }

            if (currentPipe is '-' or 'L' or 'F' && lastDirection is not Direction.West)
            {
                var eastCoordinate = currentPosition.GetRightCoordinate();
                if (pipes.ContainsKey(eastCoordinate))
                {
                    lastDirection = Direction.East;
                    currentPosition = eastCoordinate;
                    continue;
                }
            }

            if (currentPipe is '|' or '7' or 'F' && lastDirection is not Direction.North)
            {
                var southCoordinate = currentPosition.GetBelowCoordinate();
                if (pipes.ContainsKey(southCoordinate))
                {
                    lastDirection = Direction.South;
                    currentPosition = southCoordinate;
                    continue;
                }
            }

            if (currentPipe is '-' or '7' or 'J' && lastDirection is not Direction.East)
            {
                var westCoordinate = currentPosition.GetLeftCoordinate();
                if (pipes.ContainsKey(westCoordinate))
                {
                    lastDirection = Direction.West;
                    currentPosition = westCoordinate;
                    continue;
                }
            }

            return (false, length, visited.ToArray());
        }
    }

    private static int CountEnclosed(IReadOnlyList<char[]> map, HashSet<Coordinate2D> loop)
    {
        List<string> cleanedMap = [];
        for (var y = 0; y < map.Count; y++)
        {
            StringBuilder sb = new();
            for (var x = 0; x <= map[y].Length; x++)
            {
                sb.Append(loop.Contains(new Coordinate2D(y, x)) ? map[y][x] : '.');
            }

            cleanedMap.Add(Regex.Replace(Regex.Replace(sb.ToString(), "F-*7|L-*J", string.Empty), "F-*J|L-*7", "|"));
        }

        var enclosed = 0;
        foreach (var line in cleanedMap)
        {
            var parity = 0;
            foreach (var c in line)
            {
                switch (c)
                {
                    case '|':
                        parity++;
                        break;
                    case '.' when parity % 2 == 1:
                        enclosed++;
                        break;
                }
            }
        }

        return enclosed;
    }

    private enum Direction
    {
        North,
        East,
        South,
        West
    }
}
