namespace AOC23;

internal class Day01 : AocDay {

    public override void Run() {
        var strings = Data!.Split("\r\n").ToArray();

        Console.WriteLine(Solve1(strings));
        // 53974

        Console.WriteLine(Solve2(strings));
        // 52840
    }

    private int Solve1(string[] strings) {
        return strings.Select(v => GetCalibrationValue1(v)).Sum();
    }
    private int Solve2(string[] strings) {
        return strings.Select(v => GetCalibrationValue2(v)).Sum();
    }

    private int GetCalibrationValue1(string s) {
        var first = s.Where(c => char.IsDigit(c)).First();
        var last = s.Where(c => char.IsDigit(c)).Last();
        var num = $"{first}{last}";
        return int.Parse(num);
    }

    private int GetCalibrationValue2(string s) {
        Dictionary<string, char> digits = new Dictionary<string, char> {
            { "one", '1' },
            { "two", '2' },
            { "three", '3' },
            { "four", '4' },
            { "five", '5' },
            { "six", '6' },
            { "seven", '7' },
            { "eight", '8' },
            { "nine", '9' },
            { "0", '0' },
            { "1", '1' },
            { "2", '2' },
            { "3", '3' },
            { "4", '4' },
            { "5", '5' },
            { "6", '6' },
            { "7", '7' },
            { "8", '8' },
            { "9", '9' },
        };
        var first = digits.Keys
            .Select(k => (digits[k], s.IndexOf(k)))
            .Where(ki => ki.Item2 != -1)
            .OrderBy(ki => ki.Item2)
            .Select(ki => ki.Item1)
            .Take(1)
            .ToArray()[0];
        var last = digits.Keys
            .Select(k => (digits[k], s.LastIndexOf(k)))
            .Where(ki => ki.Item2 != -1)
            .OrderBy(ki => ki.Item2)
            .Select(ki => ki.Item1)
            .TakeLast(1)
            .ToArray()[0];
        var num = $"{first}{last}";
        return int.Parse(num);
    }
}
