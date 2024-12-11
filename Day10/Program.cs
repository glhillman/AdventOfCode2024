// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

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
    List<string> map = new();
    List<(int, int)> trailHeads = new();
    bool[,] visited;
    public DayClass()
    {
        LoadData();
        visited = new bool[map.Count, map.Count];
    }

    public void Part1()
    {
        int allTrails = 0;
        foreach ((int row, int col) in  trailHeads)
        {
            int nTrails = 0;
            CurseTrail(row, col, '0', ref nTrails, false);
            allTrails += nTrails;
            InitVisited();
        }

        Console.WriteLine("Part1: {0}", allTrails);
    }

    public void Part2()
    {
        int allTrails = 0;
        foreach ((int row, int col) in trailHeads)
        {
            int nTrails = 0;
            CurseTrail(row, col, '0', ref nTrails, true);
            allTrails += nTrails;
        }

        Console.WriteLine("Part2: {0}", allTrails);
    }

    public void CurseTrail(int row, int col, char curNum, ref int nTrails, bool doRating)
    {
        if (doRating == false)
        {
            visited[row, col] = true;
        }
        if (map[row][col] == '9')
        {
            nTrails++;
            return;
        }
        char nextNum = (char)(curNum + 1);
        if (map[row + 1][col] == nextNum && visited[row + 1, col] == false)
            CurseTrail(row + 1, col, nextNum, ref nTrails, doRating);
        if (map[row - 1][col] == nextNum && visited[row - 1, col] == false)
            CurseTrail(row - 1, col, nextNum, ref nTrails, doRating);
        if (map[row][col + 1] == nextNum && visited[row, col + 1] == false)
            CurseTrail(row, col + 1, nextNum, ref nTrails, doRating);
        if (map[row][col - 1] == nextNum && visited[row, col - 1] == false)
            CurseTrail(row, col - 1, nextNum, ref nTrails, doRating);
        return;
    }

    private void InitVisited()
    {
        for (int i = 0; i < visited.GetLength(0); i++)
        {
            for (int j = 0; j < visited.GetLength(1); j++)
            {
                visited[i,j] = false;
            }
        }
    }
    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            line = file.ReadLine();
            int lineLen = line.Length;
            string pad = new string('A', lineLen + 2);
            map.Add(pad);
            map.Add('A' + line + 'A');
            while ((line = file.ReadLine()) != null)
            {
                map.Add('A' + line + 'A');
            }
            map.Add(pad);
            file.Close();
        }
        // find the trailheads
        for (int row = 1; row < map.Count-1; row++)
        {
            for (int col = 1; col < map.Count-1; col++)
            {
                if (map[row][col] == '0')
                {
                    trailHeads.Add((row, col));
                }
            }
        }
    }

}

