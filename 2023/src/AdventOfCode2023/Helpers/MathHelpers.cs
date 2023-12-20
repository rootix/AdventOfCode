namespace AdventOfCode2023.Helpers;

public static class MathHelpers
{
    public static long CalculateLeastCommonMultiple(long a, long b) => a * b / CalculateGreatestCommonDivisor(a, b);

    public static long CalculateGreatestCommonDivisor(long a, long b) => b == 0 ? a : CalculateGreatestCommonDivisor(b, a % b);
}
