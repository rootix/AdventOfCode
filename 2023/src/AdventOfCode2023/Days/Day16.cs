namespace AdventOfCode2023.Days;

public class Day16 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var grid = new GridBuilder<char>().Build(Input.Value);
        var energyLevel = GetEnergyLevel(grid, new Beam(new Coordinate2D(0, -1), Direction.Right));

        return new ValueTask<string>(energyLevel.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var grid = new GridBuilder<char>().Build(Input.Value);
        var startBeams = new List<Beam>();

        for (var col = 0; col < grid.MaxColIndex; col++)
        {
            startBeams.Add(new Beam(new Coordinate2D(-1, col), Direction.Down));
            startBeams.Add(new Beam(new Coordinate2D(grid.MaxRowIndex + 1, col), Direction.Up));
        }

        for (var row = 0; row < grid.MaxRowIndex; row++)
        {
            startBeams.Add(new Beam(new Coordinate2D(row, -1), Direction.Right));
            startBeams.Add(new Beam(new Coordinate2D(row, grid.MaxColIndex + 1), Direction.Left));
        }

        var maxEnergyLevel = startBeams.Select(beam => GetEnergyLevel(grid, beam)).Prepend(0).Max();

        return new ValueTask<string>(maxEnergyLevel.ToString());
    }

    private static int GetEnergyLevel(Grid<char> grid, Beam startBeam)
    {
        var beams = new List<Beam> { startBeam };
        var energized = new HashSet<(Coordinate2D, Direction)>();
        while (beams.Count > 0)
        {
            foreach (var beam in beams.ToArray())
            {
                beam.Position = beam.Position.Move(beam.Direction);
                if (energized.Contains((beam.Position, beam.Direction)) ||
                    !grid.IsWithinBounds(beam.Position))
                {
                    beams.Remove(beam);
                    continue;
                }

                energized.Add((beam.Position, beam.Direction));
                var encounter = grid.GetValue(beam.Position);
                switch (encounter)
                {
                    case '-' when beam.Direction is Direction.Left or Direction.Right:
                        continue;
                    case '-':
                        beam.Direction = Direction.Left;
                        beams.Add(new Beam(beam.Position, Direction.Right));
                        break;
                    case '|' when beam.Direction is Direction.Up or Direction.Down:
                        continue;
                    case '|':
                        beam.Direction = Direction.Down;
                        beams.Add(new Beam(beam.Position, Direction.Up));
                        break;
                    case '/':
                        beam.Direction = beam.Direction switch
                        {
                            Direction.Up => Direction.Right,
                            Direction.Right => Direction.Up,
                            Direction.Down => Direction.Left,
                            Direction.Left => Direction.Down,
                            _ => throw new NotSupportedException()
                        };
                        break;
                    case '\\':
                        beam.Direction = beam.Direction switch
                        {
                            Direction.Up => Direction.Left,
                            Direction.Right => Direction.Down,
                            Direction.Down => Direction.Right,
                            Direction.Left => Direction.Up,
                            _ => throw new NotSupportedException()
                        };
                        break;
                }
            }
        }

        return energized.Select(e => e.Item1).Distinct().Count();
    }

    private sealed class Beam(Coordinate2D position, Direction direction)
    {
        public Coordinate2D Position { get; set; } = position;

        public Direction Direction { get; set; } = direction;
    }
}
