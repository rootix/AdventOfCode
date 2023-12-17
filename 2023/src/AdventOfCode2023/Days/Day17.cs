namespace AdventOfCode2023.Days;

public class Day17 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var minHeathLoss = CalculateMinHeathLoss(3, _ => true);
        return new ValueTask<string>(minHeathLoss.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var minHeathLoss = CalculateMinHeathLoss(10, move => move.StraightMoves >= 4);
        return new ValueTask<string>(minHeathLoss.ToString());
    }

    private int CalculateMinHeathLoss(int maxStraightMoves, Func<Move, bool> turnCondition)
    {
        var grid = Input.Value.ToIntGrid();
        var endPoint = new Coordinate2D(grid.Length - 1, grid[0].Length - 1);
        var queue = new PriorityQueue<Move, int>();
        var cache = new HashSet<string>();

        queue.Enqueue(new Move(new Coordinate2D(0, 0), Direction.Right, 0, 0), 0);
        queue.Enqueue(new Move(new Coordinate2D(0, 0), Direction.Down, 0, 0), 0);

        while (queue.Count > 0)
        {
            var move = queue.Dequeue();
            if (move.Position == endPoint)
            {
                return move.HeatLoss;
            }

            if (move.StraightMoves < maxStraightMoves)
            {
                TryEnqueueNextMove(grid, move, move.Direction, queue, cache);
            }

            if (turnCondition(move))
            {
                if (move.Direction is Direction.Up or Direction.Down)
                {
                    TryEnqueueNextMove(grid, move, Direction.Left, queue, cache);
                    TryEnqueueNextMove(grid, move, Direction.Right, queue, cache);
                }
                else {
                    TryEnqueueNextMove(grid, move, Direction.Up, queue, cache);
                    TryEnqueueNextMove(grid, move, Direction.Down, queue, cache);
                }
            }
        }

        return 0;
    }

    private static void TryEnqueueNextMove(int[][] grid, Move move, Direction direction, PriorityQueue<Move, int> queue, HashSet<string> cache)
    {
        var newPos = move.Position.Move(direction);
        if (newPos.Row < 0 ||
            newPos.Row >= grid.Length ||
            newPos.Col < 0 ||
            newPos.Col >= grid[0].Length)
        {
            return;
        }

        var newMove = move with
        {
            Direction = direction,
            StraightMoves = move.Direction == direction ? move.StraightMoves + 1 : 1,
            Position = newPos,
            HeatLoss = move.HeatLoss + grid[newPos.Row][newPos.Col]
        };

        var cacheKey = newMove.GetCacheKey();
        if (cache.Contains(cacheKey))
        {
            return;
        }

        queue.Enqueue(newMove, newMove.HeatLoss);
        cache.Add(cacheKey);
    }

    private sealed record Move(Coordinate2D Position, Direction Direction, int StraightMoves, int HeatLoss)
    {
        public string GetCacheKey() => $"{Position.Row};{Position.Col};{Direction};{StraightMoves};";
    };
}
