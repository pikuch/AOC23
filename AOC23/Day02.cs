namespace AOC23;

internal class Day02 : AocDay {
    internal class Grab {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public Grab(string line) {
            var items = line.Split(',', StringSplitOptions.TrimEntries);
            foreach (var item in items) {
                var parts = item.Split(' ', StringSplitOptions.TrimEntries);
                switch (parts[1]) {
                    case "red":
                        R = int.Parse(parts[0]);
                        break;
                    case "green":
                        G = int.Parse(parts[0]);
                        break;
                    case "blue":
                        B = int.Parse(parts[0]);
                        break;
                    default:
                        throw new Exception($"bad data {parts[1]}");
                }
            }
        }
    }

    internal class Game {
        public int Id { get; set; }
        List<Grab> Grabs { get; set; }
        public Game(string line) {
            var parts = line.Split(':', StringSplitOptions.TrimEntries);
            Id = int.Parse(parts[0].Split(' ', StringSplitOptions.TrimEntries)[1]);
            Grabs = parts[1].Split(';', StringSplitOptions.TrimEntries).Select(g => new Grab(g)).ToList();
        }
        public bool IsGamePossible(int r, int g, int b) {
            return Grabs.All(grab => grab.R <= r && grab.G <= g && grab.B <= b);
        }
        public int CalculatePower() {
            return Grabs.Select(grab => grab.R).Max() *
                   Grabs.Select(grab => grab.G).Max() *
                   Grabs.Select(grab => grab.B).Max();
        }
    }

    public override void Run() {
        var games = Data!.Split("\r\n").Select(line => new Game(line)).ToArray();

        Console.WriteLine(Solve1(games));
        // 2331

        Console.WriteLine(Solve2(games));
        // 71585
    }

    private int Solve1(Game[] games) {
        return games.Where(g => g.IsGamePossible(12, 13, 14)).Select(g => g.Id).Sum();
    }

    private int Solve2(Game[] games) {
        return games.Select(g => g.CalculatePower()).Sum();
    }
}
