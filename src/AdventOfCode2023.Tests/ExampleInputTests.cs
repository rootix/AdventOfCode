using AdventOfCode2023.Days;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AdventOfCode2023.Tests;

public class ExampleInputTests
{
    [Theory]
    [InlineData(typeof(Day01), "142", "281")]
    [InlineData(typeof(Day02), "8", "2286")]
    [InlineData(typeof(Day03), "4361", "467835")]
    [InlineData(typeof(Day04), "13", "30")]
    [InlineData(typeof(Day05), "35", "46")]
    public async Task TestDay(Type type, string expectedSolution1 = "Not solved", string expectedSolution2 = "Not solved")
    {
        if (Activator.CreateInstance(type) is DayBase instance)
        {
            instance.OverrideFileDirPath = "ExampleInputs";

            using (new AssertionScope())
            {
                var actualSolution1 = await instance.Solve_1();
                actualSolution1.Should().Be(expectedSolution1);

                var actualSolution2 = await instance.Solve_2();
                actualSolution2.Should().Be(expectedSolution2);
            }
        }
        else
        {
            Assert.Fail("Test class does not inherit from DayBase");
        }
    }
}
