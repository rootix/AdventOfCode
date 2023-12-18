using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

public class Day10 : DayBase
{
    private const string PIPE_CHARACTERS = "|-LJ7F";

    public override ValueTask<string> Solve_1()
    {
        var (pipes, sPosition) = ParseMap();
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
        var (pipes, sPosition) = ParseMap();
        var enclosed = 0;
        foreach (var startPipe in PIPE_CHARACTERS)
        {
            var result = FollowPipe(startPipe, sPosition, pipes);
            if (!result.IsLoop) continue;
            enclosed = CountEnclosed(result.Path);
            break;
        }

        return new ValueTask<string>(enclosed.ToString());
    }

    private (IDictionary<Coordinate2D, char> Pipes, Coordinate2D SPosition) ParseMap()
    {
        var sPosition = new Coordinate2D(0, 0);
        var lines = Input.Value.SplitByLine();
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
        }

        return (pipes, sPosition);
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

    // Shoelace & Pick's Theorem
    private static int CountEnclosed(Coordinate2D[] loop)
    {
        double area = 0;
        for (var i = 0; i < loop.Length; i++)
        {
            var nextIndex = (i + 1) % loop.Length;
            area += loop[i].Col * loop[nextIndex].Row - loop[i].Row * loop[nextIndex].Col;
        }

        var result = Math.Round(area / 2 - loop.Length / 2 + 1);
        return Convert.ToInt32(result);
    }
}
