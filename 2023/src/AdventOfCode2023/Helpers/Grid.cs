namespace AdventOfCode2023.Helpers;

public class Grid<T>(T[][] grid, Dictionary<Coordinate2D, T>? items, Coordinate2D? startPosition, Coordinate2D? endPosition) where T : struct
{
    public long RowCount => grid.LongLength;
    public long ColCount => grid[0].LongLength;
    public long MaxRowIndex => grid.LongLength - 1;
    public long MaxColIndex => grid[0].LongLength - 1;

    public Coordinate2D StartPosition => startPosition ?? throw new InvalidOperationException("Start position not configured");
    public Coordinate2D EndPosition => endPosition ?? throw new InvalidOperationException("End position not configured");
    public IEnumerable<GridBuilder<T>.Row> Rows => grid.Select((row, rowIndex) => new GridBuilder<T>.Row(rowIndex, row));
    public GridBuilder<T>.Row GetRow(long index) => new(index, grid[index]);

    public bool IsIncluded(Coordinate2D position)
    {
        return items?.ContainsKey(position) ?? false;
    }

    public T GetValue(Coordinate2D position)
    {
        if (!IsWithinBounds(position))
        {
            throw new ArgumentOutOfRangeException("Position is out of bounds");
        }

        return grid[position.Row][position.Col];
    }

    public bool IsWithinBounds(Coordinate2D position) => position.Row >= 0 && position.Row <= MaxRowIndex && position.Col >= 0 && position.Col <= MaxColIndex;

    public void SetStartPosition(Coordinate2D position)
    {
        startPosition = position;
    }

    public void SetEndPosition(Coordinate2D position)
    {
        endPosition = position;
    }
}

public class GridBuilder<T> where T : struct
{
    private T? _startPositionValue;
    private T? _endPositionValue;
    private HashSet<T>? _includedValues;
    private HashSet<T>? _excludedValues;

    public GridBuilder<T> WithStartPosition(T value)
    {
        _startPositionValue = value;
        return this;
    }

    public GridBuilder<T> WithEndPosition(T value)
    {
        _endPositionValue = value;
        return this;
    }

    public GridBuilder<T> IncludeValues(T[] value)
    {
        _includedValues = value.ToHashSet();
        return this;
    }

    public GridBuilder<T> ExcludeValues(T[] value)
    {
        _excludedValues = value.ToHashSet();
        return this;
    }

    public Grid<T> Build(string input)
    {
        var rows = input.SplitByLine();
        T[][] grid = new T[rows.LongLength][];
        Coordinate2D? startPosition = null;
        Coordinate2D? endPosition = null;

        var items = new Dictionary<Coordinate2D, T>();
        for (var row = 0; row < rows.LongLength; row++)
        {
            var colValues = new T[rows[row].Length];
            for (var col = 0; col < rows[row].Length; col++)
            {
                var colValue = ParseColValue(rows[row][col]);
                colValues[col] = colValue;

                if ((_includedValues == null || _includedValues.Contains(colValue)) && (_excludedValues == null || !_excludedValues.Contains(colValue)))
                {
                    items.Add(new Coordinate2D(row, col), colValue);
                }

                if (_startPositionValue.HasValue && colValue.Equals(_startPositionValue.Value))
                {
                    if (startPosition == null)
                    {
                        startPosition = new Coordinate2D(row, col);
                    }
                    else
                    {
                        throw new ArgumentException("Start position value is contained more than once");
                    }
                }

                if (_endPositionValue != null && colValue.Equals(_endPositionValue))
                {
                    if (endPosition == null)
                    {
                        endPosition = new Coordinate2D(row, col);
                    }
                    else
                    {
                        throw new ArgumentException("End position value is contained more than once");
                    }
                }
            }

            grid[row] = colValues;
        }

        return new Grid<T>(grid, items, startPosition, endPosition);
    }

    private static T ParseColValue(char colValue)
    {
        // Check if T is int or char
        if (typeof(T) != typeof(int) && typeof(T) != typeof(char))
        {
            throw new ArgumentException($"Type {typeof(T)} is not supported");
        }

        if (typeof(T) == typeof(char))
        {
            return (T)(object)colValue;
        }

        if (typeof(T) == typeof(int))
        {
            return (T)(object)(colValue - '0');
        }

        throw new InvalidOperationException("Unexpected type.");
    }

    public class Row(long index, T[] cols)
    {
        public long Index => index;
        public IEnumerable<Col> Cols { get; } = cols.Select((val, col) => new Col(new Coordinate2D(index, col), val));
    }

    public class Col(Coordinate2D position, T value)
    {
        public Coordinate2D Position { get; } = position;
        public T Value { get; } = value;
    }
}
