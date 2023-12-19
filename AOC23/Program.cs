using System.Reflection;

namespace AOC23;

internal class Program {
    static void Main(string[] args) {
        var days = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(AocDay)))
            .ToList();
        days.Sort((d1, d2) => d1.Name.CompareTo(d2.Name));

        //Console.WriteLine($"Days: {string.Join(',', days.Select(t => t.Name))}");
        var chosenDay = 0;
        var choice = days.Find(d => d.Name == $"Day{chosenDay:00}");
        AocDay? day;

        if (choice is null) {
            day = (AocDay?)Activator.CreateInstance(days.Last());
        }
        else {
            day = (AocDay?)Activator.CreateInstance(choice!);
        }

        if (day == null) {
            Console.WriteLine("No days to run!");
        }
        else {
            Console.WriteLine($"Running Day{day.Day:00}:");
            day.Run();
        }
    }
}
