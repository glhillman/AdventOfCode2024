// See https://aka.ms/new-console-template for more information
using Day16;

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
    public char[,] _map = new char[1, 1];
    public (int x, int y) _start;
    public (int x, int y) _end;
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int turnCost = 1000;

        List<DijkstraWeighted.Node> paths = DijkstraWeighted.Dijkstra(_map, _start, _end, turnCost, false);

        Console.WriteLine("Part1: {0}", paths[0].Cost);
    }

    public void Part2()
    {
        int turnCost = 1000;

        List<DijkstraWeighted.Node> paths = DijkstraWeighted.Dijkstra(_map, _start, _end, turnCost, true);
        HashSet<(int x, int y)> seats = new();
        foreach(DijkstraWeighted.Node node in paths)
        {
            DijkstraWeighted.Node? n = node;
            while (n != null)
            {
                seats.Add((n.X, n.Y));
                n = n.Prev;
            }
        }

        Console.WriteLine("Part2: {0}", seats.Count);
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            int y = 0;
            while ((line = file.ReadLine()) != null)
            {
                if (_map.Length == 1)
                {
                    _map = new char[line.Length, line.Length];
                }
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == 'S')
                        _start = ((x, y));
                    else if (line[x] == 'E')
                        _end = ((x, y));
                    _map[x,y] = line[x];
                }
                y++;
            }
            file.Close();
        }
    }

}
