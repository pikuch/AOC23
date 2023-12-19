namespace AOC23;

internal class Day06 : AocDay {

    internal class Race {
        public long Time { get; set; }
        public long Best { get; set; }
        public Race(long time, long best) {
            Time = time;
            Best = best;
        }
        public bool IsWinning(long n) => (Time - n) * n > Best;

        private long FindLowest() {
            long low = 1;
            long high = Time / 2;
            while (high-low > 1) {
                var mid = low + (high - low) / 2;
                if (IsWinning(mid)) {
                    high = mid;
                }
                else {
                    low = mid;
                }
            }
            return high;
        }

        private long FindHighest() {
            long low = Time / 2;
            long high = Time;
            while (high - low > 1) {
                var mid = low + (high - low) / 2;
                if (IsWinning(mid)) {
                    low = mid;
                }
                else {
                    high = mid;
                }
            }
            return high;
        }

        public long CountWaysToWin() {
            long low = FindLowest();
            long high = FindHighest();
            return high - low;
        }
    }

    public override void Run() {
        var lines = Data!.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        var times = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(time => long.Parse(time)).ToArray();
        var distances = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(dist => long.Parse(dist)).ToArray();
        var oneTime = lines[0].Replace(" ", "").Split(':', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(time => long.Parse(time)).ToArray()[0];
        var oneDistance = lines[1].Replace(" ", "").Split(':', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(time => long.Parse(time)).ToArray()[0];

        var races = times.Zip(distances).Select(td => new Race(td.First, td.Second)).ToArray();
        var waysToWin = races.Select(race => race.CountWaysToWin()).Aggregate((r1, r2) => r1 * r2);

        Console.WriteLine(waysToWin);
        // 131376

        var theRace = new Race(oneTime, oneDistance);

        Console.WriteLine(theRace.CountWaysToWin());
        // 34123437
    }
}
