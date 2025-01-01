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

internal class PathPoint
{
    public PathPoint(char type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
        Dist = 0;
        Visited = false;
    }
    public char Type { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Dist { get; set; }
    public bool Visited { get; set; }
    public (int x, int y) Next { get; set; }
    public override string ToString()
    {
        return string.Format("{0} X,Y:{1},{2}, Dist:{3} Next:{4}", Type, X, Y, Dist, Next);
    }
}
internal class DayClass
{
    PathPoint[,] _maze = new PathPoint[1, 1];
    int _mazeSize = 0;
    (int x, int y) _start;
    (int x, int y) _end;

    public DayClass()
    {
        LoadData();
        (int x, int y)[] deltas = { (-1, 0), (1, 0), (0, -1), (0, 1) }; 
        // traverse the map filling in accumulated distances
        int distance = 0;
        (int x, int y) curr = _start;
        while (curr != _end)
        {
            foreach ((int x, int y) delta in deltas)
            {
                (int x, int y) nextPoint = (curr.x + delta.x, curr.y + delta.y);
                if (_maze[nextPoint.x, nextPoint.y].Visited == false && _maze[nextPoint.x, nextPoint.y].Type != '#')
                {
                    _maze[curr.x, curr.y].Visited = true;
                    _maze[curr.x, curr.y].Next = nextPoint;
                    curr = nextPoint;
                    _maze[curr.x, curr.y].Dist = ++distance;
                    break;
                }
            }
        }
    }

    public void Part1()
    {
        Dictionary<int, int> cheats = new();

        (int x, int y) curr = _start;
        (int x, int y)[] tests = { (-2, 0), (2, 0), (0, -2), (0, 2) };

        while (curr != _end)
        {
            foreach ((int x, int y) test in tests)
            { 
                if (IsValidXY(test, curr))
                {
                    int cheat = _maze[curr.x + test.x, curr.y + test.y].Dist - _maze[curr.x, curr.y].Dist - 2;
                    if (cheat >= 100)
                    {
                        if (cheats.ContainsKey(cheat) == false)
                            cheats[cheat] = 1;
                        else
                            cheats[cheat]++;
                    }
                }
            }
            curr = _maze[curr.x, curr.y].Next;
        }

        Console.WriteLine("Part1: {0}", cheats.Sum(x => x.Value));
    }

    public void Part2()
    {
        Dictionary<int, int> cheats = new();
        int maxStep = 20;

        (int x, int y) curr = _start;
        (int x, int y) next = curr;
        while (curr != _end)
        {
            int currentDistance = _maze[curr.x, curr.y].Dist;
            bool allChecked = false;
            while (!allChecked)
            {
                int manHattanDist = Math.Abs(next.x - curr.x) + Math.Abs(next.y - curr.y);
                if (manHattanDist <= maxStep)
                {
                    int cheat = (_maze[next.x, next.y].Dist - currentDistance) - manHattanDist;
                    if (cheat >= 100)
                    {
                        if (cheats.ContainsKey(cheat) == false)
                            cheats[cheat] = 1;
                        else
                            cheats[cheat]++;
                    }
                }
                if (next == _end)
                    allChecked = true;
                else
                    next = _maze[next.x, next.y].Next;
            }
            curr = _maze[curr.x, curr.y].Next;
            next = curr;
        }

        Console.WriteLine("Part1: {0}", cheats.Sum(x => x.Value));
    }

    private bool IsValidXY((int x, int y) test, (int x, int y) curr)
    {
        int newX = curr.x + test.x;
        int newY = curr.y + test.y;

        return (newX >= 0 && newX < _mazeSize && newY >= 0 && newY < _mazeSize && _maze[newX, newY].Type != '#');
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
                if (_mazeSize == 0)
                {
                    _mazeSize = line.Length;
                    _maze = new PathPoint[_mazeSize, _mazeSize];
                }
                for (int x = 0; x < _mazeSize; x++)
                {
                    char c = line[x];
                    _maze[x, y] = new PathPoint(c, x, y);
                    if (c == 'S')
                        _start = (x, y);
                    else if (c == 'E')
                        _end = (x, y);
                }
                y++;
            }

            file.Close();
        }
    }

}
