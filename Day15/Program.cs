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
    char[,] _map = new char[1, 1];
    char[,] _map2 = new char[1, 1];
    List<char> _steps = new();
    int _robotX = 0;
    int _robotY = 0;
    int _saveX = 0;
    int _saveY = 0;
    Dictionary<char, char> _boxVSides = new();


    public DayClass()
    {
        LoadData();
        //DumpMap('1', '*', _map);
        _map2 = new char[_map.GetLength(0) * 2, _map.GetLength(1)];
        int x2 = 0;
        //int y2 = 0;
        for (int y = 0; y < _map.GetLength(0); y++)
        {
            x2 = 0;
            for (int x = 0;  x < _map.GetLength(1); x++)
            {
                switch (_map[x,y])
                {
                    case '#':
                        _map2[x2++, y] = '#';
                        _map2[x2++, y] = '#';
                        break;
                    case 'O':
                        _map2[x2++, y] = '[';
                        _map2[x2++, y] = ']';
                        break;
                    case '.':
                        _map2[x2++, y] = '.';
                        _map2[x2++, y] = '.';
                        break;
                    case '@':
                        _map2[x2, y] = '@';
                        _saveX = x2++;
                        _saveY = y;
                        _map2[x2++, y] = '.';
                        break;
                    default:
                        break;
                }
            }
        }
        //DumpMap('2', '*', _map2);
    }

    public void Part1()
    {
        foreach (char step in _steps)
        {
            MoveRobot(step, ref _robotX, ref _robotY);
            //DumpMap(step);
            //Console.ReadLine();
        }

        long sum = 0;
        for (int y = 0; y < _map.GetLength(0); y++)
        {
            for (int x = 0; x < _map.GetLength(1); x++)
            {
                if (_map[x,y] == 'O')
                {
                    sum += 100 * y + x;
                }
            }
        }
        Console.WriteLine("Part1: {0}", sum);
    }

    public void Part2()
    {
        _boxVSides['['] = ']';
        _boxVSides[']'] = '[';
        for (int step = 0; step < _steps.Count; step++)
        {
            MoveRobot2(_steps[step], ref _saveX, ref _saveY, _map2);
            char nextStep = step < _steps.Count - 1 ? _steps[step + 1] : '*';
            //DumpMap(_steps[step], nextStep, _map2);
            //Console.ReadLine();
        }

        long sum = 0;
        for (int y = 0; y < _map2.GetLength(1); y++)
        {
            for (int x = 0; x < _map2.GetLength(0); x++)
            {
                if (_map2[x, y] == '[')
                {
                    sum += 100 * y + x;
                }
            }
        }
        Console.WriteLine("Part2: {0}", sum);
    }

    private void MoveRobot(char direction, ref int x, ref int y)
    {
        int nextX = x;
        int nextY = y;
        int dx;
        int dy;
        switch (direction)
        {
            case '<': dx = -1; dy = 0; break;
            case '>': dx = 1; dy = 0; break;
            case '^': dx = 0; dy = -1; break;
            default: dx = 0; dy = 1; break; // down
        }


        nextX = x + dx;
        nextY = y + dy;
        switch (_map[nextX, nextY])
        {
            case '#': break;
            case '.':
                {
                    _map[x, y] = '.';
                    x = nextX;
                    y = nextY;
                    _map[x, y] = '@';
                }
                break;
            default: // 'O'
                {
                    while (_map[nextX, nextY] == 'O')
                    {
                        nextX += dx;
                        nextY += dy;
                    }
                    if (_map[nextX, nextY] == '.')
                    {
                        _map[x, y] = '.';
                        _map[nextX, nextY] = 'O';
                        x += dx;
                        y += dy;
                        _map[x, y] = '@';
                    }
                }
                break;
        }
    }

    private void MoveRobot2(char direction, ref int x, ref int y, char[,] map)
    {
        int nextX = x;
        int nextY = y;
        int dx;
        int dy;
        switch (direction)
        {
            case '<': dx = -1; dy = 0; break;
            case '>': dx = 1; dy = 0; break;
            case '^': dx = 0; dy = -1; break;
            default: dx = 0; dy = 1; break; // down
        }

        nextX = x + dx;
        nextY = y + dy;
        switch (map[nextX, nextY])
        {
            case '#': break;
            case '.':
                {
                    map[x, y] = '.';
                    x = nextX;
                    y = nextY;
                    map[x, y] = '@';
                }
                break;
            case '[':
            case ']':
                if (direction == '<' || direction == '>')
                {
                    // left & right is easy, always groups of two - [ is moving right & ] is moving left
                    int boxCount = 0;
                    while (_boxVSides.ContainsKey(map[nextX, nextY]) && map[nextX + dx, nextY] == _boxVSides[map[nextX, nextY]])
                    {
                        nextX += dx * 2;
                        boxCount++;
                    }
                    if (map[nextX, nextY] == '.')
                    {
                        while (boxCount > 0)
                        {
                            map[nextX, nextY] = map[nextX - dx, nextY];
                            nextX -= dx;
                            map[nextX, nextY] = map[nextX - dx, nextY];
                            nextX -= dx;
                            boxCount--;
                        }
                        map[x + dx,y] = '@';
                        map[x, nextY] = '.';
                        x = nextX;
                        y = nextY;
                    }
                }
                else
                {
                    // direction == ^ or v
                    bool okToMove = MoveVertical(dy, x, y+dy, map, false);
                    if (okToMove)
                    {
                        MoveVertical(dy, x, y+dy, map, true);
                        map[x, y + dy] = '@';
                        map[x, y] = '.';
                        y = y + dy;
                    }
                }
                break;
            default: 
                {
                }
                break;
        }
    }

    private bool MoveVertical(int dy, int x, int y, char[,] map, bool doMove)
    {
        // always pointing to a box or space or wall above

        if (x < 1 || x > map.GetLength(0) - 2 || y < 1 || y > map.GetLength(1) - 2)
            return false;

        if (map[x, y] == '.' && map[x + 1, y] == '.')
            // we have space above we can move
            return true;

        bool robotBelow = map[x, y - dy] == '@';

        bool above = map[x, y] == '[';
        bool aboveLeft = map[x, y] == ']';
        bool aboveRight = map[x + 1, y] == '[' && !robotBelow;
        bool okToMove = true;

        if (robotBelow && aboveLeft)
        {
            x--;
            aboveLeft = false;
            above = true;
        }

        if (map[x, y] == '#' || map[x + 1, y] == '#')
            // there is a segment of wall blocking us - can't move
            return false;


        if (above)
        {
            okToMove = PushColumn(dy, x, y, map, doMove);
        }
        else
        {
            if (aboveLeft)
            {
                okToMove = PushColumn(dy, x - 1, y, map, doMove);
            }
            if (okToMove && aboveRight)
            {
                okToMove = PushColumn(dy, x + 1, y, map, doMove);
            }
        }

        return okToMove;
    }

    private bool PushColumn(int dy, int x, int y, char[,] map, bool doMove)
    {
        bool okToMove = MoveVertical(dy, x, y + dy, map, doMove);
        if (okToMove && doMove)
        {
            map[x, y + dy] = map[x, y];
            map[x + 1, y + dy] = map[x + 1, y];
            map[x, y] = '.';
            map[x + 1, y] = '.';
        }
        return okToMove;
    }


    int _moves = 0;
    private void DumpMap(char direction, char nextDirection, char[,] map)
    {
        Console.WriteLine("{0} Move {1}:", _moves++, direction);
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                Console.Write(map[x, y]);
            }
            Console.WriteLine();
        }
        Console.WriteLine("Next direction {0}:", nextDirection);
        Console.WriteLine();
    
    
    
    
    }
    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            line = file.ReadLine();
            int size = line.Length;
            _map = new char[size, size];
            int y = 0;
            while (line.Length > 0)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    _map[x, y] = line[x];
                    if (line[x] == '@')
                    {
                        _robotX = x;
                        _robotY = y;
                    }
                }
                y++;
                line = file.ReadLine();
            }

            while ((line = file.ReadLine()) != null)
            {
                foreach (char c in line)
                {
                    _steps.Add(c);
                }
            }

            file.Close();
        }
    }

}
