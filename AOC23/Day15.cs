namespace AOC23;

internal class Day15 : AocDay {
    string[] Steps;
    List<(string label, int focal)>[] Boxes = new List<(string label, int focal)>[256];

    public override void Run() {
        Steps = Data!.Split(',').ToArray();

        var result1 = Steps.Select(s => GetHash(s)).Sum();
        Console.WriteLine(result1);
        // 514281

        DoOperations();
        var result2 = GetFocusingPower();
        Console.WriteLine(result2);
        // 244199
    }

    private void DoOperations() {
        for (int i = 0; i < 256; i++) {
            Boxes[i] = new List<(string label, int focal)>();
        }

        foreach (var step in Steps) {
            string label;
            string op;
            int value = 0;
            int hash;

            if (step.EndsWith('-')) {
                label = step[0..(step.Length - 1)];
                hash = GetHash(label);
                Boxes[hash] = Boxes[hash].Where(item => item.label != label).ToList();
            }
            else {
                var parts = step.Split('=');
                label = parts[0];
                value = int.Parse(parts[1]);
                hash = GetHash(label);
                var i = Boxes[hash].FindIndex(item => item.label == label);
                if (i >= 0) {
                    Boxes[hash][i] = (label, value);
                }
                else {
                    Boxes[hash].Add((label, value));
                }
            }
        }
    }

    private int GetFocusingPower() => Boxes.SelectMany((b, i) => b.Select((lens, j) => (i + 1) * (j + 1) * lens.focal)).Sum();

    private int GetHash(string s) {
        int x = 0;
        foreach (var c in s) {
            x = (x + (byte)c) * 17 % 256;
        }
        return x;
    }
}
