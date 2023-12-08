namespace AdventOfCode2023.Days;

public class Day02 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var games = ParseGames();
        var sum = games
            .Where(g => g.Rounds.All(r => r is { Red: <= 12, Green: <= 13, Blue: <= 14 }))
            .Select(g => g.Id)
            .Sum();

        return new ValueTask<string>(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var games = ParseGames();
        var sum = games
            .Select(g =>
            {
                var maxRed = g.Rounds.Select(r => r.Red).Max();
                var maxGreen = g.Rounds.Select(r => r.Green).Max();
                var maxBlue = g.Rounds.Select(r => r.Blue).Max();
                return maxRed * maxGreen * maxBlue;
            })
            .Sum();

        return new ValueTask<string>(sum.ToString());
    }

    private Game[] ParseGames()
    {
        var games = new List<Game>();
        var gameLines = Input.Value.SplitByLine();
        foreach (var gameLine in gameLines)
        {
            var parts = gameLine.Split(':', StringSplitOptions.TrimEntries);
            var gameId = int.Parse(parts[0]["Game ".Length..]);

            var roundsString = parts[1].Split(';', StringSplitOptions.TrimEntries);
            var rounds = new List<Round>();
            foreach (var roundString in roundsString)
            {
                var red = 0;
                var green = 0;
                var blue = 0;
                var colorsWithAmount = roundString.Split(',', StringSplitOptions.TrimEntries);
                foreach (var colorWithAmountString in colorsWithAmount)
                {
                    var colorWithAmount = colorWithAmountString.SplitByWhitespace();
                    var amount = int.Parse(colorWithAmount[0]);
                    switch (colorWithAmount[1])
                    {
                        case "red":
                            red += amount;
                            break;
                        case "green":
                            green += amount;
                            break;
                        case "blue":
                            blue += amount;
                            break;
                    }
                }

                rounds.Add(new Round(red, green, blue));
            }

            games.Add(new Game(gameId, [.. rounds]));
        }

        return [.. games];
    }

    private sealed record Game(int Id, Round[] Rounds);

    private sealed record Round(int Red, int Green, int Blue);
}
