namespace AdventOfCode2023.Days;

public class Day04 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var allPoints = ParseCards()
            .Select(card => card.Numbers.Intersect(card.WinningNumbers).Count())
            .Select(matchingNumbers => (int)Math.Pow(2, matchingNumbers - 1))
            .Sum();

        return new ValueTask<string>(allPoints.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var cards = ParseCards().ToArray();
        var stack = cards.ToDictionary(c => c, _ => 1);
        for (var i = 0; i < cards.Length; i++)
        {
            var card = cards[i];
            var matchingNumbers = card.Numbers.Intersect(card.WinningNumbers).Count();
            for (var j = i + 1; j <= i + matchingNumbers; j++)
            {
                stack[cards[j]] += stack[card];
            }
        }

        return new ValueTask<string>(stack.Values.Sum().ToString());
    }

    private IEnumerable<Card> ParseCards()
    {
        foreach (var cardString in Input.Value.SplitByLine())
        {
            var parts = cardString.Split('|', StringSplitOptions.TrimEntries);
            var winningNumbers = parts[0].SplitByWhitespace().ToHashSet();
            var numbers = parts[1].SplitByWhitespace().ToArray();

            yield return new Card(winningNumbers, numbers);
        }
    }

    private sealed record Card(HashSet<string> WinningNumbers, string[] Numbers);
}
