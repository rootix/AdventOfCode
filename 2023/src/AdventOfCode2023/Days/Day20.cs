namespace AdventOfCode2023.Days;

public class Day20 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var modules = ParseModules();
        var lowPulses = 0;
        var highPulses = 0;

        for (var i = 0; i < 1000; i++)
        {
            var pulseQueue = new Queue<(string From, string To, bool Pulse)>();
            pulseQueue.Enqueue(("button", "broadcaster", false));

            while (pulseQueue.Count > 0)
            {
                var p = pulseQueue.Dequeue();
                if (p.Pulse)
                {
                    highPulses++;
                }
                else
                {
                    lowPulses++;
                }

                foreach (var nextPulse in Pulse(modules, p.From, p.To, p.Pulse))
                {
                    pulseQueue.Enqueue((nextPulse.From, nextPulse.To, nextPulse.Pulse));
                }
            }
        }

        return new ValueTask<string>((lowPulses * highPulses).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var modules = ParseModules();
        var rxSource = modules.FirstOrDefault(m => m.Value.Targets.Contains("rx"));
        var conjunctionSources = rxSource.Value.SourcePulses.Keys;
        var conjunctionSourceCycles = conjunctionSources.ToDictionary(s => s, _ => new List<long>());
        var buttonPresses = 0;
        long result;

        while (true)
        {
            buttonPresses++;

            var pulseQueue = new Queue<(string From, string To, bool Pulse)>();
            pulseQueue.Enqueue(("button", "broadcaster", false));

            while (pulseQueue.Count > 0)
            {
                var p = pulseQueue.Dequeue();
                foreach (var nextPulse in Pulse(modules, p.From, p.To, p.Pulse))
                {
                    pulseQueue.Enqueue((nextPulse.From, nextPulse.To, nextPulse.Pulse));
                }

                if (conjunctionSourceCycles.TryGetValue(p.To, out var cycles) &&
                    cycles.Count < 2 &&
                    modules[p.To].SourcePulses.Values.Any(p => !p))
                {
                    cycles.Add(buttonPresses);
                }
            }

            if (conjunctionSourceCycles.All(c => c.Value.Count == 2))
            {
                var allCycles = conjunctionSourceCycles.Select(c => c.Value[1] - c.Value[0]);
                result = allCycles.Aggregate(MathHelpers.CalculateLeastCommonMultiple);
                break;
            }
        }

        return new ValueTask<string>(result.ToString());
    }

    private Dictionary<string, Module> ParseModules()
    {
        Dictionary<string, Module> modules = [];
        foreach (var module in Input.Value.SplitByLine())
        {
            var parts = module.Split(" -> ", StringSplitOptions.TrimEntries);
            var targets = parts[1].Split(',', StringSplitOptions.TrimEntries);
            if (char.IsAsciiLetter(parts[0][0]))
            {
                var type = 'b';
                var name = parts[0];
                modules.Add(name, new Module(name, type, targets));
            }
            else
            {
                var type = parts[0][0];
                var name = parts[0][1..];
                modules.Add(name, new Module(name, type, targets));
            }
        }

        foreach (var m in modules.Values.Where(m => m.Type == '&'))
        {
            foreach (var source in modules.Values.Where(m2 => m2.Targets.Any(t => t == m.Name)))
            {
                m.SourcePulses.Add(source.Name, false);
            }
        }

        return modules;
    }

    private static IEnumerable<(string From, string To, bool Pulse)> Pulse(IReadOnlyDictionary<string, Module> modules, string from, string to, bool pulse)
    {
        if (!modules.TryGetValue(to, out var m))
        {
            yield break;
        }

        var nextPulse = false;
        switch (m.Type)
        {
            case '%' when pulse:
                yield break;
            case '%':
                m.IsOn = !m.IsOn;
                nextPulse = m.IsOn;
                break;
            case '&':
                m.SourcePulses[from] = pulse;
                nextPulse = m.SourcePulses.Values.Any(p => !p);
                break;
        }

        foreach (var target in m.Targets)
        {
            yield return (m.Name, target, nextPulse);
        }
    }

    private sealed class Module(string name, char type, string[] targets)
    {
        public string Name { get; } = name;
        public char Type { get; } = type;
        public string[] Targets { get; } = targets;
        public bool IsOn { get; set; } // Flip-Flop
        public Dictionary<string, bool> SourcePulses { get; } = new(); // Conjunction
    }
}
