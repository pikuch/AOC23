using System.Numerics;

namespace AOC23;

internal class Day19 : AocDay {
    Dictionary<string, Workflow> Workflows;
    List<(int x, int m, int a, int s)> Parts;

    internal class Workflow {
        internal List<(char prop, char comp, int val, string dest)> Steps = new();
        public Workflow(string line) {
            var items = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items) {
                if (item.Contains(':')) {
                    var prop = item[0];
                    var comp = item[1];
                    var rest = item[2..].Split(':').ToArray();
                    var val = int.Parse(rest[0]);
                    var dest = rest[1];
                    Steps.Add((prop, comp, val, dest));
                }
                else {
                    Steps.Add((' ', ' ', 0, item));
                }
            }
        }

        internal string GetDestination((int x, int m, int a, int s) part) {
            foreach (var step in Steps) {
                if (step.prop == ' ') {
                    return step.dest;
                }
                switch (step.prop) {
                    case 'x':
                        if (step.comp == '<' && part.x < step.val) {
                            return step.dest;
                        }
                        if (step.comp == '>' && part.x > step.val) {
                            return step.dest;
                        }
                        break;
                    case 'm':
                        if (step.comp == '<' && part.m < step.val) {
                            return step.dest;
                        }
                        if (step.comp == '>' && part.m > step.val) {
                            return step.dest;
                        }
                        break;
                    case 'a':
                        if (step.comp == '<' && part.a < step.val) {
                            return step.dest;
                        }
                        if (step.comp == '>' && part.a > step.val) {
                            return step.dest;
                        }
                        break;
                    case 's':
                        if (step.comp == '<' && part.s < step.val) {
                            return step.dest;
                        }
                        if (step.comp == '>' && part.s > step.val) {
                            return step.dest;
                        }
                        break;
                    default:
                        throw new Exception("bad step");
                }
            }
            throw new Exception("out of steps");
        }
    }

    public override void Run() {
        var chunks = Data!.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries).ToArray();
        Workflows = chunks[0].Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(line => MakeWorkflow(line)).ToDictionary(kv => kv.name, kv => kv.wf);
        Parts = chunks[1].Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(line => MakePart(line)).ToList();

        long result1 = Parts.Where(part => IsAccepted(part)).Select(part => part.x + part.m + part.a + part.s).Sum();
        Console.WriteLine(result1);
        // 348378

        BigInteger result2 = CountAccepted();
        Console.WriteLine(result2);
        // 121158073425385
    }

    private BigInteger CountAccepted() {
        BigInteger accepted = 0;
        Stack<(int x0, int x1, int m0, int m1, int a0, int a1, int s0, int s1, string wf)> ToCheck = new();
        ToCheck.Push((1, 4000, 1, 4000, 1, 4000, 1, 4000, "in"));
        while (ToCheck.Count > 0) {
            var (x0, x1, m0, m1, a0, a1, s0, s1, wf) = ToCheck.Pop();
            bool stepsDone = false;
            foreach (var step in Workflows[wf].Steps) {
                if (stepsDone) {
                    break;
                }
                if (step.prop == ' ') {
                    if (step.dest == "A") {
                        accepted += new BigInteger(x1+1-x0) * new BigInteger(m1+1-m0) * new BigInteger(a1+1-a0) * new BigInteger(s1+1-s0);
                    }
                    else if (step.dest == "R") {
                        break;
                    }
                    else {
                        ToCheck.Push((x0, x1, m0, m1, a0, a1, s0, s1, step.dest));
                    }
                }
                else {
                    switch (step.prop) {
                        // X ===============================================================================
                        case 'x':
                            if (step.comp == '<') {
                                if (x1 < step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                        stepsDone = true;
                                        break;
                                    }
                                    else if (step.dest == "R") {
                                        stepsDone = true;
                                        break;
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, a0, a1, s0, s1, step.dest));
                                        stepsDone = true;
                                        break;
                                    }
                                }
                                else if (x0 < step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(step.val - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                    }
                                    else if (step.dest == "R") {
                                    }
                                    else {
                                        ToCheck.Push((x0, step.val-1, m0, m1, a0, a1, s0, s1, step.dest));
                                    }
                                    x0 = step.val;
                                }
                            }
                            else { // >
                                if (x0 > step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                        stepsDone = true;
                                        break;
                                    }
                                    else if (step.dest == "R") {
                                        stepsDone = true;
                                        break;
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, a0, a1, s0, s1, step.dest));
                                        stepsDone = true;
                                        break;
                                    }
                                }
                                else if (x1 > step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 - step.val) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                    }
                                    else if (step.dest == "R") {
                                    }
                                    else {
                                        ToCheck.Push((step.val + 1, x1, m0, m1, a0, a1, s0, s1, step.dest));
                                    }
                                    x1 = step.val;
                                }
                            }
                            break;
                        // M ===============================================================================
                        case 'm':
                            if (step.comp == '<') {
                                if (m1 < step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                        stepsDone = true;
                                        break;
                                    }
                                    else if (step.dest == "R") {
                                        stepsDone = true;
                                        break;
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, a0, a1, s0, s1, step.dest));
                                        stepsDone = true;
                                        break;
                                    }
                                }
                                else if (m0 < step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(step.val - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                    }
                                    else if (step.dest == "R") {
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, step.val - 1, a0, a1, s0, s1, step.dest));
                                    }
                                    m0 = step.val;
                                }
                            }
                            else { // >
                                if (m0 > step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                        stepsDone = true;
                                        break;
                                    }
                                    else if (step.dest == "R") {
                                        stepsDone = true;
                                        break;
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, a0, a1, s0, s1, step.dest));
                                        stepsDone = true;
                                        break;
                                    }
                                }
                                else if (m1 > step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 - step.val) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                    }
                                    else if (step.dest == "R") {
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, step.val + 1, m1, a0, a1, s0, s1, step.dest));
                                    }
                                    m1 = step.val;
                                }
                            }
                            break;
                        // A ===============================================================================
                        case 'a':
                            if (step.comp == '<') {
                                if (a1 < step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                        stepsDone = true;
                                        break;
                                    }
                                    else if (step.dest == "R") {
                                        stepsDone = true;
                                        break;
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, a0, a1, s0, s1, step.dest));
                                        stepsDone = true;
                                        break;
                                    }
                                }
                                else if (a0 < step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(step.val - a0) * new BigInteger(s1 + 1 - s0);
                                    }
                                    else if (step.dest == "R") {
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, a0, step.val - 1, s0, s1, step.dest));
                                    }
                                    a0 = step.val;
                                }
                            }
                            else { // >
                                if (a0 > step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                        stepsDone = true;
                                        break;
                                    }
                                    else if (step.dest == "R") {
                                        stepsDone = true;
                                        break;
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, a0, a1, s0, s1, step.dest));
                                        stepsDone = true;
                                        break;
                                    }
                                }
                                else if (a1 > step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 - step.val) * new BigInteger(s1 + 1 - s0);
                                    }
                                    else if (step.dest == "R") {
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, step.val + 1, a1, s0, s1, step.dest));
                                    }
                                    a1 = step.val;
                                }
                            }
                            break;
                        // S ===============================================================================
                        case 's':
                            if (step.comp == '<') {
                                if (s1 < step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                        stepsDone = true;
                                        break;
                                    }
                                    else if (step.dest == "R") {
                                        stepsDone = true;
                                        break;
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, a0, a1, s0, s1, step.dest));
                                        stepsDone = true;
                                        break;
                                    }
                                }
                                else if (s0 < step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(step.val - s0);
                                    }
                                    else if (step.dest == "R") {
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, a0, a1, s0, step.val - 1, step.dest));
                                    }
                                    s0 = step.val;
                                }
                            }
                            else { // >
                                if (s0 > step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 + 1 - s0);
                                        stepsDone = true;
                                        break;
                                    }
                                    else if (step.dest == "R") {
                                        stepsDone = true;
                                        break;
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, a0, a1, s0, s1, step.dest));
                                        stepsDone = true;
                                        break;
                                    }
                                }
                                else if (s1 > step.val) {
                                    if (step.dest == "A") {
                                        accepted += new BigInteger(x1 + 1 - x0) * new BigInteger(m1 + 1 - m0) * new BigInteger(a1 + 1 - a0) * new BigInteger(s1 - step.val);
                                    }
                                    else if (step.dest == "R") {
                                    }
                                    else {
                                        ToCheck.Push((x0, x1, m0, m1, a0, a1, step.val + 1, s1, step.dest));
                                    }
                                    s1 = step.val;
                                }
                            }
                            break;
                        default:
                            throw new Exception("impossible");
                    }
                }
            }
        }
        return accepted;
    }

    private bool IsAccepted((int x, int m, int a, int s) part) {
        string dest = "in";
        while (true) {
            dest = Workflows[dest].GetDestination(part);
            if (dest == "A") {
                return true;
            }
            if (dest == "R") {
                return false;
            }
        }
        return false;
    }

    private (int x, int m, int a, int s) MakePart(string line) {
        var items = line[1..(line.Length - 1)].Split(',');
        int x = 0, m = 0, a = 0, s = 0;
        foreach (var item in items) {
            var propval = item.Split('=').ToArray();
            switch (propval[0]) {
                case "x":
                    x = int.Parse(propval[1]);
                    break;
                case "m":
                    m = int.Parse(propval[1]);
                    break;
                case "a":
                    a = int.Parse(propval[1]);
                    break;
                case "s":
                    s = int.Parse(propval[1]);
                    break;
                default:
                    throw new Exception("bad property");
            }
        }
        return (x, m, a, s);
    }

    private (string name, Workflow wf) MakeWorkflow(string line) {
        var items = line[0..(line.Length-1)].Split('{');
        return (items[0], new Workflow(items[1]));
    }
}
