namespace AOC23;

internal class Day10 : AocDay {
    public enum Direction {
        Up,
        Right,
        Down,
        Left
    }

    internal class Pipe {
        public bool[] Connections = new bool[4];
        public Pipe(char c) {
            switch (c) {
                case '|':
                    SetConnection(Direction.Up);
                    SetConnection(Direction.Down);
                    break;
                case '-':
                    SetConnection(Direction.Left);
                    SetConnection(Direction.Right);
                    break;
                case 'F':
                    SetConnection(Direction.Right);
                    SetConnection(Direction.Down);
                    break;
                case '7':
                    SetConnection(Direction.Left);
                    SetConnection(Direction.Down);
                    break;
                case 'L':
                    SetConnection(Direction.Up);
                    SetConnection(Direction.Right);
                    break;
                case 'J':
                    SetConnection(Direction.Up);
                    SetConnection(Direction.Left);
                    break;
                default:
                    break;
            }
        }

        private void SetConnection(Direction dir) {
            Connections[(int)dir] = true;
        }

        public Direction GetOneDirection() {
            return (Direction)Connections
                .Select((connected, index) => connected ? index : -1)
                .Where(i => i != -1)
                .First();
        }

        public Direction GetNextDirection(Direction current) {
            return (Direction)Connections
                .Select((connected, index) => connected ? index : -1)
                .Where(i => i != -1)
                .Where(i => i != ((int)current+2)%4)
                .Single();
        }
    }

    internal class Maze {
        public Pipe[,] Pipes;
        int[,] Map;
        public (int row, int col) Start;
        public HashSet<(int, int)> Loop;
        private readonly List<(int, int)> Neighbours = new List<(int, int)>() { (-1, 0), (1, 0), (0, -1), (0, 1) };
        public Maze(string[] lines) {
            Pipes = new Pipe[lines.Length, lines[0].Length];
            for (int row=0; row<lines.Length; row++) {
                for (int col=0; col < lines[0].Length; col++) {
                    if (lines[row][col] != 'S') {
                        Pipes[row, col] = new Pipe(lines[row][col]);
                    }
                    else {
                        Start = (row, col);
                    }
                }
            }
            // place the right pipe at the start
            Pipes[Start.row, Start.col] = new Pipe('.');
            foreach (var (dr, dc) in Neighbours) {
                var rr = Start.row + dr;
                var cc = Start.col + dc;
                if (rr >= 0 && rr < Pipes.GetLength(0) && cc >= 0 && cc < Pipes.GetLength(1)) {
                    if (dr == -1 && Pipes[rr, cc].Connections[(int)Direction.Down]) {
                        Pipes[Start.row, Start.col].Connections[(int)Direction.Up] = true;
                    }
                    if (dr == 1 && Pipes[rr, cc].Connections[(int)Direction.Up]) {
                        Pipes[Start.row, Start.col].Connections[(int)Direction.Down] = true;
                    }
                    if (dc == -1 && Pipes[rr, cc].Connections[(int)Direction.Right]) {
                        Pipes[Start.row, Start.col].Connections[(int)Direction.Left] = true;
                    }
                    if (dc == 1 && Pipes[rr, cc].Connections[(int)Direction.Left]) {
                        Pipes[Start.row, Start.col].Connections[(int)Direction.Right] = true;
                    }
                }
            }

            Loop = new HashSet<(int, int)>();
            Map = new int[lines.Length * 3, lines[0].Length * 3];
        }

        public void MakeMap() {
            for (int row = 0; row < Pipes.GetLength(0); row++) {
                for (int col = 0; col < Pipes.GetLength(1); col++) {
                    if (Loop.Contains((row, col))) {
                        Map[row * 3 + 1, col * 3 + 1] = 1;
                        if (Pipes[row, col].Connections[0]) {
                            Map[row * 3 + 0, col * 3 + 1] = 1;
                        }
                        if (Pipes[row, col].Connections[1]) {
                            Map[row * 3 + 1, col * 3 + 2] = 1;
                        }
                        if (Pipes[row, col].Connections[2]) {
                            Map[row * 3 + 2, col * 3 + 1] = 1;
                        }
                        if (Pipes[row, col].Connections[3]) {
                            Map[row * 3 + 1, col * 3 + 0] = 1;
                        }
                    } else {
                        Map[row * 3 + 1, col * 3 + 1] = 2;
                    }
                }
            }
        }
            // display map
            //for (int r = 0; r < Map.GetLength(0); r++) {
            //    for (int c = 0; c < Map.GetLength(1); c++) {
            //        if (Map[r, c] == 0) {
            //            Console.Write(" ");
            //        }
            //        else if (Map[r, c] == 1) {
            //            Console.Write("█");
            //        }
            //        else {
            //            Console.Write("X");
            //        }
            //    }
            //    Console.WriteLine();
            //}

            
        public int CountInsideCells() {
            FloodFill();
            var cells = 0;
            foreach (var cell in Map) {
                if (cell == 2) {
                    cells++;
                }
            }
            return cells;
        }

        private void FloodFill() {
            HashSet<(int, int)> visited = new();
            Stack<(int, int)> toCheck = new();
            toCheck.Push((0, 0));
            visited.Add((0, 0));
            int rr;
            int cc;
            while (toCheck.Count>0) {
                var (r, c) = toCheck.Pop();
                if (Map[r, c] != 1) {
                    Map[r, c] = 1;
                    foreach (var (dr, dc) in Neighbours) {
                        rr = r + dr;
                        cc = c + dc;
                        if (!visited.Contains((rr, cc)) && rr >= 0 && rr < Map.GetLength(0) && cc >= 0 && cc < Map.GetLength(1)) {
                            visited.Add((rr, cc));
                            toCheck.Push((rr, cc));
                        }
                    }
                }
            }
        }

        public int MeasurePathLength() {
            var dir = Pipes[Start.row, Start.col].GetOneDirection();
            var dist = 0;
            var (row, col) = Start;
            do {
                switch (dir) {
                    case Direction.Up:
                        row -= 1;
                        dir = Pipes[row, col].GetNextDirection(dir);
                        break;
                    case Direction.Right:
                        col += 1;
                        dir = Pipes[row, col].GetNextDirection(dir);
                        break;
                    case Direction.Down:
                        row += 1;
                        dir = Pipes[row, col].GetNextDirection(dir);
                        break;
                    case Direction.Left:
                        col -= 1;
                        dir = Pipes[row, col].GetNextDirection(dir);
                        break;
                    default:
                        throw new Exception("Invalid direction!");
                }
                dist++;
                Loop.Add((row, col));
            } while ((row, col) != Start);
            return dist;
        }
    }

    public override void Run() {
        var lines = Data!.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToArray();
        var maze = new Maze(lines);

        Console.WriteLine(maze.MeasurePathLength()/2);
        // 6828

        maze.MakeMap();

        Console.WriteLine(maze.CountInsideCells());
        // 459
    }
}
