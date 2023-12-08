using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

public class Day08 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var (instructions, network) = ParseInput();
        var currentNode = "AAA";
        var steps = 0;
        while (currentNode != "ZZZ")
        {
            currentNode = GetNextNode(instructions[steps++ % instructions.Length], network[currentNode]);
        }

        return new ValueTask<string>(steps.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var (instructions, network) = ParseInput();
        var startNodes = network.Keys.Where(n => n[2] == 'A').ToArray();
        var allSteps = new long[startNodes.Length];
        for (var n = 0; n < startNodes.Length; n++)
        {
            var currentNode = startNodes[n];
            var nodeSteps = 0;
            while (currentNode[2] != 'Z')
            {
                currentNode = GetNextNode(instructions[nodeSteps++ % instructions.Length], network[currentNode]);
            }

            allSteps[n] = nodeSteps;
        }

        var steps = allSteps.Aggregate(CalculateLeastCommonMultiple);

        return new ValueTask<string>(steps.ToString());
    }

    private (char[] Instructions, IReadOnlyDictionary<string, (string Left, string Right)> Network) ParseInput()
    {
        var groups = Input.Value.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries);
        var instructions = groups[0].ToArray();

        var networkLineRegex = new Regex(@"([A-Z]{3}) = \(([A-Z]{3}), ([A-Z]{3})\)");
        var network = groups[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(networkLine => networkLineRegex.Match(networkLine)).ToDictionary(
                nodeMatches => nodeMatches.Groups[1].Value,
                nodeMatches => (nodeMatches.Groups[2].Value, nodeMatches.Groups[3].Value));

        return (instructions, network);
    }

    private static string GetNextNode(char instruction, (string Left, string Right) currentNode) => instruction == 'L' ? currentNode.Left : currentNode.Right;

    private static long CalculateLeastCommonMultiple(long a, long b) => a * b / CalculateGreatestCommonDivisor(a, b);

    private static long CalculateGreatestCommonDivisor(long a, long b) => b == 0 ? a : CalculateGreatestCommonDivisor(b, a % b);
}
