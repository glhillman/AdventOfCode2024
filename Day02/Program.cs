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
    List<List<int>> list = new List<List<int>>();
    int part1SafeCount = 0;

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int safeCount = 0;
        int index = 0;
        while (index < list.Count)
        { 
            bool isSafe = IsSafe(list[index]);
            if (isSafe)
            {
                safeCount++;
                list.RemoveAt(index);
            }
            else
            {
                index++;
            }
        }
        part1SafeCount = safeCount;
        Console.WriteLine("Part1: {0}", safeCount);
    }

    public void Part2()
    {
        int safeCount = 0;


        foreach (List<int> subList in list)
        {
            for (int i = 0; i < subList.Count; i++)
            {
                List<int> copy = new List<int>(subList);
                copy.RemoveAt(i);
                if (IsSafe(copy))
                {
                    safeCount++;
                    break;
                }
            }
        }

        Console.WriteLine("Part2: {0}", safeCount + part1SafeCount);
    }

    public bool IsSafe(List<int> subList)
    {
        bool isSafePlus = true;
        bool isSafeMinus = true;

        for (int i = 0; isSafeMinus && i < subList.Count-1; i++)
        {
            int delta = (subList[i] - subList[i + 1]);
            isSafeMinus = delta >= 1 && delta <= 3;
        }
        if (!isSafeMinus)
        {
            for (int i = 0; isSafePlus && i < subList.Count - 1; i++)
            {
                int delta = (subList[i+1] - subList[i]);
                isSafePlus = delta >= 1 && delta <= 3;
            }
        }
        return isSafePlus || isSafeMinus;
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
                List<int> subList = new List<int>();
                string[] parts = line.Split(' ');
                foreach (string part in parts)
                {
                    subList.Add(int.Parse(part));
                }
                list.Add(subList);
            }

            file.Close();
        }
    }

}
