namespace AOC23;

internal class Day05 : AocDay {

    internal class Mapping {
        List<(long low, long high, long delta)> Ranges { get; set; } = new();
        public Mapping(string chunk) {
            Ranges.Add((long.MinValue, long.MaxValue, 0L));
            var lines = chunk.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines.Skip(1)) {
                var items = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
                long destStart = long.Parse(items[0]);
                long sourceStart = long.Parse(items[1]);
                long rangeLength = long.Parse(items[2]);
                long delta = destStart - sourceStart;
                AddNewRange(sourceStart, sourceStart + rangeLength, delta);
            };
        }

        public long Map(long value) {
            foreach (var range in Ranges) {
                if (value >= range.low && value < range.high) {
                    return value += range.delta;
                }
            }
            throw new Exception("Missing range");
        }

        private void AddNewRange(long low, long high, long delta) {
            int i;
            for (i = 0; i < Ranges.Count; i++) {
                if (Ranges[i].low <= low && Ranges[i].high >= high) {
                    break;
                }
            }
            List<(long low, long high, long delta)> newRanges = new();
            newRanges.AddRange(Ranges.Take(i));
            if (Ranges[i].low != low) {
                newRanges.Add((Ranges[i].low, low, Ranges[i].delta));
            }
            newRanges.Add((low, high, delta));
            if (high != Ranges[i].high) {
                newRanges.Add((high, Ranges[i].high, Ranges[i].delta));
            }
            newRanges.AddRange(Ranges.Skip(i+1));

            Ranges = newRanges;
        }
    }

    public override void Run() {
        var lines = Data!.Split("\r\n");
        long[] seeds = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(n => long.Parse(n)).ToArray();

        var mappings = Data.Split("\r\n\r\n", StringSplitOptions.TrimEntries).Skip(1).Select(chunk => new Mapping(chunk)).ToArray();

        Console.WriteLine(Solve1(seeds, mappings));
        // 178159714

        Console.WriteLine(Solve2(seeds, mappings));
        // 100165128

        // TODO: compress ranges
    }

    private long Solve2(long[] seeds, Mapping[] mappings) {
        List<long> mins = new();
        for (int i=2; i<seeds.Length/2; i++) {
            long start = seeds[i * 2];
            long length = seeds[i * 2 + 1];
            Console.WriteLine($"Testing range {i} (length {length})");
            long localMin = long.MaxValue;
            for (long x = start; x < start + length; x++) {
                long result = MapAll(x, mappings);
                if (result < localMin) {
                    localMin = result;
                }
            }
            
            Console.WriteLine($"Got {localMin}");
            mins.Add(localMin);
        }
        return mins.Min();
    }

    private long Solve1(long[] seeds, Mapping[] mappings) {
        return seeds.Select(seed => MapAll(seed, mappings)).Min();
    }

    private long MapAll(long value, Mapping[] mappings) {
        foreach (var m in mappings) {
            value = m.Map(value);
        }
        return value;
    }
}
