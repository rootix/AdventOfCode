namespace AdventOfCode2023.Days;

// What a hard ending this year! I tried for 6 hours with no success.
// I got the gold star myself with the Girvan–Newman algorithm (https://en.wikipedia.org/wiki/Girvan%E2%80%93Newman_algorithm), but it took 18 minutes to complete.
// Other things i tried:
//   - https://en.wikipedia.org/wiki/Bridge_(graph_theory)
//   - https://en.wikipedia.org/wiki/Karger%27s_algorithm
// At the end, i copied the solution with the Karger's algorithm from a topaz paste, as i got errors in my attempt. It's Christmas, i have family :')
public class Day25 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var (nodes, edges) = ParseGraph();
        var groupCounts = FindGroupSizes(nodes, edges);

        return new ValueTask<string>((groupCounts.FirstGroupCount * groupCounts.SecondGroupCount).ToString());
    }

    private (HashSet<string> Nodes, List<(string, string)> Edges) ParseGraph()
    {
        var nodes = new HashSet<string>();
        var edges = new List<(string, string)>();
        foreach (var line in Input.Value.SplitByLine())
        {
            var nodeList = line.Replace(':', ' ').SplitByWhitespace();
            var node = nodeList[0];

            nodes.Add(node);
            foreach (var n in nodeList)
            {
                if (n == node) continue;
                nodes.Add(n);
                edges.Add((node, n));
            }
        }

        return (nodes, edges);
    }

    private (int FirstGroupCount, int SecondGroupCount) FindGroupSizes(HashSet<string> nodes, List<(string, string)> edges)
    {
        List<List<string>> groups = [];

        do
        {
            int i;
            groups = [];
            groups.AddRange(nodes.Select(node => (List<string>)[node]));
            while (groups.Count > 2)
            {
                i = new Random().Next() % edges.Count;

                var firstGroup = groups.First(s => s.Contains(edges[i].Item1));
                var secondGroup = groups.First(s => s.Contains(edges[i].Item2));

                if (firstGroup == secondGroup) continue;

                groups.Remove(secondGroup);
                firstGroup.AddRange(secondGroup);
            }

        } while (CountCuts(edges, groups) != 3);

        return (groups[0].Count, groups[1].Count);
    }

    private static int CountCuts(List<(string, string)> edges, List<List<string>> subsets)
    {
        var cuts = 0;
        for (var i = 0; i < edges.Count; ++i)
        {
            var firstGroup = subsets.First(s => s.Contains(edges[i].Item1));
            var secondGroup = subsets.First(s => s.Contains(edges[i].Item2));
            if (firstGroup != secondGroup) ++cuts;
        }

        return cuts;
    }
}
