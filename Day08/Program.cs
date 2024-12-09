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

internal class Pos
{
    public static int MaxValue = 0;
    public Pos(int row, int col)
    {
        Row = row;
        Col = col;
    }
    public int Row { get; set; }
    public int Col { get; set; }
    public Pos? Subtract(Pos other)
    {
        Pos? newPos = new Pos(this.Row - other.Row, this.Col - other.Col);
        return newPos;
    }
    public Pos? Add(Pos other)
    {
        Pos? newPos = new Pos(this.Row + other.Row, this.Col + other.Col);
        if (newPos.Row < 0 || newPos.Row >= MaxValue || newPos.Col < 0 || newPos.Col >= MaxValue)
        {
            newPos = null;
        }
        return newPos;
    }
    public override string ToString()
    {
        return string.Format("R/C:{0}/{1}", Row, Col);
    }
}
internal class DayClass
{
    public Dictionary<char, List<Pos>> map = new();
    public HashSet<(int,int)> antiNodes = new();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        List<char> ants = map.Keys.ToList();
        foreach (char ant in ants)
        {
            PlaceNodes(ant, false);  
        }

        Console.WriteLine("Part1: {0}", antiNodes.Count);
    }

    public void Part2()
    {
        antiNodes.Clear();
        List<char> ants = map.Keys.ToList();
        foreach (char ant in ants)
        {
            PlaceNodes(ant, true);
        }

        Console.WriteLine("Part2: {0}", antiNodes.Count);
    }

    public void PlaceNodes(char ant, bool extend)
    {
        List<Pos> allPos = map[ant];

        for (int i = 0; i < allPos.Count; i++)
        {
            for (int j = i + 1; j < allPos.Count; j++)
            {
                if (i != j)
                {
                    // get the deltas
                    Pos? newPos = allPos[i];
                    if (extend)
                    {
                        antiNodes.Add((newPos.Row, newPos.Col));
                    }
                    do
                    {
                        newPos = newPos.Subtract(allPos[j]);
                        newPos = allPos[i].Add(newPos);
                        if (newPos != null)
                        {
                            antiNodes.Add((newPos.Row, newPos.Col));
                        }
                    } while (extend && newPos != null);

                    newPos = allPos[j];
                    if (extend)
                    {
                        antiNodes.Add((newPos.Row, newPos.Col));
                    }
                    do
                    {
                        newPos = newPos.Subtract(allPos[i]);
                        newPos = allPos[j].Add(newPos);
                        if (newPos != null)
                        {
                            antiNodes.Add((newPos.Row, newPos.Col));
                        }
                    } while (extend && newPos != null);
                }
            }
        }
    }

    public void DumpMap()
    {
        int size = Pos.MaxValue;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (antiNodes.Contains((row, col)))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";
        int row = 0;

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                for (int col = 0; col < line.Length; col++)
                {
                    if (line[col] != '.')
                    {
                        char c = line[col];
                        if (map.ContainsKey(c) == false)
                        {
                            map[c] = new List<Pos>();
                        }
                        map[c].Add(new Pos(row, col));
                    }
                }
                row++;
            }
            file.Close();
        }
        Pos.MaxValue = row;
    }

}

