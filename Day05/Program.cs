// See https://aka.ms/new-console-template for more information
using System.Diagnostics.CodeAnalysis;

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
    Dictionary<int,List<int>> before = new Dictionary<int,List<int>>();
    Dictionary<int,List<int>> after = new Dictionary<int,List<int>>();
    List<List<int>> updates = new List<List<int>>();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int sum = 0;
        int updateIndex = 0;
        while (updateIndex < updates.Count)
        {
            List<int> update = updates[updateIndex];
            bool updateOK = true;
            for (int i = 0; i < update.Count; i++)
            {
                int page = update[i];
                // check all following
                for (int j = i + 1; updateOK && j < update.Count; j++)
                {
                    updateOK = before.ContainsKey(page) && before[page].Contains(update[j]);
                }
                for (int j = i - 1; updateOK && j >= 0; j--)
                {
                    updateOK = after.ContainsKey(page) && after[page].Contains(update[j]);
                }
            }
            if (updateOK)
            {
                int midIndex = update.Count / 2;
                sum += update[midIndex];
                updates.RemoveAt(updateIndex);
            }
            else
            {
                updateIndex++;
            }
        }

        Console.WriteLine("Part1: {0}", sum);
    }

    public void Part2()
    {
        int sum = 0;
        List<int> ints = new List<int>();

        foreach (List<int> update in updates)
        {
            int newInt;
            ints.Clear();
            ints.Add(update[0]);
            for (int i = 0; i < update.Count-1; i++)
            {
                int lAnchor = -1;
                newInt = update[i+1];
                // test coming in from the left
                for (int j = 0; j < ints.Count; j++)
                {
                    int examInt = ints[j];
                    if (before.ContainsKey(examInt) && before[examInt].Contains(newInt))
                    {
                        lAnchor = j + 1;
                    }
                }
                if (lAnchor > 0)
                {
                    if (lAnchor >= ints.Count)
                    {
                        ints.Add(newInt);
                    }
                    else
                    {
                        ints.Insert(lAnchor, newInt);
                    }
                }
                else
                {
                    ints.Insert(0, newInt);
                }

            }
            int midIndex = ints.Count / 2;
            sum += ints[midIndex];
        }
        Console.WriteLine("Part2: {0}", sum);
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            int key = 0;
            int value = 0;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null && line.Length > 0)
            {
                // store the forward-looking 
                string[] parts = line.Split('|');
                key = int.Parse(parts[0]);
                value = int.Parse(parts[1]);
                if (before.ContainsKey(key) == false)
                {
                    before[key] = new List<int>(); 
                }
                before[key].Add(value);

                if (after.ContainsKey(value) == false)
                {
                    after[value] = new List<int>();
                }
                after[value].Add(key);
            }

            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                List<int> pages = new List<int>();
                foreach (string part in parts)
                {
                    pages.Add(int.Parse(part));
                }
                updates.Add(pages);
            }

            file.Close();
        }
    }

}
