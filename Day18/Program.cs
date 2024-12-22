// See https://aka.ms/new-console-template for more information
using Day18;
using System;
using System.Collections.Generic;

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
    List<(int x, int y)> _cells = new();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int size = 71;
        char[,] maze = new char[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                maze[x,y] = '.';
            }
        }
        for (int i = 0; i < 1024; i++)
        {
            maze[_cells[i].x, _cells[i].y] = '#';
        }
        int dist = Dijkstra.FindShortestPath(maze, (0, 0), (size - 1, size - 1));

        Console.WriteLine("Part1: {0}", dist);
    }

    public void Part2()
    {
        int size = 71;
        int startSize = 1024;
        int maxIndex = _cells.Count -1;

        char[,] maze = new char[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                maze[x, y] = '.';
            }
        }
        for (int i = 0; i < startSize; i++)
        {
            maze[_cells[i].x, _cells[i].y] = '#';
        }
        bool bigStepsFinished = false;
        int stepSize = 1000;
        int nSteps = 1;
        while (!bigStepsFinished)
        {
            MarkRange(maze, startSize, stepSize * nSteps, '#');
            if (Dijkstra.FindShortestPath(maze, (0, 0), (size - 1, size - 1)) < 1)
            {
                MarkRange(maze, startSize, stepSize * nSteps, '.');
                if (stepSize / 10 == 1)
                {
                    MarkRange(maze, startSize, stepSize * (nSteps - 1), '#');
                    bigStepsFinished = true;
                }
                else
                {
                    MarkRange(maze, startSize, stepSize * (nSteps - 1), '#');
                    startSize += stepSize * (nSteps - 1);
                    stepSize /= 10;
                    nSteps = 1;
                }
            }
            else
            {
                nSteps++;
            }
        }
        int dist;
        do
        {
            startSize++;
            maze[_cells[startSize].x, _cells[startSize].y] = '#';
            dist = Dijkstra.FindShortestPath(maze, (0, 0), (size - 1, size - 1));
        } while (dist > 0);


        Console.WriteLine("Part2: {0},{1} ({2})", _cells[startSize].x, _cells[startSize].y, startSize );
    }

    public void MarkRange(char[,] maze, int start, int count, char c)
    {
        for (int i = 0; i+start < _cells.Count && i < count; i++)
        {
            maze[_cells[i + start].x, _cells[i+start].y] = c;
        }
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
                string[] parts = line.Split(',');
                int x = int.Parse(parts[0]);
                int y = int.Parse(parts[1]);
                _cells.Add((x, y));
            }

            file.Close();
        }
    }

}
