using System.Text;

namespace AOC23;

internal class Day14 : AocDay {
    internal class Rocks {
        char[,] Field;
        public Rocks(string[] lines) {
            Field = new char[lines.Length, lines[0].Length];
            for (int r = 0; r < lines.Length; r++) {
                for (int c = 0; c < lines[0].Length; c++) {
                    Field[r, c] = lines[r][c];
                }
            }
        }

        internal void Tilt(int dir) {
            int moves;
            if (dir == 0) {
                do {
                    moves = 0;
                    for (int r = 0; r < Field.GetLength(0); r++) {
                        for (int c = 0; c < Field.GetLength(1); c++) {
                            if (Field[r, c] == 'O') {
                                if (r - 1 >= 0 && Field[r - 1, c] == '.') {
                                    Field[r - 1, c] = 'O';
                                    Field[r, c] = '.';
                                    moves++;
                                }
                            }

                        }
                    }

                }
                while (moves > 0);
            }
            else if (dir == 1) {
                do {
                    moves = 0;
                    for (int r = 0; r < Field.GetLength(0); r++) {
                        for (int c = 0; c < Field.GetLength(1); c++) {
                            if (Field[r, c] == 'O') {
                                if (c - 1 >= 0 && Field[r, c - 1] == '.') {
                                    Field[r, c - 1] = 'O';
                                    Field[r, c] = '.';
                                    moves++;
                                }
                            }

                        }
                    }

                }
                while (moves > 0);
            }
            else if (dir == 2) {
                do {
                    moves = 0;
                    for (int r = 0; r < Field.GetLength(0); r++) {
                        for (int c = 0; c < Field.GetLength(1); c++) {
                            if (Field[r, c] == 'O') {
                                if (r + 1 < Field.GetLength(0) && Field[r + 1, c] == '.') {
                                    Field[r + 1, c] = 'O';
                                    Field[r, c] = '.';
                                    moves++;
                                }
                            }

                        }
                    }

                }
                while (moves > 0);
            }
            if (dir == 3) {
                do {
                    moves = 0;
                    for (int r = 0; r < Field.GetLength(0); r++) {
                        for (int c = 0; c < Field.GetLength(1); c++) {
                            if (Field[r, c] == 'O') {
                                if (c + 1 < Field.GetLength(1) && Field[r, c + 1] == '.') {
                                    Field[r, c + 1] = 'O';
                                    Field[r, c] = '.';
                                    moves++;
                                }
                            }

                        }
                    }

                }
                while (moves > 0);
            }
            
        }

        internal long GetTotalLoad() {
            long load = 0;
            for (int r = 0; r < Field.GetLength(0); r++) {
                for (int c = 0; c < Field.GetLength(1); c++) {
                    if (Field[r, c] == 'O') {
                        load += Field.GetLength(0) - r;
                    }
                }
            }
            return load;
        }

        internal void Show() {
            for (int r = 0; r < Field.GetLength(0); r++) {
                for (int c = 0; c < Field.GetLength(1); c++) {
                    Console.Write(Field[r, c]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        internal void Spin() {
            for (int i = 0; i < 4; i++) {
                Tilt(i);
            }
        }

        internal string GetData() {
            StringBuilder sb = new(Field.Length);
            for (int r = 0; r < Field.GetLength(0); r++) {
                for (int c = 0; c < Field.GetLength(1); c++) {
                    sb.Append(Field[r, c]);
                }
            }
            return sb.ToString();
        }
    }

public override void Run() {
        var lines = Data!.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToArray();
        var rocks = new Rocks(lines);

        rocks.Tilt(0);
        var result1 = rocks.GetTotalLoad();

        Console.WriteLine(result1);
        // 109654

        Dictionary<string, int> seen = new();
        string s;
        for (int i = 1; i <= 1000000000; i++) {
            rocks.Spin();
            s = rocks.GetData(); 
            if (seen.ContainsKey(s)) {
                int index1 = seen[s];
                int diff = i - index1;
                i += ((1000000000 - i) / diff) * diff;
                seen.Clear();
            }
            else {
                seen[s] = i;
            }
        }

        var result2 = rocks.GetTotalLoad();

        Console.WriteLine(result2);
        // 94876
    }
}
