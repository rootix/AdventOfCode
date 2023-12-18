using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

public class Day18 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var lagoonSize = CalculateLagoonSize(false);
        return new ValueTask<string>(lagoonSize.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var lagoonSize = CalculateLagoonSize(true);
        return new ValueTask<string>(lagoonSize.ToString());
    }

    private long CalculateLagoonSize(bool part2)
    {
        List<Coordinate2D> holes = [];
        Coordinate2D currentPosition = new(0, 0);
        long borderLength = 0;
        foreach (var digInstruction in ParseDigInstructions(part2))
        {
            borderLength += digInstruction.Meters;
            currentPosition = currentPosition.Move(digInstruction.Direction, digInstruction.Meters);
            holes.Add(currentPosition);
        }

        // Shoelace & Pick's Theorem
        double area = 0;
        for (var i = 0; i < holes.Count; i++)
        {
            var nextIndex = (i + 1) % holes.Count;
            area += holes[i].Col * holes[nextIndex].Row - holes[i].Row * holes[nextIndex].Col;
        }

        var result = Math.Round(area / 2 - borderLength / 2 + 1) + borderLength;
        return Convert.ToInt64(result);
    }

    private IEnumerable<DigInstruction> ParseDigInstructions(bool part2)
    {
        var lineRegex = new Regex(@"([URDL]{1}) (\d*) \(#(.*)\)");
        foreach (var line in Input.Value.SplitByLine())
        {
            var match = lineRegex.Match(line);

            if (part2)
            {
                var hexCode = match.Groups[3].Value;
                var direction = hexCode[^1] switch
                {
                    '0' => Direction.Right,
                    '1' => Direction.Down,
                    '2' => Direction.Left,
                    '3' => Direction.Up,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var meters = Convert.ToInt32(hexCode[..5], 16);
                yield return new DigInstruction(direction, meters);
            }
            else
            {
                var direction = match.Groups[1].Value switch
                {
                    "U" => Direction.Up,
                    "R" => Direction.Right,
                    "D" => Direction.Down,
                    "L" => Direction.Left,
                    _ => throw new ArgumentOutOfRangeException()
                };
                yield return new DigInstruction(direction, int.Parse(match.Groups[2].Value));
            }
        }
    }

    private sealed record DigInstruction(Direction Direction, int Meters);
}
