namespace AOC23;

internal class Day13 : AocDay {
    internal class Pattern {
        string[] lines;
        public Pattern(string chunk) {
            lines = chunk.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToArray();
        }

        internal int GetReflectionValue(int errors) {
            int v = FindVertical(errors);
            if (v != 0) {
                return v;
            }
            int h = FindHorizontal(errors) * 100;
            return h;
        }

        private int FindVertical(int errorCount) {
            for (var col = 1; col < lines[0].Length; col++) {
                int errors = 0;
                for (var row = 0; row < lines.Length; row++) {
                    for (var dist = 0; dist < lines[0].Length; dist++) {
                        int c1 = col + dist;
                        int c2 = col - 1 - dist;
                        if (c1 >= lines[0].Length || c2 < 0) {
                            break;
                        }
                        if (lines[row][c1] != lines[row][c2]) {
                            errors++;
                            if (errors > errorCount) {
                                break;
                            }
                        }
                    }
                    if (errors > errorCount) {
                        break;
                    }
                }
                if (errors == errorCount) {
                    return col;
                }
            }
            return 0;
        }

        private int FindHorizontal(int errorCount) {
            for (var row = 1; row < lines.Length; row++) {
                int errors = 0;
                for (var col = 0; col < lines[0].Length; col++) {
                    for (var dist = 0; dist < lines.Length; dist++) {
                        int r1 = row + dist;
                        int r2 = row - 1 - dist;
                        if (r1 >= lines.Length || r2 < 0) {
                            break;
                        }
                        if (lines[r1][col] != lines[r2][col]) {
                            errors++;
                            if (errors > errorCount) {
                                break;
                            }
                        }
                    }
                    if (errors > errorCount) {
                        break;
                    }
                }
                if (errors == errorCount) {
                    return row;
                }
            }
            return 0;
        }
    }

public override void Run() {
        var patterns = Data!.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries).Select(chunk => new Pattern(chunk)).ToArray();

        var result1 = patterns.Select(p => p.GetReflectionValue(0)).Sum();

        Console.WriteLine(result1);
        // 37381

        var result2 = patterns.Select(p => p.GetReflectionValue(1)).Sum();

        Console.WriteLine(result2);
        // 28210
    }
}
