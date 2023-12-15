namespace AdventOfCode2023.Days;

public class Day15 : DayBase
{
    public override ValueTask<string> Solve_1()
    {
        var steps = Input.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Replace("\n", string.Empty));
        var verificationNumber = steps.Sum(GetHash);

        return new ValueTask<string>(verificationNumber.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var steps = Input.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Replace("\n", string.Empty));
        var boxes = Enumerable.Range(0, 256).Select(_ => new Box()).ToArray();

        foreach (var step in steps)
        {
            if (step.Contains('-'))
            {
                var label = step[..step.IndexOf('-')];
                var boxNumber = GetHash(label);
                var lensSlot = boxes[boxNumber].LensSlots.FirstOrDefault(s => s.Label == label);
                if (lensSlot != null)
                {
                    boxes[boxNumber].LensSlots.Remove(lensSlot);
                }
            }
            else
            {
                var split = step.Split('=');
                var label = split[0];
                var focalLength = int.Parse(split[1]);
                var boxNumber = GetHash(label);
                var lensSlot = boxes[boxNumber].LensSlots.FirstOrDefault(s => s.Label == label);
                if (lensSlot != null)
                {
                    lensSlot.FocalLength = focalLength;
                }
                else
                {
                    boxes[boxNumber].LensSlots.Add(new LensSlot(label, focalLength));
                }
            }
        }

        var totalFocusingPower = 0;
        for (var i = 0; i < boxes.Length; i++)
        {
            for (var j = 0; j < boxes[i].LensSlots.Count; j++)
            {
                var focusingPower = i + 1;
                focusingPower *= (j + 1) * boxes[i].LensSlots[j].FocalLength;

                totalFocusingPower += focusingPower;
            }
        }

        return new ValueTask<string>(totalFocusingPower.ToString());
    }

    private static int GetHash(string step)
    {
        var currentValue = 0;
        foreach (var c in step)
        {
            currentValue += c;
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }

    private sealed class Box
    {
        public List<LensSlot> LensSlots { get; } = new();
    }

    private sealed class LensSlot(string label, int focalLength)
    {
        public string Label { get; init; } = label;

        public int FocalLength { get; set; } = focalLength;
    }
}
