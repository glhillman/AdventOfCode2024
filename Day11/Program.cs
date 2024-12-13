// See https://aka.ms/new-console-template for more information
using System.ComponentModel.Design;
using System.Diagnostics;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1And2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class DayClass
{
    List<long> nums = new();
    public class TargetStruct
    {
        public TargetStruct(long value1, long value2 = -1)
        {
            Value1 = value1;
            Value2 = value2;
            InPlay = 0;
            BlinkCount = 0;
        }
        public long Value1;
        public long Value2;
        public long InPlay { get; set; }
        public long BlinkCount { get; set; }
        public override string ToString()
        {
            return string.Format("{0}, {1} | {2}, {3}", Value1, Value2 >= 0 ? Value2 : "", InPlay, BlinkCount);
        }
    }

    public Dictionary<long, TargetStruct> map = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1And2()
    {
        LoadMap();

        int nLoops = 75;
        foreach (long i in nums)
        {
            map[i].InPlay++;
        }

        for (int i = 0; i < nLoops; i++)
        {
            Blink();
            if (i == 24)
            {
                Console.WriteLine("Part1: {0}", CountStones());
            }
        }

        Console.WriteLine("Part2: {0}", CountStones());
    }

    private void Blink()
    {
        foreach (long key in map.Keys)
        {
            map[key].BlinkCount = map[key].InPlay;
        }
        
        foreach (long key in map.Keys)
        {
            if (map[key].BlinkCount > 0)
            {
                map[key].InPlay -= map[key].BlinkCount;
                map[map[key].Value1].InPlay += map[key].BlinkCount;
                if (map[key].Value2 >= 0)
                {
                    map[map[key].Value2].InPlay += map[key].BlinkCount;
                }
                map[key].BlinkCount = 0;
            }
        }
    }

    private long CountStones()
    {
        long stones = 0;
        foreach (long key in map.Keys)
        {
            stones += map[key].InPlay;
        }
        return stones;
    }

    private void LoadMap()
    {
        map[0] = new TargetStruct(1);

        for (int i = 0; i < nums.Count; i++)
        {
            ReCurse(nums[i]);
        }
    }

    private void ReCurse(long num)
    {
        if (map.ContainsKey(num))
        {
            return;
        }
        int len = (int)(Math.Floor(Math.Log10(num)) + 1);
        if (len % 2 == 0)
        {
            long left = 0;
            long right = 0;
            Split(num, len, ref left, ref right);
            map[num] = new TargetStruct(left, right);
            ReCurse(left);
            ReCurse(right);
        }
        else
        {
            long value = num * 2024;
            map[num] = new TargetStruct(value);
            ReCurse(value);
        }
    }
    private void Split(long num, int len, ref long left, ref long right)
    {
        int half = len / 2;
        int mag = (int)(Math.Pow(10, half));
        left = num / mag;
        right = num % mag;
    }
    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            line = file.ReadLine();
            string[] parts = line.Split(' ');
            foreach (string s in parts)
            {
                nums.Add(long.Parse(s));
            }

            file.Close();
        }
    }

}
