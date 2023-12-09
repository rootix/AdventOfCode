namespace AdventOfCode2023.Days;

public class Day01 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        return GetCalibrationValue(Input.Value.SplitByLine());
    }

    public override ValueTask<string> Solve_2()
    {
        var lines = Input.Value.SplitByLine().Select(l => l
            .Replace("zero", "zero0zero")
            .Replace("one", "one1one")
            .Replace("two", "two2two")
            .Replace("three", "three3three")
            .Replace("four", "four4four")
            .Replace("five", "five5five")
            .Replace("six", "six6six")
            .Replace("seven", "seven7seven")
            .Replace("eight", "eight8eight")
            .Replace("nine", "nine9nine"));
        return GetCalibrationValue(lines);
    }

    private static ValueTask<string> GetCalibrationValue(IEnumerable<string> lines)
    {
        var calibrationValue = 0;
        foreach (var line in lines)
        {
            char? firstDigit = null;
            char? lastDigit = null;
            foreach (var character in line.Where(char.IsDigit))
            {
                if (!firstDigit.HasValue)
                {
                    firstDigit = character;
                }
                else
                {
                    lastDigit = character;
                }
            }

            lastDigit ??= firstDigit;

            calibrationValue += int.Parse($"{firstDigit}{lastDigit}");
        }

        return new ValueTask<string>(calibrationValue.ToString());
    }
}
