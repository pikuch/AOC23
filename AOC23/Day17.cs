namespace AOC23;

internal class Day17 : AocDay {
    int[,] Map;
    int MaxR;
    int MaxC;
    public override void Run() {
        var lines = Data!.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToArray();
        Map = new int[lines.Length, lines[0].Length];
        MaxR = lines.Length - 1;
        MaxC = lines[0].Length - 1;

        for (int r = 0; r < lines.Length; r++) {
            for (int c = 0; c < lines[0].Length; c++) {
                Map[r, c] = int.Parse(lines[r][c].ToString());
            }
        }

        var result1 = GetMinCost(1, 3);
        Console.WriteLine(result1);
        // 907

        var result2 = GetMinCost(4, 10);
        Console.WriteLine(result2);
        // 1057
    }

    private int GetMinCost(int minDist, int maxDist) {
        int[] dir2dr = { -1, 0, 1, 0 };
        int[] dir2dc = { 0, 1, 0, -1 };
        var Results = new Dictionary<(int r, int c, int dir), int>();
        var ToCheck = new PriorityQueue<(int r, int c, int dir, int cost), int>();
        ToCheck.Enqueue((0, 0, 1, 0), EstimateFrom(0, 0));
        ToCheck.Enqueue((0, 0, 2, 0), EstimateFrom(0, 0));

        while (ToCheck.Count > 0) {
            var (r, c, dir, cost) = ToCheck.Dequeue();
            for (int steps = 1; steps <= maxDist; steps++) {
                r += dir2dr[dir];
                c += dir2dc[dir];
                if (r < 0 || r > MaxR || c < 0 || c > MaxC) {
                    break;
                }
                cost += Map[r, c];
                if (steps >= minDist) {
                    if (Results.ContainsKey((r, c, dir)) && Results[(r, c, dir)] <= cost) {
                        continue;
                    }
                    else {
                        Results[(r, c, dir)] = cost;
                    }
                    ToCheck.Enqueue((r, c, (dir + 3) % 4, cost), EstimateFrom(r, c));
                    ToCheck.Enqueue((r, c, (dir + 1) % 4, cost), EstimateFrom(r, c));
                }

            }
        }
        return Results.Keys.Where(k => k.r == MaxR && k.c == MaxC).Select(f => Results[f]).Min();
    }

    private int EstimateFrom(int rr, int cc) {
        return MaxR - rr + MaxC - cc;
    }
}
