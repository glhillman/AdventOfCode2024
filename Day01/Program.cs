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

internal class DayClass
{
    List<int> list1 = new List<int>();
    List<int> list2 = new List<int>();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        list1.Sort();
        list2.Sort();
        int diffSum = 0;

        for (int i = 0; i < list1.Count; i++)
        {
            diffSum += Math.Abs((list1[i] - list2[i]));
        }

        Console.WriteLine("Part1: {0}", diffSum);
    }

    public void Part2()
    {
        long rslt = 0;

        Dictionary<int, int> dict = new Dictionary<int, int>();
        foreach (int i in list2)
        {
            if (dict.ContainsKey(i))
            {
                dict[i]++;
            }
            else
            {
                dict[i] = 1;
            }
        }
        foreach (int i in list1)
        {
            if (dict.ContainsKey(i))
            {
                rslt += dict[i] * i;
            }
        }


        Console.WriteLine("Part2: {0}", rslt);
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
                string[] parts = line.Split(' ');
                list1.Add(int.Parse(parts[0]));
                list2.Add(int.Parse(parts[3]));
            }

            file.Close();
        }
    }

}
