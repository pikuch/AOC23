namespace AOC23;

internal class Day03 : AocDay {

    internal class Number {
        public int Value { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int Len { get; set; }
        public Number(int row, int col, List<char> chars) {
            Row = row;
            Col = col;
            Len = chars.Count;
            Value = int.Parse(string.Join("", chars));
        }
        internal bool IsNear(int row, int col) {
            return row >= Row - 1 && row <= Row + 1 && col >= Col - 1 && col <= Col + Len;
        }
    }

    internal class Item {
        public char C { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public Item(int row, int col, char c) {
            C = c;
            Row = row;
            Col = col;
        }
    }

    private string[] Lines { get; set; }
    private List<Number> Numbers { get; set; } = new List<Number> ();
    private List<Item> Items { get; set; } = new List<Item>();
    public override void Run() {
        Lines = Data!.Split("\r\n").ToArray();
        ParseLines();

        Console.WriteLine(GetSumOfPartNumbers());
        // 532445

        Console.WriteLine(GetSumOfGearRatios());
        // 79842967
    }

    private int GetSumOfGearRatios() {
        return Items
            .Where(item => item.C == '*')
            .Select(item => Numbers.Where(n => n.IsNear(item.Row, item.Col)).ToList())
            .Where(nums => nums.Count == 2)
            .Select(nums => nums[0].Value * nums[1].Value)
            .Sum();
    }

    private int GetSumOfPartNumbers() {
        return Numbers
            .Where(n => Items.Any(item => n.IsNear(item.Row, item.Col)))
            .Select(n => n.Value)
            .Sum();
    }

    private void ParseLines() {
        for (int row = 0; row < Lines.Length; row++) {
            var col = 0;
            var digits = new List<char>();
            while (col < Lines[0].Length) {
                if ("0123456789".Contains(Lines[row][col])) {
                    digits.Add(Lines[row][col]);
                }
                else {
                    if (digits.Count > 0) {
                        Numbers.Add(new Number(row, col-digits.Count, digits));
                        digits.Clear();
                    }
                    if (Lines[row][col] != '.') {
                        Items.Add(new Item(row, col, Lines[row][col]));
                    }
                }
                col++;
            }
            if (digits.Count > 0) {
                Numbers.Add(new Number(row, col - digits.Count, digits));
                digits.Clear();
            }
        }
    }
}
