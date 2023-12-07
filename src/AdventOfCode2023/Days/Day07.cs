namespace AdventOfCode2023.Days;

public class Day07 : DayBase
{
    private static readonly Dictionary<char, int> CardValuesPart1 = new()
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'J', 11 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 },
    };

    private static readonly Dictionary<char, int> CardValuesPart2 = new()
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 },
        { 'J', 1 },
    };

    public override ValueTask<string> Solve_1()
    {
        var hands = ParseHands(true);
        var sum = GetSum(hands, CardValuesPart1);

        return new ValueTask<string>(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var hands = ParseHands(false);
        var sum = GetSum(hands, CardValuesPart2);

        return new ValueTask<string>(sum.ToString());
    }

    private IEnumerable<Hand> ParseHands(bool part1)
    {
        foreach (var line in Input.Value.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = line.Split(' ', StringSplitOptions.TrimEntries);
            var handString = parts[0];
            var bid = int.Parse(parts[1]);
            var type = part1 ? EvaluateTypeForHand(handString) : EvaluateTypeForHandWithJ(handString);

            yield return new Hand(handString, type, bid);
        }
    }

    private static Type EvaluateTypeForHand(string hand) =>
        hand.GroupBy(h => h).OrderByDescending(g => g.Count()).ToList() switch
        {
            { Count: 1 } => Type.FiveOfAKind,
            { Count: 2 } g when g.First().Count() == 4 => Type.FourOfAKind,
            { Count: 2 } g when g.First().Count() == 3 => Type.FullHouse,
            { Count: 3 } g when g.First().Count() == 3 => Type.ThreeOfAKind,
            { Count: 3 } g when g.ElementAt(1).Count() == 2 => Type.TwoPair,
            { Count: 4 } => Type.OnePair,
            { Count: 5 } => Type.HighCard,
            _ => Type.Invalid
        };

    private static Type EvaluateTypeForHandWithJ(string hand)
    {
        if (!hand.Contains('J'))
        {
            return EvaluateTypeForHand(hand);
        }

        return hand.Count(c => c == 'J') switch
        {
            5 => Type.FiveOfAKind,
            _ when hand.GroupBy(c => c).Count() == 2 => Type.FiveOfAKind,
            2 or 3 when hand.GroupBy(c => c).Count() == 3 => Type.FourOfAKind,
            2 when hand.GroupBy(c => c).Count() == 4 => Type.ThreeOfAKind,
            1 when hand.GroupBy(c => c).Count() == 5 => Type.OnePair,
            1 when hand.GroupBy(c => c).Count() == 4 => Type.ThreeOfAKind,
            1 when hand.GroupBy(c => c).Count() == 3 =>
                hand.GroupBy(c => c).OrderByDescending(g => g.Count()).First().Count() == 3
                    ? Type.FourOfAKind
                    : Type.FullHouse,
            _ => Type.Invalid
        };
    }

    private static int GetSum(IEnumerable<Hand> hands, IReadOnlyDictionary<char, int> cardValues)
    {
        return hands.Order(new HandComparer(cardValues))
            .Select((hand, idx) => hand.Bid * (idx + 1))
            .Sum();
    }

    private sealed record Hand(string Cards, Type Type, int Bid);

    private enum Type
    {
        Invalid,
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

	private sealed class HandComparer(IReadOnlyDictionary<char, int> cardValues) : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            if (x == null || y == null)
            {
                throw new NotSupportedException("This is not expected in AoC :D");
            }

            if (x.Type != y.Type)
            {
                return x.Type - y.Type;
            }

            for (var i = 0; i < x.Cards.Length; i++)
            {
                var cardValueX = cardValues[x.Cards[i]];
                var cardValueY = cardValues[y.Cards[i]];
                if (cardValueX != cardValueY)
                {
                    return cardValueX - cardValueY;
                }
            }

            return 0;
        }
    }
}
