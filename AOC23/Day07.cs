namespace AOC23;

internal class Day07 : AocDay {

    internal class Hand {
        public int[] Cards { get; }
        public int[] CardsJ { get; }
        public int HandType { get; set; }
        public int HandTypeJ { get; set; }
        private static Dictionary<char, int> CodeToInt = new() { { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 },
            { '8', 8 }, { '9', 9 }, { 'T', 10 }, { 'J', 11 }, { 'Q', 12 }, { 'K', 13 }, { 'A', 14 } };
        private static Dictionary<char, int> CodeToIntJ = new() { { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 },
            { '8', 8 }, { '9', 9 }, { 'T', 10 }, { 'J', 1 }, { 'Q', 12 }, { 'K', 13 }, { 'A', 14 } };
        public Hand(string s) {
            Cards = s.Select(c => CodeToInt[c]).ToArray();
            CardsJ = s.Select(c => CodeToIntJ[c]).ToArray();
            HandType = GetHandType(Cards);
            HandTypeJ = GetHandTypeJ();
        }

        private int GetHandTypeJ() {
            return Enumerable.Range(2, 13)
                .Select(val => GetHandType(CardsJ.Select(c => c == 1 ? val : c).ToArray()))
                .Max();
        }

        private int GetHandType(int[] cards) {
            var groups = new HashSet<int>(cards).Count;
            switch (groups) {
                case 1:
                    return 7;
                case 2:
                    var firstCount = cards.Where(c => c == cards[0]).Count();
                    return (firstCount == 1 || firstCount == 4) ? 6 : 5;
                case 3:
                    var anyThree = cards.Any(cc => cards.Where(c => c == cc).Count() == 3);
                    return (anyThree) ? 4 : 3;
                case 4:
                    return 2;
                case 5:
                    return 1;
                default:
                    throw new Exception("Impossible hand type");
            }
        }
        internal long GetValue() => (1 << 24) * HandType + Cards[4] << 4 | Cards[3] << 8 | Cards[2] << 12 | Cards[1] << 16 | Cards[0] << 20;

        internal long GetValueJ() => (1 << 24) * HandTypeJ + CardsJ[4] << 4 | CardsJ[3] << 8 | CardsJ[2] << 12 | CardsJ[1] << 16 | CardsJ[0] << 20;
    }

    public override void Run() {
        var lines = Data!.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        var hands = lines.Select(line => {
            var items = line.Split(' ', StringSplitOptions.TrimEntries);
            return (new Hand(items[0]), int.Parse(items[1]));
        }).ToList();

        var sorted = hands.OrderBy(hb => hb.Item1.GetValue()).ToList();
        Console.WriteLine(EvaluateTotal(sorted));
        // 251029473

        var sortedJ = hands.OrderBy(hb => hb.Item1.GetValueJ()).ToList();
        Console.WriteLine(EvaluateTotal(sortedJ));
        // 251003917
    }

    public int EvaluateTotal(List<(Hand, int)> hands) => hands.Select((hb, index) => (index + 1) * hb.Item2).Sum();
}
