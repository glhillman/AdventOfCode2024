// See https://aka.ms/new-console-template for more information
using System;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

public class Secret
{
    public Secret(int num)
    {
        Num = num;
        Price = (int)num % 10;
        Delta = 0;
        Next = this;
        Prev = this;
    }
    public int Num { get; set; }
    public int Delta { get; set; } // change from previous num
    public int Price { get; set; }
    public Secret Next { get; set; }
    public Secret Prev { get; set; }

    public override string ToString()
    {
        return string.Format("Num: {0}, Delta: {1}, Price: {2}, Next Num: {3}, Prev Num: {4}", Num, Delta, Price, Next.Num, Prev.Num);
    }
}

public class SecretCircle
{
    public SecretCircle(Secret secret)
    {
        Current = secret;
        for (int i = 0; i < 3; i++)
        {
            AddSecret(new Secret(NextSecret(Current.Num)));
        }
        Secret sec = Current.Next;
        do
        {
            sec.Price = sec.Num % 10;
            sec.Delta = sec.Price - sec.Prev.Price;
            sec = sec.Next;
        } while (sec != Current.Next);
        Current.Next.Delta = 0;
    }

    public Secret Current { get; set; }

    public static int NextSecret(int num)
    {
        num = ((num << 6) ^ num) & 16777215;
        num = ((num >> 5) ^ num) & 16777215;
        num = ((num << 11) ^ num) & 16777215;

        return num;
    }

    public (long key, int price) TakeNextStep()
    {
        int newSecretNum = NextSecret(Current.Num);
        Current = Current.Next;
        Current.Num = newSecretNum;
        Current.Price = newSecretNum % 10;
        Secret curr = Current;
        curr.Delta = curr.Price - curr.Prev.Price;
        curr = Current.Next;
        long key = 0;
        for (int i = 0; i < 4; i++)
        {
            long value = curr.Delta;
            if (value < 0)
            {
                value = -1 * (value * 1000000000);
            }
            key *= 10;
            key += value;
            curr = curr.Next;
        }
        return (key, Current.Price);
    }
    public void AddSecret(Secret secret)
    {
        secret.Prev = Current;
        secret.Next = Current.Next;
        Current.Next = secret;
        secret.Next.Prev = secret;
        Current = secret;
    }
}

internal class DayClass
{
    List<int> _nums = new();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        long sum = 0;
        foreach (int num in _nums)
        {
            int n = num;
            for (int i = 0; i < 2000; i++)
            {
                n = SecretCircle.NextSecret(n);
            }
            sum += n;
        }

        Console.WriteLine("Part1: {0}", sum);
    }

    public void Part2()
    {
        Dictionary<long, int> master = new();
        Dictionary<long, int> dict = new();
        foreach (int num in _nums)
        {
            Secret sec = new Secret(num);
            SecretCircle circ = new SecretCircle(sec);
            for (int i = 0; i < 2000 - 3; i++)
            {
                (long key, int price) pair = circ.TakeNextStep();
                if (dict.ContainsKey(pair.key) == false)
                {
                    dict[pair.key] = pair.price;
                }
            }
            foreach (long key in dict.Keys)
            {
                if (master.ContainsKey(key))
                {
                    master[key] += dict[key];
                }
                else
                {
                    master[key] = dict[key];
                }
            }
            dict.Clear();
        }
        int max = master.Values.Max();
        Console.WriteLine("Part2: {0}", max);
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
                _nums.Add(int.Parse(line));
            }

            file.Close();
        }
    }

}
