namespace AOC23;

internal class Day11 : AocDay {
    private readonly List<(long row, long col)> Galaxies = new();

    public override void Run() {
        var lines = Data!.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToArray();
        FindGalaxies(lines);

        long[] emptyRows = Enumerable.Range(0, lines.Length)
            .Where(row => Galaxies.Where(rc => row == rc.row).Count() == 0)
            .Select(row => (long)row)
            .ToArray();
        long[] emptyCols = Enumerable.Range(0, lines[0].Length)
            .Where(col => Galaxies.Where(rc => col == rc.col).Count() == 0)
            .Select(col => (long)col)
            .ToArray();

        Dictionary<int, int> inRows = new();
        for (int row = 0; row < lines.Length; row++) {
            inRows[row] = Galaxies.Where(rc => row == rc.row).Count();
        }
        Dictionary<int, int> inCols = new();
        for (int col = 0; col < lines[0].Length; col++) {
            inCols[col] = Galaxies.Where(rc => col == rc.col).Count();
        }

        (long, long)[] expanded = Galaxies
            .Select(gal => (gal.row + emptyRows.Where(row => row < gal.row).Count(),
                            gal.col + emptyCols.Where(col => col < gal.col).Count()))
            .ToArray();

        var result1 = GetSumOfDistances(expanded);

        Console.WriteLine(result1);
        // 9550717

        (long, long)[] expandedMore = Galaxies
            .Select(gal => (gal.row + emptyRows.Where(row => row < gal.row).Count() * (1000000 - 1),
                            gal.col + emptyCols.Where(col => col < gal.col).Count() * (1000000 - 1)))
            .ToArray();

        var result2 = GetSumOfDistances(expandedMore);

        Console.WriteLine(result2);
        // 648458253817
    }

    private long GetSumOfDistances((long, long)[] galaxies) {
        long sum = 0;
        for (int i = 0; i < galaxies.Length; i++) {
            for (int j = 0; j < galaxies.Length; j++) {
                if (i >= j) {
                    continue;
                }
                sum += Math.Abs(galaxies[i].Item1 - galaxies[j].Item1) + Math.Abs(galaxies[i].Item2 - galaxies[j].Item2);
            }
        }
        return sum;
    }

    private void FindGalaxies(string[] lines) {
        for (int row = 0; row < lines.Length; row++) {
            for (int col = 0; col < lines[0].Length; col++) {
                if (lines[row][col] == '#') {
                    Galaxies.Add((row, col));
                }
            }
        }
    }
}
