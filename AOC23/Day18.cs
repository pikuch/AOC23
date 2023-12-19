namespace AOC23;

internal class Day18 : AocDay {
    List<(int dir, long dist)> BasicPoints = new();
    List<(int dir, long dist)> Points = new();

    public override void Run() {
        var lines = Data!.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToArray();
        GetBasicPoints(lines);
        GetPoints(lines);

        long result1 = CalculateArea(BasicPoints);
        Console.WriteLine(result1);
        // 46359

        var result2 = CalculateArea(Points);
        Console.WriteLine(result2);
        // 59574883048274
    }

    private long CalculateArea(List<(int dir, long dist)> points) {
        long area = 0;
        while (points.Count > 4) {
            long diff;
            do {
                diff = ReduceArea(points);
                SimplifyPoints(points);
                diff += ReduceNarrowArea(points);
                SimplifyPoints(points);
                area += diff;
            } while (diff > 0);
        }
        return area + 1;
    }

    private void SimplifyPoints(List<(int dir, long dist)> points) {
        bool changes;
        do {
            changes = false;
            for (int i = 0; i < points.Count; i++) {
                int i0 = i;
                int i1 = (i + 1) % points.Count;
                if (points[i0].dir == points[i1].dir) {
                    points[i0] = (points[i0].dir, points[i0].dist + points[i1].dist);
                    points.RemoveAt(i1);
                    changes = true;
                    break;
                }
            }
        } while (changes);

        for (int i = points.Count - 1; i >= 0; i--) {
            if (points[i].dist == 0) {
                points.RemoveAt(i);
            }
        }
    }

    private long ReduceArea(List<(int dir, long dist)> points) {
        long minReduction = long.MaxValue;
        int minIndex = -1;
        for (int j = 0; j < points.Count; j++) {
            int j0 = j;
            int j1 = (j + 1) % points.Count;
            int j2 = (j + 2) % points.Count;
            if (points[j1].dir == (points[j0].dir + 1) % 4 && points[j2].dir == (points[j0].dir + 2) % 4) {
                if (points[j1].dist < minReduction) {
                    minReduction = points[j1].dist;
                    minIndex = j;
                }
            }
        }

        if (minIndex == -1) {
            return 0;
        }
        int index = minIndex;

        int i0 = index;
        int i1 = (index + 1) % points.Count;
        int i2 = (index + 2) % points.Count;
        if (points[i1].dir == (points[i0].dir + 1) % 4 && points[i2].dir == (points[i0].dir + 2) % 4) {
            long reduction = Math.Min(points[i0].dist, points[i2].dist);
            points[i0] = (points[i0].dir, points[i0].dist - reduction);
            points[i2] = (points[i2].dir, points[i2].dist - reduction);
            return reduction * (points[i1].dist + 1);
        }
        
        return 0;
    }
    private long ReduceNarrowArea(List<(int dir, long dist)> points) {
        for (int i = 0; i < points.Count; i++) {
            int i0 = i;
            int i1 = (i + 1) % points.Count;
            if (points[i1].dir == (points[i0].dir + 2) % 4) {
                long reduction = Math.Min(points[i0].dist, points[i1].dist);
                points[i0] = (points[i0].dir, points[i0].dist - reduction);
                points[i1] = (points[i1].dir, points[i1].dist - reduction);
                return reduction;
            }
        }
        return 0;
    }

    private void GetPoints(string[] lines) {
        Dictionary<string, int> n2dir = new() { { "3", 0 }, { "0", 1 }, { "1", 2 }, { "2", 3 } };
        foreach (var line in lines) {
            var items = line.Split(' ', StringSplitOptions.TrimEntries).ToArray();
            var dist = Convert.ToInt32(items[2][2..7], 16);
            var dir = n2dir[items[2][7..8]];            
            Points.Add((dir, dist));
        }
    }

    private void GetBasicPoints(string[] lines) {
        Dictionary<string, int> s2dir = new() { { "U", 0 }, { "R", 1 }, { "D", 2 }, { "L", 3 } };
        foreach (var line in lines) {
            var items = line.Split(' ', StringSplitOptions.TrimEntries).ToArray();
            BasicPoints.Add((s2dir[items[0]], int.Parse(items[1])));
        }
    }
}
