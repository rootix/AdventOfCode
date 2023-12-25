namespace AdventOfCode2023;

public abstract class DayBase : BaseDay
{
    protected DayBase()
    {
        Input = new Lazy<string>(() => File.ReadAllText(InputFilePath));
    }

    public bool IsSample { get; set; }
    public string? OverrideFileDirPath { get; set; }
    protected override string InputFileDirPath => OverrideFileDirPath ?? base.InputFileDirPath;
    protected Lazy<string> Input { get; private set; }

    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>("Not solved");
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>("Not solved");
    }
}
