namespace AdventOfCode2023.Helpers;

public record Coordinate2D(int Y, int X)
{
    public Coordinate2D GetAboveCoordinate() => this with { Y = Y - 1 };
    public Coordinate2D GetRightCoordinate() => this with { X = X + 1 };
    public Coordinate2D GetBelowCoordinate() => this with { Y = Y + 1 };
    public Coordinate2D GetLeftCoordinate() => this with { X = X - 1 };
}
