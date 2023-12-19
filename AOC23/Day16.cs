namespace AOC23;

internal class Day16 : AocDay {
    string[] Device;
    enum Dir {
        up = 0,
        right,
        down,
        left
    }
    int[] Dir2dr = { -1, 0, 1, 0 };
    int[] Dir2dc = { 0, 1, 0, -1 };

    public override void Run() {
        Device = Data!.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToArray();

        var result1 = Energize(0, 0, Dir.right);
        Console.WriteLine(result1);
        // 7562

        var result2 = GetMaxEnergized();
        Console.WriteLine(result2);
        // 7793
    }

    private int GetMaxEnergized() {
        List<int> e = new();
        for (var i = 0; i < Device.Length; i++) {
            e.Add(Energize(i, 0, Dir.right));
            e.Add(Energize(i, Device[0].Length-1, Dir.left));
        }
        for (var i = 0; i < Device[0].Length; i++) {
            e.Add(Energize(0, i, Dir.down));
            e.Add(Energize(Device.Length-1, i, Dir.up));
        }
        return e.Max();
    }

    private int Energize(int row, int col, Dir dddir) {
        HashSet<(int, int, Dir)> Energized = new();
        Stack<(int r, int c, Dir d)> beams = new();
        beams.Push((row, col, dddir));
        while (beams.Count > 0) {
            var (r, c, d) = beams.Pop();
            if (r < 0 || r >= Device.Length || c < 0 || c >= Device[0].Length) {
                continue;
            }
            if (Energized.Contains((r, c, d))) {
                continue;
            }
            Energized.Add((r, c, d));
            switch (Device[r][c]) {
                case '.':
                    beams.Push(BeamContinue(r, c, d));
                    break;
                case '/':
                    switch (d) {
                        case Dir.up:
                            beams.Push((r, c + 1, Dir.right));
                            break;
                        case Dir.right:
                            beams.Push((r - 1, c, Dir.up));
                            break;
                        case Dir.down:
                            beams.Push((r, c - 1, Dir.left));
                            break;
                        case Dir.left:
                            beams.Push((r + 1, c, Dir.down));
                            break;
                    }
                    break;
                case '\\':
                    switch (d) {
                        case Dir.up:
                            beams.Push((r, c - 1, Dir.left));
                            break;
                        case Dir.right:
                            beams.Push((r + 1, c, Dir.down));
                            break;
                        case Dir.down:
                            beams.Push((r, c + 1, Dir.right));
                            break;
                        case Dir.left:
                            beams.Push((r - 1, c, Dir.up));
                            break;
                    }
                    break;
                case '-':
                    switch (d) {
                        case Dir.up:
                            beams.Push((r, c - 1, Dir.left));
                            beams.Push((r, c + 1, Dir.right));
                            break;
                        case Dir.right:
                            beams.Push((r, c + 1, Dir.right));
                            break;
                        case Dir.down:
                            beams.Push((r, c - 1, Dir.left));
                            beams.Push((r, c + 1, Dir.right));
                            break;
                        case Dir.left:
                            beams.Push((r, c - 1, Dir.left));
                            break;
                    }
                    break;
                case '|':
                    switch (d) {
                        case Dir.up:
                            beams.Push((r - 1, c, Dir.up));
                            break;
                        case Dir.right:
                            beams.Push((r - 1, c, Dir.up));
                            beams.Push((r + 1, c, Dir.down));
                            break;
                        case Dir.down:
                            beams.Push((r + 1, c, Dir.down));
                            break;
                        case Dir.left:
                            beams.Push((r - 1, c, Dir.up));
                            beams.Push((r + 1, c, Dir.down));
                            break;
                    }
                    break;
            }
        }
        return Energized.Select(rcd => (rcd.Item1, rcd.Item2)).Distinct().Count();
    }

    private (int r, int c, Dir d) BeamContinue(int r, int c, Dir d) {
        return (r + Dir2dr[(int)d], c + Dir2dc[(int)d], d);
    }
}
