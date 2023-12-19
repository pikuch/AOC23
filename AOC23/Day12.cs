namespace AOC23;

internal class Day12 : AocDay {
    internal class Row {
        string Bits;
        int[] Groups;
        Dictionary<(int, int), long> Seen = new();
        public Row(string line, bool makeBig) {
            var parts = line.Split(' ', StringSplitOptions.TrimEntries).ToArray();
            if (makeBig) {
                parts[0] = parts[0] + "?" + parts[0] + "?"+ parts[0] + "?" + parts[0] + "?" + parts[0];
                parts[1] = parts[1] + "," + parts[1] + "," + parts[1] + "," + parts[1] + "," + parts[1];
            }
            Bits = parts[0];
            Groups = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToArray();
        }

        internal long CountCombinations() {
            long counts = 0;
            for (int i=0; i<Bits.Length; i++) {
                if (Agrees(0, i, '.')) {
                    counts += CountFrom(0, i);
                }
            }
            return counts;
        }

        internal long CountFrom(int group, int offset) {
            if (Seen.ContainsKey((group, offset))) {
                return Seen[(group, offset)];
            }
            long count = 0;
            // last group
            if (group == Groups.Length - 1) {
                if (Agrees(offset, Groups[group], '#') && Agrees(offset + Groups[group], Bits.Length - (offset + Groups[group]), '.')) {
                    count = 1;
                }
            }
            else {
                if (Agrees(offset, Groups[group], '#')) {
                    for (int newOffset = offset + Groups[group] + 1; newOffset < Bits.Length; newOffset++) {
                        if (Agrees(offset + Groups[group], newOffset - (offset + Groups[group]), '.')) {
                            count += CountFrom(group+1, newOffset);
                        }
                    }
                }
            }

            Seen[(group, offset)] = count;
            return count;
        }

        private bool Agrees(int offset, int length, char c) {
            if (offset + length > Bits.Length) {
                return false;
            }
            for (int o=0; o<length; o++) {
                if (Bits[offset + o] != '?' && Bits[offset + o] != c) {
                    return false;
                }
            }
            return true;
        }
    }

    public override void Run() {
        var rows = Data!.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(line => new Row(line, false)).ToArray();
        var bigRows = Data!.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(line => new Row(line, true)).ToArray();

        var result1 = rows.Select(row => row.CountCombinations()).Sum();

        Console.WriteLine(result1);
        // 7674

        var result2 = bigRows.Select(row => row.CountCombinations()).Sum();

        Console.WriteLine(result2);
        // 
    }
}
