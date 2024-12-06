// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class DayClass
{
    List<string> inStrings = new List<string>();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {

        long sum = 0;
        Regex reg = new Regex(@"mul\(\d+,\d+\)");
        foreach (string inString in inStrings)
        {
            var matches = reg.Matches(inString);
            foreach (Match s in matches)
            {
                string[] parts = s.ToString().Split('(', ',', ')');
                sum += long.Parse(parts[1]) * long.Parse(parts[2]);
            }
        }
        Console.WriteLine("Part1: {0}", sum);
    }

    public void Part2()
    {
        long sum = 0;
        Regex reg = new Regex(@"mul\(\d+,\d+\)|do\(\)|don't\(\)");
        bool doMult = true;
        foreach (string inString in inStrings)
        {
            var matches = reg.Matches(inString);

            foreach (Match s in matches)
            {
                switch (s.ToString())
                {
                    case "do()":
                        doMult = true;
                        break;
                    case "don't()":
                        doMult = false;
                        break;
                    default:
                        if (doMult)
                        {
                            string[] parts = s.ToString().Split('(', ',', ')');
                            sum += long.Parse(parts[1]) * long.Parse(parts[2]);

                        }
                        break;
                }
            }
        }

        Console.WriteLine("Part2: {0}", sum);
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                inStrings.Add(line.Trim());
            }

            file.Close();
        }
    }

}
