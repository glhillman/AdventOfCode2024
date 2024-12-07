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

internal class Pos : IEquatable<Pos>
{
    public Pos(int x, int y)
    {
        X = x;
        Y = y;
        // initial state up
        DX = 0;
        DY = -1;
        state = 0;
    }

    public Pos(Pos curPos)
    {
        X = curPos.X;
        Y = curPos.Y;
        DX = curPos.DX;
        DY = curPos.DY;
        state = curPos.state;
    }

    public override bool Equals(Object? obj)
    {
        var other = obj as Pos;
        if (other == null) return false;

        return Equals(other);
    }

    public bool Equals(Pos? other)
    {
        if (other == null) 
            return false;

        return X == other.X && Y == other.Y && DX == other.DX && DY == other.DY && state == other.state;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int DX { get; set; }
    public int DY { get; set; }
    protected int state = 0;

    public void Turn()
    {
        switch (state)
        {
            case 0: // up to right
                DX = 1;
                DY = 0;
                break;
            case 1: // right to down
                DX = 0;
                DY = 1;
                break;
            case 2: // down to left
                DX = -1;
                DY = 0;
                break;
            case 3: // left to up
                DX = 0;
                DY = -1;
                break;
        }
        state++;
        state %= 4;
    }

    public (int x, int y) NextPos()
    {
        return (X+DX,  Y+DY);
    }

    public void Step()
    {
        X += DX;
        Y += DY;
    }

    public override string ToString()
    {
        return string.Format("X:{0}, Y:{1}, State: {2}", X, Y, state);
    }
}

internal class DayClass
{
    List<char[]> gMap = new List<char[]>();
    int gRow = 0;
    int gCol = 0;
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int nVisited = TestMap(false);
        Console.WriteLine("Part1: {0}", nVisited);
    }

    public void Part2()
    {
        int nLoops = 0;

        for (int x = 0; x < gMap.Count; x++)
        {
            for (int y = 0; y < gMap.Count; y++)
            {
                if (gMap[y][x] == '.')
                {
                    gMap[y][x] = '#';
                    nLoops += TestMap(true);
                    gMap[y][x] = '.';
                }
            }
        }

        Console.WriteLine("Part2: {0}", nLoops);
    }

    public int TestMap(bool loopDetect)
    {
        bool[,] visited = new bool[gMap.Count, gMap.Count];
        Pos[,]? trail = null;

        bool loopDetected = false;
        Pos pos = new Pos(gCol, gRow);
        visited[gCol, gRow] = true;
        

        if (loopDetect)
        {
            trail = new Pos[gMap.Count,gMap.Count];
            trail[gRow, gCol] = new Pos(pos);
        }


        while (!loopDetected && pos.X >= 0 && pos.X < gMap[0].Length && pos.Y >= 0 && pos.Y < gMap.Count)
        {
            (int x, int y) peek = pos.NextPos();
            if (peek.x >= 0 && peek.x < gMap.Count && peek.y >= 0 && peek.y < gMap.Count)
            {
                if (gMap[peek.y][peek.x] == '#')
                {
                    pos.Turn();
                }
                else
                {
                    pos.Step();
                    if (loopDetect)
                    {
                        if (trail[pos.Y, pos.X] != null && trail[pos.Y, pos.X].Equals(pos))
                        {
                            loopDetected = true;
                        }
                        trail[pos.Y, pos.X] = new Pos(pos);
                    }
                    visited[pos.Y, pos.X] = true;
                }
            }
            else
            {
                pos.Step();
            }
        }
        if (loopDetect)
        {
            return loopDetected ? 1 : 0;
        }
        else
        {
            int nVisited = 0;
            for (int x = 0; x < gMap.Count; x++)
            {
                for (int y = 0; y < gMap.Count; y++)
                {
                    nVisited += visited[y, x] ? 1 : 0;
                }
            }
            return nVisited;
        }
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            int row = 0;
            while ((line = file.ReadLine()) != null)
            {
                if (gRow == 0)
                {
                    int gPos = line.IndexOf('^');
                    if (gPos >= 0)
                    {
                        gRow = row;
                        gCol = gPos;
                    }
                }
                gMap.Add(line.ToArray());
                row++;
            }

            file.Close();
        }
    }
}
