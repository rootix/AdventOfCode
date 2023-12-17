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
        for (var row = 0; row < lines.Length; row++)
        {
            var line = new char[lines[row].Length];
            for (var col = 0; col < lines[row].Length; col++)
            {
                var posChar = lines[row][col];
                line[col] = posChar;

                if (posChar != '.')
                {
                    pipes.Add(new Coordinate2D(row, col), posChar);
                }

                if (posChar == 'S')
                {
                    sPosition = new Coordinate2D(row, col);
                }
            }

            map[row] = line;
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
            if (currentPipe is '|' or 'L' or 'J' && lastDirection is not Direction.Down)
            {
                var northCoordinate = currentPosition.Move(Direction.Up);
                if (pipes.ContainsKey(northCoordinate))
                {
                    lastDirection = Direction.Up;
                    currentPosition = northCoordinate;
                    continue;
                }
            }

            if (currentPipe is '-' or 'L' or 'F' && lastDirection is not Direction.Left)
            {
                var eastCoordinate = currentPosition.Move(Direction.Right);
                if (pipes.ContainsKey(eastCoordinate))
                {
                    lastDirection = Direction.Right;
                    currentPosition = eastCoordinate;
                    continue;
                }
            }

            if (currentPipe is '|' or '7' or 'F' && lastDirection is not Direction.Up)
            {
                var southCoordinate = currentPosition.Move(Direction.Down);
                if (pipes.ContainsKey(southCoordinate))
                {
                    lastDirection = Direction.Down;
                    currentPosition = southCoordinate;
                    continue;
                }
            }

            if (currentPipe is '-' or '7' or 'J' && lastDirection is not Direction.Right)
            {
                var westCoordinate = currentPosition.Move(Direction.Left);
                if (pipes.ContainsKey(westCoordinate))
                {
                    lastDirection = Direction.Left;
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
        for (var row = 0; row < map.Count; row++)
        {
            StringBuilder sb = new();
            for (var col = 0; col <= map[row].Length; col++)
            {
                sb.Append(loop.Contains(new Coordinate2D(row, col)) ? map[row][col] : '.');
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
}
