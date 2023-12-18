namespace AdventOfCode2023.Helpers;

public record Coordinate2D(long Row, long Col)
{
    public Coordinate2D Move(Direction direction, long amount = 1) => direction switch
    {
        Direction.Up => this with { Row = Row - amount },
        Direction.Right => this with { Col = Col + amount },
        Direction.Down => this with { Row = Row + amount },
        Direction.Left => this with { Col = Col - amount },
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
    };
}
