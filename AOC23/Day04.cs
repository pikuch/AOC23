namespace AOC23;

internal class Day04 : AocDay {

    internal class Card {
        public List<int> Winning { get; set; }
        public List<int> Actual { get; set; }
        public Card(string line) {
            var nums = line.Split(':', StringSplitOptions.TrimEntries)[1];
            var parts = nums.Split('|', StringSplitOptions.TrimEntries);
            Winning = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();
            Actual = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();
        }
        public int GetHits() {
            return Actual.Where(n => Winning.Contains(n)).Count();
        }
        public int GetValue() {
            var hits = GetHits();
            return hits == 0 ? 0 : 1 << (hits - 1);
        }
    }

    public Card[] Cards { get; set; }
    public int[] Counts { get; set; }

    public override void Run() {
        Cards = Data!.Split("\r\n").Select(line => new Card(line)).ToArray();
        Counts = Cards.Select(c => 1).ToArray();
       
        Console.WriteLine(Cards.Select(c => c.GetValue()).Sum());
        // 23235

        GetCounts();

        Console.WriteLine(Counts.Sum());
        // 5920640
    }

    private void GetCounts() {
        for (int i = 0; i < Cards.Length; i++) {
            var hits = Cards[i].GetHits();
            for (var j = i + 1; j <= Math.Min(i + hits, Cards.Length); j++) {
                Counts[j] += Counts[i];
            }
        }
    }
}
