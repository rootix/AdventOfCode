using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

public class Day03 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var schematic = ParseSchematic();
        var partNumber = schematic.Numbers.Where(n => IsAdjacentToSymbol(n, schematic.Symbols)).Sum(n => n.Value);

        return new ValueTask<string>(partNumber.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var schematic = ParseSchematic();
        var gears = schematic.Symbols.Where(s => s.Character == '*');
        var gearRatios = gears.Select(g => GetAdjacentNumbers(g, schematic.Numbers))
            .Where(g => g.Count() == 2)
            .Select(n => n.Select(l => l.Value).Multiply())
            .Sum();

        return new ValueTask<string>(gearRatios.ToString());
    }

    private (Number[] Numbers, Symbol[] Symbols) ParseSchematic()
    {
        var lines = Input.Value.SplitByLine();
        var numbers = new List<Number>();
        var symbols = new List<Symbol>();
        var numberPattern = new Regex(@"\d+");
        var symbolPattern = new Regex(@"[^.\d]");

        for (var i = 0; i < lines.Length; i++)
        {
            foreach (Match numberMatch in numberPattern.Matches(lines[i]))
            {
                var numberValue = int.Parse(numberMatch.Value);
                var startIndex = numberMatch.Index;
                var endIndex = numberMatch.Index + numberMatch.Length - 1;
                numbers.Add(new Number(numberValue, i, startIndex, endIndex));
            }

            foreach (Match symbolMatch in symbolPattern.Matches(lines[i]))
            {
                symbols.Add(new Symbol(symbolMatch.Value[0], i, symbolMatch.Index));
            }
        }

        return (numbers.ToArray(), symbols.ToArray());
    }

    private static bool IsAdjacentToSymbol(Number number, Symbol[] symbols) =>
        symbols.Any(n => n.Line >= number.Line - 1 &&
                         n.Line <= number.Line + 1 &&
                         n.Pos >= number.Start - 1 &&
                         n.Pos <= number.End + 1);

    private static IEnumerable<Number> GetAdjacentNumbers(Symbol gear, Number[] numbers) =>
        numbers.Where(n => n.Line >= gear.Line - 1 &&
                           n.Line <= gear.Line + 1 &&
                           n.End >= gear.Pos - 1 &&
                           n.Start <= gear.Pos + 1);

    private sealed record Number(int Value, int Line, int Start, int End);

    private sealed record Symbol(char Character, int Line, int Pos);
}
