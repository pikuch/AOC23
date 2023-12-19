namespace AOC23;

internal class Day09 : AocDay {
    internal class Series {
        public List<long> Values = new List<long>();
        public Series(string line) {
            Values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(item => long.Parse(item)).ToList();
        }
        public long GetNextValue(List<long> input) => (input.All(x => x == 0)) ? 0 : input.Last() + GetNextValue(GetDiffs(input));
        public long GetPreviousValue(List<long> input) => (input.All(x => x == 0)) ? 0 : input[0] - GetPreviousValue(GetDiffs(input));
        private List<long> GetDiffs(List<long> input) => input.Zip(input.Skip(1)).Select(pair => pair.Second - pair.First).ToList();
    }

    public override void Run() {
        var series = Data!.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(line => new Series(line)).ToList();

        Console.WriteLine(series.Select(series => series.GetNextValue(series.Values)).Sum());
        // 1901217887

        Console.WriteLine(series.Select(series => series.GetPreviousValue(series.Values)).Sum());
        // 905
    }
}
