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
    List<int[]> locks = new();
    List<int[]> keys = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int fit = 0;
        foreach (int[] _lock in locks)
        {
            foreach (int[] key in keys)
            {
                fit += TestOverlap(_lock, key);
            }
        }

        Console.WriteLine("Part1: {0}", fit);
    }

    public void Part2()
    {

        long rslt = 0;

        Console.WriteLine("Part2: {0}", rslt);
    }

    private int TestOverlap(int[] _lock, int[] key)
    {
        bool fit = true;
        for (int i = 0; fit && i < 5; i++)
        {
            fit = _lock[i] + key[i] <= 5;
        }
        return fit ? 1 : 0;
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
                int[] block = new int[5];
                bool isLock = line[0] == '#';
                line = file.ReadLine();
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        block[x] += line[x] == '#' ? 1 : 0;
                    }
                    line = file.ReadLine();
                }
                if (isLock)
                    locks.Add(block);
                else
                    keys.Add(block);
                line = file.ReadLine();
            }

            file.Close();
        }
    }

}
