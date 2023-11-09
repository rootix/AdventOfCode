namespace AdventOfCode2023.Days;

public class Day01 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var packing = GetPackList(Input.Value);
        var calories = packing.Select(Enumerable.Sum).ToList();
        calories.Sort();
        var result = calories.Last();
        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var packing = GetPackList(Input.Value);
        var calories = packing.Select(Enumerable.Sum).ToList();
        calories.Sort();
        return new ValueTask<string>(calories.TakeLast(3).Sum().ToString());
    }

    private static IEnumerable<IEnumerable<int>> GetPackList(string input)
    {
        return input.Split($"{Environment.NewLine}{Environment.NewLine}")
            .Select(backpack => backpack.Split(Environment.NewLine)
                .Select(int.Parse)
                .ToList()).Cast<IEnumerable<int>>();
    }
}
