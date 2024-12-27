// See https://aka.ms/new-console-template for more information
DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();
public class Towel
{
    public Towel(string all)
    {
        All = all;
        StartsWith = all[0];
        Length = all.Length;
    }
    public string All { get; set; }
    public char StartsWith { get; set; }
    public int Length { get; set; }
    public override string ToString()
    {
        return string.Format(All);
    }
}
internal class DayClass
{
    List<Towel> _towels = new();
    List<string> _patterns = new();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        long[] cache = new long[_patterns.Max(p => p.Length) + 1];
        long foundCount = 0;
        long makeable = 0;

        foreach (string pattern in _patterns)
        {
            // clear the cache
            for (int i = 1; i < cache.Length; i++)
            {
                cache[i] = 0;
            }
            cache[0] = 1;
            foundCount = MatchAllPatterns(pattern, 0, cache);
            makeable += foundCount > 0 ? 1 : 0;
        }

        Console.WriteLine("Part1: {0}", makeable);
    }

    public void Part2()
    {
        long[] cache = new long[_patterns.Max(p => p.Length) + 1];
        long foundCount = 0;
        long makeable = 0;

        foreach (string pattern in _patterns)
        {
            // clear the cache
            for (int i = 1; i < cache.Length; i++)
            {
                cache[i] = 0;
            }
            cache[0] = 1;
            foundCount = MatchAllPatterns(pattern, 0, cache);
            makeable += foundCount;
        }

        Console.WriteLine("Part2: {0}", makeable);
    }

    private long MatchAllPatterns(string pattern, int index, long[] cache)
    {
        if (index == pattern.Length)
        {
            return cache[pattern.Length];
        }
        else
        {
            foreach (Towel towel in _towels.Where(t => t.StartsWith == pattern[index]))
            {
                if (index + towel.Length <= pattern.Length && pattern.Substring(index, towel.Length).Equals(towel.All))
                {
                    cache[index + towel.Length] += cache[index];
                }
            }
            return MatchAllPatterns(pattern, index + 1, cache);
        }
    }
    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            line = file.ReadLine();
            string[] parts = line.Split(", ");
            foreach (string part in parts)
            {
                _towels.Add(new Towel(part));
            }
            line = file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                _patterns.Add(line);
            }

            file.Close();
        }
    }

}
