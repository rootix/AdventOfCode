namespace AdventOfCode2023.Days;

public class Day23 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var (graph, _, startPosition, endPosition) = ParseInput(true);
        var longestPath = FindLongestPath(graph, [], startPosition, endPosition);

        return new ValueTask<string>(longestPath.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var (graph, keyPositions, startPosition, endPosition) = ParseInput(false);
        var keyPositionGraph = BuildKeyPositionGraph(keyPositions, graph);
        var longestPath = FindLongestPath(keyPositionGraph, [], startPosition, endPosition);

        return new ValueTask<string>(longestPath.ToString());
    }

    private (Dictionary<Coordinate2D, List<(Coordinate2D Position, int Steps)>> Graph, List<Coordinate2D> KeyPositions, Coordinate2D StartPosition, Coordinate2D EndPosition) ParseInput(bool slippery)
    {
        var graph = new Dictionary<Coordinate2D, List<(Coordinate2D, int)>>();
        var keyPositions = new List<Coordinate2D>();
        var grid = new GridBuilder<char>().ExcludeValues(['#']).Build(Input.Value);

        foreach (var row in grid.Rows)
        {
            foreach (var col in row.Cols)
            {
                if (!grid.IsIncluded(col.Position)) continue;

                if (row.Index == 0)
                {
                    grid.SetStartPosition(col.Position);
                }

                if (row.Index == grid.MaxRowIndex)
                {
                    grid.SetEndPosition(col.Position);
                }

                graph[col.Position] = [];

                if (slippery)
                {
                    switch (col.Value)
                    {
                        case '^':
                            graph[col.Position].Add((col.Position.Move(Direction.Up), 1));
                            continue;
                        case '>':
                            graph[col.Position].Add((col.Position.Move(Direction.Right), 1));
                            continue;
                        case 'v':
                            graph[col.Position].Add((col.Position.Move(Direction.Down), 1));
                            continue;
                        case '<':
                            graph[col.Position].Add((col.Position.Move(Direction.Left), 1));
                            continue;
                    }
                }

                List<(Coordinate2D, int)> neighbours = [];
                var isIntersection = true;
                foreach (var neighbour in Enum.GetValues<Direction>().Select(d => col.Position.Move(d)))
                {
                    if (!grid.IsIncluded(neighbour)) continue;
                    neighbours.Add((neighbour, 1));
                    if (grid.GetValue(neighbour) is '.')
                    {
                        isIntersection = false;
                    }
                }

                graph[col.Position] = neighbours;
                if (isIntersection)
                {
                    keyPositions.Add(col.Position);
                }
            }
        }

        keyPositions.Insert(0, grid.StartPosition);
        keyPositions.Add(grid.EndPosition);

        return (graph, keyPositions, grid.StartPosition, grid.EndPosition);
    }

    private static int FindLongestPath(
        Dictionary<Coordinate2D, List<(Coordinate2D Position, int Steps)>> graph,
        HashSet<Coordinate2D> visited,
        Coordinate2D currentPosition,
        Coordinate2D endPosition,
        int currentPathLength = 0)
    {
        if (currentPosition == endPosition)
        {
            return currentPathLength;
        }

        var longestPath = 0;
        foreach (var (pos, steps) in graph[currentPosition])
        {
            if (!visited.Add(pos)) continue;
            var pathLength = FindLongestPath(graph, visited, pos, endPosition, currentPathLength + steps);
            longestPath = Math.Max(longestPath, pathLength);
            visited.Remove(pos);
        }

        return longestPath;
    }

    private static Dictionary<Coordinate2D, List<(Coordinate2D Position, int Steps)>> BuildKeyPositionGraph(
        List<Coordinate2D> keyPositions,
        Dictionary<Coordinate2D, List<(Coordinate2D Position, int Steps)>> graph)
    {
        var keyGraph = new Dictionary<Coordinate2D, List<(Coordinate2D Position, int Steps)>>();
        foreach (var keyPosition in keyPositions)
        {
            var nearest = GetNearestKeyPositions(keyPosition, keyPositions, graph);
            keyGraph.Add(keyPosition, nearest);
        }

        return keyGraph;
    }

    private static List<(Coordinate2D KeyPosition, int Steps)> GetNearestKeyPositions(
        Coordinate2D startPosition,
        List<Coordinate2D> keyPositions,
        Dictionary<Coordinate2D, List<(Coordinate2D Position, int Steps)>> graph)
    {
        var queue = new Queue<(Coordinate2D Position, int Steps)>();
        var seen = new HashSet<Coordinate2D> { startPosition };
        var neighbours = new List<(Coordinate2D Position, int Steps)>();

        queue.Enqueue((startPosition, 0));

        while (queue.Count > 0)
        {
            var (pos, steps) = queue.Dequeue();
            if (pos != startPosition && keyPositions.Contains(pos))
            {
                neighbours.Add((pos, steps));
                continue;
            }

            foreach (var (npos, _) in graph[pos])
            {
                if (seen.Add(npos))
                {
                    queue.Enqueue((npos, steps + 1));
                }
            }
        }

        return neighbours;
    }
}
