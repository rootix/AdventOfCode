namespace AdventOfCode2023.Helpers;

public record Coordinate2D(int Row, int Col)
{
    public Coordinate2D Move(Direction direction) => direction switch
    {
        Direction.Up => this with { Row = Row - 1 },
        Direction.Right => this with { Col = Col + 1 },
        Direction.Down => this with { Row = Row + 1 },
        Direction.Left => this with { Col = Col - 1 },
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
    };
}
