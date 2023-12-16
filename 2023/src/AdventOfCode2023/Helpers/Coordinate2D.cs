namespace AdventOfCode2023.Helpers;

public record Coordinate2D(int Row, int Col)
{
    public Coordinate2D GetAboveCoordinate() => this with { Row = Row - 1 };
    public Coordinate2D GetRightCoordinate() => this with { Col = Col + 1 };
    public Coordinate2D GetBelowCoordinate() => this with { Row = Row + 1 };
    public Coordinate2D GetLeftCoordinate() => this with { Col = Col - 1 };
}
