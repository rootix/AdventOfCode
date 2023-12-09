namespace AdventOfCode2023.Days;

public class Day09 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var sequences = ParseSequences();
        var sum = 0;
        foreach (var sequence in sequences)
        {
            var iterations = GetNonZeroIterations(sequence);
            var currentLastValue = 0;
            for (var i = iterations.Length - 1; i >= 0; i--)
            {
                currentLastValue = iterations[i][^1] + currentLastValue;
            }

            sum += currentLastValue;
        }

        return new ValueTask<string>(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var sequences = ParseSequences();
        var sum = 0;
        foreach (var sequence in sequences)
        {
            var iterations = GetNonZeroIterations(sequence);
            var currentFirstValue = 0;
            for (var i = iterations.Length - 1; i >= 0; i--)
            {
                currentFirstValue = iterations[i][0] - currentFirstValue;
            }

            sum += currentFirstValue;
        }

        return new ValueTask<string>(sum.ToString());
    }

    private int[][] ParseSequences()
    {
        return Input.Value
            .SplitByLine()
            .Select(s => s.SplitByWhitespace()
                .Select(int.Parse)
                .ToArray())
            .ToArray();
    }

    private static int[][] GetNonZeroIterations(int[] sequence)
    {
        var iterations = new List<int[]>();
        var iteration = sequence;
        while (iteration.Any(s => s != iteration[0]))
        {
            iterations.Add(iteration);
            var nextIteration = new int[iteration.Length - 1];
            for (var i = 0; i < nextIteration.Length; i++)
            {
                nextIteration[i] = iteration[i + 1] - iteration[i];
            }

            iteration = nextIteration;
        }

        return [.. iterations, iteration];
    }
}
