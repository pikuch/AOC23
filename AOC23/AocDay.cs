namespace AOC23;

internal abstract class AocDay {
    public byte Day { get; init; }
    protected string? Data { get; set; }
    public AocDay() {
        Day = Convert.ToByte(GetType().Name[3..5]);
        ReadData();
    }
    private void ReadData() {
        var filename = $"../../../../Day{Day:00}.txt";
        try {
            Data = File.ReadAllText(filename);
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }
    }
    public abstract void Run();
}
