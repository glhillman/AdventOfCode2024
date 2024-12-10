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
    // part 1 structures
    List<int> filesMaster = new();
    List<int> gapsMaster = new();

    // part 2 structure
    List<(int id, int pos, int count)> combo = new();
    public DayClass()
    {
        LoadData();

        int id = 0;
        int pos = 0;

        for (int i = 0; i < filesMaster.Count; i++)
        {
            combo.Add((id++, pos, filesMaster[i]));
            pos += filesMaster[i];
            if (i < filesMaster.Count - 1)
            {
                combo.Add((-1, pos, gapsMaster[i]));
                pos += gapsMaster[i];
            }
        }
    }

    public void Part1()
    {
        List<int> files = new List<int>(filesMaster);
        List<int> gaps = new List<int>(gapsMaster);
        long sum = 0;
        int idHead = 0; // points to current "id" block of files 
        int pos = 0; // virtual position of files
        int idTail = files.Count - 1; // points to "id" of trailing file block
        int gapIndex = 0; // points to current gap to fill from tail

        // process files at current idHead
        while (idHead < idTail)
        {
            while (files[idHead] != 0)
            {
                sum += pos++ * idHead;
                files[idHead]--;
            }
            idHead++;
            // fill in gaps from idTail
            bool gapFilled = false;
            while (!gapFilled)
            {
                while (gaps[gapIndex] != 0 && idHead <= idTail)
                {
                    while (idTail >= 0 && files[idTail] != 0 && gaps[gapIndex] != 0)
                    {
                        sum += pos++ * idTail;
                        gaps[gapIndex]--;
                        files[idTail]--;
                    }
                    if (gaps[gapIndex] != 0 && files[idTail] == 0)
                    {
                        idTail--;
                    }
                }
                if (gaps[gapIndex] == 0 || idHead >= idTail)
                {
                    gapFilled = true;
                }
            }
            if (gaps[gapIndex] == 0)
            {
                gapIndex++;
            }
        }
        // cleanup
        while (files[idHead] != 0)
        {
            sum += pos++ * idHead;
            files[idHead]--;
        }


        Console.WriteLine("Part1: {0}", sum);
    }

    public void Part2()
    {
        int rightIndex = combo.Count - 1;
        while (rightIndex > 0)
        {
            (int id, int pos, int count) blockToMove;
            while (rightIndex > 0 && combo[rightIndex].id == -1)
            {
                rightIndex--;
            }
            if (rightIndex > 0)
            {
                bool moved = false;
                blockToMove = combo[rightIndex];
                // now search from the left for a spot the block can move to
                for (int i = 0; !moved && i < rightIndex; i++)
                {
                    if (combo[i].id == -1 && combo[i].count >= blockToMove.count)
                    {
                        int newId = blockToMove.id;
                        int newCount = blockToMove.count;
                        int slack = combo[i].count - blockToMove.count;
                        combo[i] = ((newId, combo[i].pos, newCount));
                        if (slack > 0)
                        {
                            combo.Insert(i+1, (-1, combo[i].pos + combo[i].count, slack));
                            rightIndex++;
                        }
                        combo[rightIndex] = (-1, combo[rightIndex].pos, combo[rightIndex].count);
                        moved = true;
                    }
                }
                rightIndex--;
            }
        }

        long sum = 0;
        foreach (var block in combo)
        {
            if (block.id != -1)
            {
                for (int i = 0; i < block.count; i++)
                {
                    sum += block.id * (block.pos + i);
                }
            }
        }
        
        Console.WriteLine("Part2: {0}", sum);
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
                for (int i = 0; i < line.Length; i+=2)
                {
                    filesMaster.Add(line[i] - '0');
                    if (i+1 < line.Length)
                        gapsMaster.Add(line[i+1] - '0');
                }
            }

            file.Close();
        }
    }

}
