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
    List<string> letters = new List<string>();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int len = letters[1].Length;
        int xCount = 0;

        for (int row = 3; row < letters.Count-3; row++)
        {
            for (int col = 3; col < len - 3; col++)
            {
                if (letters[row][col] == 'X')
                {
                    xCount += XmasAt(row, col, 1, 0); // check to the right
                    xCount += XmasAt(row, col, -1, 0); // check to the left
                    xCount += XmasAt(row, col, 0, 1); // check down
                    xCount += XmasAt(row, col, 0, -1); // check up
                    xCount += XmasAt(row, col, 1, 1); // right down
                    xCount += XmasAt(row, col, 1, -1); // right up
                    xCount += XmasAt(row, col, -1, 1); // left down
                    xCount += XmasAt(row, col, -1, -1); // left up
                }
            }
        }

        Console.WriteLine("Part1: {0}", xCount);
    }

    public void Part2()
    {
        int len = letters[1].Length;
        int xCount = 0;

        for (int row = 3; row < letters.Count - 3; row++)
        {
            for (int col = 3; col < len - 3; col++)
            {
                if (letters[row][col] == 'A')
                {
                    xCount += X_MasAt(row, col);
                }
            }
        }

        Console.WriteLine("Part2: {0}", xCount);
    }

    public int XmasAt(int row, int col, int deltaX, int deltaY)
    {
        bool isAt = true;
        // we are already on 'X' at row,col, so move to next position
        col += deltaX;
        row += deltaY;
        int state = 0; // three states 'M', 'A', 'S'
        while (state < 3 && isAt)
        {
            switch (state)
            {
                case 0: isAt = letters[row][col] == 'M'; break;
                case 1: isAt = letters[row][col] == 'A'; break;
                case 2: isAt = letters[row][col] == 'S'; break;
            }
            state++;
            col += deltaX;
            row += deltaY;

        }
        return isAt ? 1 : 0;
    }

    public int X_MasAt(int row, int col)
    {
        int matchCount = 0;

        // sitting on 'A' - check for 'M','S' upper left, lower right; upper right, lower left; lower left, upper right; lower right, upper left
        matchCount += (letters[row - 1][col - 1] == 'M' && letters[row + 1][col + 1] == 'S') ? 1 : 0;
        matchCount += (letters[row - 1][col + 1] == 'M' && letters[row + 1][col - 1] == 'S') ? 1 : 0;
        matchCount += (letters[row + 1][col - 1] == 'M' && letters[row - 1][col + 1] == 'S') ? 1 : 0;
        matchCount += (letters[row + 1][col + 1] == 'M' && letters[row - 1][col - 1] == 'S') ? 1 : 0;

        return matchCount == 2 ? 1 : 0;
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
                letters.Add("###" + line + "###");
            }

            file.Close();
            int len = letters[0].Length;
            string pad = new string('#', len);
            for (int i = 0; i < 3; i++)
            {
                letters.Insert(0, pad);
                letters.Add(pad);
            }
        }
    }

}
