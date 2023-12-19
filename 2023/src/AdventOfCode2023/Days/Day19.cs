using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

public class Day19 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var (workflows, ratings) = ParseInput();
        var sum = 0;

        foreach (var rating in ratings)
        {
            var workflowStep = workflows["in"];
            var workflowCompleted = false;
            while (!workflowCompleted)
            {
                foreach (var step in workflowStep)
                {
                    if (!step.Category.HasValue)
                    {
                        HandleTargetStep();
                        break;
                    }

                    var value = GetValue(step.Category.Value, rating);
                    if (step.Operation == '<')
                    {
                        if (value < step.ConditionValue)
                        {
                            HandleTargetStep();
                            break;
                        }
                    }
                    else
                    {
                        if (value > step.ConditionValue)
                        {
                            HandleTargetStep();
                            break;
                        }
                    }

                    void HandleTargetStep()
                    {
                        if (step.TargetStep == "A")
                        {
                            sum += rating.Sum;
                            workflowCompleted = true;
                        }
                        else if (step.TargetStep == "R")
                        {
                            workflowCompleted = true;
                        }
                        else
                        {
                            workflowStep = workflows[step.TargetStep];
                        }
                    }
                }
            }
        }

        return new ValueTask<string>(sum.ToString());

        int GetValue(char category, Rating rating) => category switch
        {
            'x' => rating.X,
            'm' => rating.M,
            'a' => rating.A,
            's' => rating.S,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override ValueTask<string> Solve_2()
    {
        var (workflows, _) = ParseInput();
        var sum = GetRatingCombinations(workflows, "in",
            new Dictionary<char, (int From, int To)>
            {
                { 'x', (1, 4000) },
                { 'm', (1, 4000) },
                { 'a', (1, 4000) },
                { 's', (1, 4000) }
            });

        return new ValueTask<string>(sum.ToString());
    }

    // Thanks @kevinbrechbuehl (https://github.com/kevinbrechbuehl/advent-of-code/blob/main/2023/19/puzzle.ts#L99C18-L99C18)
    private static long GetRatingCombinations(IReadOnlyDictionary<string, IReadOnlyList<WorkflowStep>> workflows, string workflowName, IDictionary<char, (int From, int To)> ranges)
    {
        if (workflowName == "A")
        {
            return (long)(ranges['x'].To - ranges['x'].From + 1) *
                   (ranges['m'].To - ranges['m'].From + 1) *
                   (ranges['a'].To - ranges['a'].From + 1) *
                   (ranges['s'].To - ranges['s'].From + 1);
        }

        if (workflowName == "R")
        {
            return 0;
        }

        long sum = 0;
        var workflow = workflows[workflowName];
        foreach (var step in workflow)
        {
            if (!step.Category.HasValue)
            {
                sum += GetRatingCombinations(workflows, step.TargetStep, ranges);
            }
            else
            {
                var range = ranges[step.Category.Value];
                if (range.From < step.ConditionValue && range.To > step.ConditionValue)
                {
                    var newRanges = ranges.ToDictionary(r => r.Key, r => r.Value);
                    if (step.Operation == '<')
                    {
                        newRanges[step.Category.Value] = (range.From, step.ConditionValue.Value - 1);
                        ranges[step.Category.Value] = (step.ConditionValue.Value, range.To);
                    }
                    else
                    {
                        newRanges[step.Category.Value] = (step.ConditionValue.Value + 1, range.To);
                        ranges[step.Category.Value] = (range.From, step.ConditionValue.Value);
                    }

                    sum += GetRatingCombinations(workflows, step.TargetStep, newRanges);
                }
            }
        }

        return sum;
    }

    private (IReadOnlyDictionary<string, IReadOnlyList<WorkflowStep>> Workflows, IReadOnlyList<Rating> Ratings) ParseInput()
    {
        var groups = Input.Value.SplitByGroup();
        Dictionary<string, IReadOnlyList<WorkflowStep>> workflows = [];
        foreach (var w in groups[0].SplitByLine())
        {
            var startStepIndex = w.IndexOf('{');
            var endStepIndex = w.IndexOf('}');
            var name = w[..startStepIndex];
            var stepsString = w[(startStepIndex + 1)..endStepIndex];
            var steps = new List<WorkflowStep>();
            foreach (var step in stepsString.Split(','))
            {
                var doublePointIndex = step.IndexOf(':');
                if (doublePointIndex != -1)
                {
                    var category = step[0];
                    var operation = step[1];
                    var conditionValue = int.Parse(step[2..doublePointIndex]);
                    var target = step[(doublePointIndex + 1)..];
                    steps.Add(new WorkflowStep(category, operation, conditionValue, target));
                }
                else
                {
                    steps.Add(new WorkflowStep(null, null, null, step));
                }
            }

            workflows.Add(name, steps);
        }

        var ratingRegex = new Regex(@"{x=(\d*),m=(\d*),a=(\d*),s=(\d*)}");
        var ratings = new List<Rating>();
        foreach (var r in groups[1].SplitByLine())
        {
            var match = ratingRegex.Match(r);
            ratings.Add(new Rating(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
        }

        return (workflows, ratings);
    }

    private sealed record WorkflowStep(char? Category, char? Operation, int? ConditionValue, string TargetStep);
    private sealed record Rating(int X, int M, int A, int S)
    {
        public int Sum => X + M + A + S;
    }
}
