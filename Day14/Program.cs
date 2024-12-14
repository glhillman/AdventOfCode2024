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

public class Robot
{
    public Robot( int id, int x, int y, int vx, int vy)
    {
        ID = id;
        X = x;
        Y = y;
        VX = vx;
        VY = vy;
    }
    public int ID { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int VX { get; private set; }
    public int VY { get; private set; }
    public void Move(int seconds, int xMax, int yMax)
    {
        X = ((seconds * VX) + X) % xMax;
        if (X < 0)
            X += xMax;
        Y = ((seconds * VY) + Y) % yMax;
        if (Y < 0)
            Y += yMax;
    }
    public void Step(int xMax, int yMax)
    {
        X = (VX + X) % xMax;
        if (X < 0)
            X += xMax;
        Y = (VY + Y) % yMax;
        if (Y < 0)
            Y += yMax;
    }

    public override string ToString()
    {
        return string.Format("ID:{0}, X:{1} Y:{2} VX:{3}, VY:{4}", ID, X, Y, VX, VY);
    }
}

internal class DayClass
{
    List<Robot> robots = new();
    const int XMax = 101;
    const int YMax = 103;
    Dictionary<(int, int), int> image = new();
    public DayClass()
    {
        LoadData();
    }
    public void Part1()
    {
        foreach (Robot r in robots)
        {
            r.Move(100, XMax, YMax);
        }
        (long tl, long tr, long bl, long br) quads = CountQuads(robots);

        long rslt = quads.tl * quads.tr * quads.bl * quads.br;

        Console.WriteLine("Part1: {0}", rslt);
    }

    public void Part2()
    {
        LoadData();
        // init the dictionary
        for (int y = 0; y < YMax; y++)
            for (int x = 0; x < XMax; x++)
                image[(x, y)] = 0;
        // represent the robots
        foreach (Robot r in robots)
        {
            image[(r.X, r.Y)]++;
        }

        bool treeFound = false;
        long steps = 0;
        while (!treeFound)
        {
            foreach (Robot r in robots)
            {
                image[(r.X, r.Y)]--;
                r.Step(XMax, YMax);
                image[(r.X, r.Y)]++;
            }
            steps++;
            treeFound = TestForTree();
        }

        Console.WriteLine("Part2: {0}", steps);
    }

    private (long tl, long tr, long bl, long br) CountQuads(List<Robot> robots)
    {
        long tl = 0;
        long tr = 0;
        long bl = 0;
        long br = 0;

        long halfMaxX = XMax / 2;
        long halfMaxY = YMax / 2;    

        foreach (Robot r in robots)
        {
            if (r.X < halfMaxX)
            {
                if (r.Y < halfMaxY)
                    tl++;
                else if (r.Y > halfMaxY)
                    bl++;
            }
            else if (r.X > halfMaxX)
            {
                if (r.Y < halfMaxY)
                    tr++;
                else if (r.Y > halfMaxY)
                    br++;
            }
        }

        return (tl, tr, bl, br);
    }

    private bool TestForTree()
    {
        bool treeFound = false;

        for (int x = 20; !treeFound && x < XMax - 40; x++)
        {
            for (int offset = 35; !treeFound && offset < YMax - 35; offset+=10)
            {
                // from left to right, scan vertically 10 chars at a time to see if they form a vertical line
                treeFound = true;
                for (int y = 0; treeFound && y < 10; y++)
                {
                    treeFound = image[(x, y + offset)] > 0;
                }
                //visual confirmation that the tree is found
                //if (treeFound)
                //{
                //    DumpImage();
                //    Console.Write("OK? y/n: ");
                //    string check = Console.ReadLine();
                //    treeFound = check == "y";
                //}
            }
        }

        return treeFound;
    }

    private void DumpImage()
    {
        for (int y = 0; y < YMax; y++)
        {
            for (int x = 0; x < XMax; x++)
            {
                int c = image[(x, y)];
                Console.Write(c == 0 ? '.' : (char)(c + '0'));
            }
            Console.WriteLine();
        }
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";
        robots.Clear();
        if (File.Exists(inputFile))
        {
            int ID = 0;
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split(' ', '=', ',');
                robots.Add(new Robot(ID++, int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[4]), int.Parse(parts[5])));
            }

            file.Close();
        }
    }
}

