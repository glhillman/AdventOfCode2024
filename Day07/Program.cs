// See https://aka.ms/new-console-template for more information
using System.Windows.Markup;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1And2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class Problem
{
    public Problem(long result)
    {
        Result = result;
        Values = new List<long>();
    }

    public long Result { get; set; }
    public List<long> Values { get; set; }
}
internal class DayClass
{
    List<Problem> problems = new();
    public DayClass()
    {
        LoadData();
    }

    public void Part1And2()
    {
        long sum = 0;

        int i = 0;
        while (i < problems.Count)
        {
            long value = Curse(problems[i], problems[i].Values[0], 0, false);
            if (value > 0)
            {
                sum += value;
                problems.RemoveAt(i); // pull out the ones we don't need to check for part2
            }
            else
            {
                i++;
            }
        }
        Console.WriteLine("Part1: {0}", sum);

        foreach (Problem problem in problems)
        {
            long value = Curse(problem, problem.Values[0], 0, true);
            sum += value > 0 ? value : 0;
        }

        Console.WriteLine("Part2: {0}", sum);
    }

    public void Part2()
    {

        long rslt = 0;

        Console.WriteLine("Part2: {0}", rslt);
    }

    public long Curse(Problem problem, long soFar, int index, bool useCat)
    {
        long rslt = -1;

        if (soFar > problem.Result)
        {
            return rslt;
        }

        if (soFar == problem.Result && index == problem.Values.Count - 1)
        {
            return soFar;
        }
        else
        {
            if (index < problem.Values.Count - 1)
            {
                if (useCat)
                {
                    rslt = Curse(problem, CatNum(soFar, problem.Values[index + 1]), index + 1, useCat);
                }
                if (rslt <= 0)
                {
                    rslt = Curse(problem, soFar + problem.Values[index + 1], index + 1, useCat);
                    if (rslt <= 0)
                    {
                        rslt = Curse(problem, soFar * problem.Values[index + 1], index + 1, useCat);
                    }
                }
            }
        }

        return rslt; 
    }

    private long CatNum(long num1, long num2)
    {
        long temp2 = num2;
        while (temp2 != 0)
        {
            temp2 /= 10;
            num1 *= 10;
        }
        return num1 + num2;
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
                string[] parts = line.Split(' ', ':');
                Problem p = new Problem(long.Parse(parts[0]));
                for (int i = 2; i < parts.Length; i++)
                {
                    p.Values.Add(long.Parse(parts[i]));
                }
                problems.Add(p);
            }

            file.Close();
        }
    }

}
