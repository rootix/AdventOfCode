﻿using AdventOfCode2023.Days;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AdventOfCode2023.Tests;

public class ExampleInputTests
{
    [Theory]
    [InlineData(typeof(Day01), "54667", "54203")] // After solving: Real input due to different samples for 1 & 2
    [InlineData(typeof(Day02), "8", "2286")]
    [InlineData(typeof(Day03), "4361", "467835")]
    [InlineData(typeof(Day04), "13", "30")]
    [InlineData(typeof(Day05), "35", "46")]
    [InlineData(typeof(Day06), "288", "71503")]
    [InlineData(typeof(Day07), "6440", "5905")]
    [InlineData(typeof(Day08), "2", "2")]
    [InlineData(typeof(Day09), "114", "2")]
    [InlineData(typeof(Day10), "80", "10")]
    [InlineData(typeof(Day11), "374", "82000210")]
    [InlineData(typeof(Day12), "21", "525152")]
    [InlineData(typeof(Day13), "405", "400")]
    [InlineData(typeof(Day14), "136", "64")]
    [InlineData(typeof(Day15), "1320", "145")]
    [InlineData(typeof(Day16), "46", "51")]
    [InlineData(typeof(Day17), "102", "94")]
    [InlineData(typeof(Day18), "62", "952408144115")]
    [InlineData(typeof(Day19), "19114", "167409079868000")]
    [InlineData(typeof(Day20), "703315117", "230402300925361")] // After solving: Real input due to no sample for 2
    [InlineData(typeof(Day21), "16", "468883362249322")] // Extrapolated solution 2 as there is none provided
    [InlineData(typeof(Day22), "5", "7")]
    [InlineData(typeof(Day23), "94", "154")]
    [InlineData(typeof(Day24), "2", "47")]
    [InlineData(typeof(Day25), "54")]
    public async Task TestDay(Type type, string expectedSolution1 = "Not solved", string expectedSolution2 = "Not solved")
    {
        if (Activator.CreateInstance(type) is DayBase instance)
        {
            instance.IsSample = true;
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
