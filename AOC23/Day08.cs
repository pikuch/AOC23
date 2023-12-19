namespace AOC23;

internal class Day08 : AocDay {
    Dictionary<string, (string, string)> Nodes { get; set; } = new();
    string Directions { get; set; }

    public override void Run() {
        var parts = Data!.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
        Directions = parts[0];
        Nodes = parts[1].Split("\r\n").ToDictionary(line => line[0..3], line => (line[7..10], line[12..15]));

        Console.WriteLine(GetPeriod("AAA"));
        // 17873

        var startNodes = Nodes.Keys.Where(key => key[2] == 'A');
        long[] periods = startNodes.Select(node => (long)GetPeriod(node)).ToArray();
        long commonPeriod = periods.Aggregate((x, y) => Lcm(x, y));

        Console.WriteLine(commonPeriod);
        // 15746133679061
    }

    private int GetPeriod(string node) {
        int steps = 0;
        var current = node;
        int pos = 0;
        while (current[2] != 'Z') {
            current = (Directions[pos % Directions.Length] == 'L') ? Nodes[current].Item1 : Nodes[current].Item2;
            pos++;
            steps++;
        }
        return steps;
    }

    static long Gcf(long x, long y) {
        while (y != 0) {
            long temp = y;
            y = x % y;
            x = temp;
        }
        return x;
    }

    static long Lcm(long x, long y) {
        return x / Gcf(x, y) * y;
    }
}
