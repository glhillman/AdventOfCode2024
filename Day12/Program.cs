// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Runtime.Versioning;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

public class Plot
{
    public Plot(char id, int row, int col, bool used)
    {
        ID = id;
        Row = row;
        Col = col;
        Used = used;
        FenceSides = 0;
    }
    public char ID { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public bool Used { get; set; }
    public int FenceSides { get; set; } // number of sides that need a fence
    public int FenceLeft { get; set; }
    public int FenceRight { get; set; }
    public int FenceTop { get; set; }
    public int FenceBottom { get; set; }

    public override string ToString()
    {
        return string.Format("{0} R/C: {1}/{2}, Used: {3}, FenceSides: {4}", ID, Row, Col, Used.ToString(), FenceSides);
    }
}

public class Region
{
    public Region(char id)
    {
        ID = id;
        Plots = new();
    }
    public char ID { get; set; }
    public List<Plot> Plots { get; set; }
    public int Perimeter
    {
        get
        {
            int perimeter = 0;
            foreach (Plot plot in Plots)
            {
                perimeter += plot.FenceSides;
            }
            return perimeter;
        }
    }
    public int Area
    {
        get
        {
            return Plots.Count;
        }
    }
    public int Price
    {
        get
        {
            return Perimeter * Area;
        }
    }
}

public class HorizontalComparer : IComparer<Plot>
{
    public int Compare(Plot plot1, Plot plot2)
    {
        if (plot1.Row != plot2.Row)
            return plot1.Row - plot2.Row;
        return plot1.Col - plot2.Col;
    }
}

public class VerticalComparer : IComparer<Plot>
{
    public int Compare(Plot plot1, Plot plot2)
    {
        if (plot1.Col != plot2.Col)
            return plot1.Col - plot2.Col;
        return plot1.Row - plot2.Row;
    }
}


internal class DayClass
{
    Plot[,] Garden = new Plot[1, 1];
    List<Region> Regions = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int size = Garden.GetLength(0);
        for (int row = 0; row <  size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Plot plot = Garden[row, col];
                if (plot.Used == false)
                {
                    Region region = new Region(plot.ID);
                    RecurseRegion(region, row, col);
                    Regions.Add(region);
                }
            }
        }
        int price = 0;
        foreach (Region region in Regions)
        {
            price += region.Price;
        }

        Console.WriteLine("Part1: {0}", price);
    }

    public void Part2()
    {
        int totalPrice = 0;
        HorizontalComparer horizontalComparer = new();
        VerticalComparer verticalComparer = new();

        foreach (Region region in Regions)
        {
            int regionPrice = 0;
            List<Plot> subPlots = region.Plots.Where(p => p.FenceTop == 1).ToList();
            regionPrice += CountHorizontalSides(subPlots, horizontalComparer);
            subPlots = region.Plots.Where(p => p.FenceBottom == 1).ToList();
            regionPrice += CountHorizontalSides(subPlots, horizontalComparer);
            subPlots = region.Plots.Where(p => p.FenceLeft == 1).ToList();
            regionPrice += CountVerticalSides(subPlots, verticalComparer);
            subPlots = region.Plots.Where(p => p.FenceRight == 1).ToList();
            regionPrice += CountVerticalSides(subPlots, verticalComparer);
            totalPrice += regionPrice * region.Area;
        }

        Console.WriteLine("Part2: {0}", totalPrice);
    }

    private int CountHorizontalSides(List<Plot> plots, HorizontalComparer hComp)
    {
        plots.Sort(hComp);
        int sides = 0;
        if (plots.Count > 0)
        {
            int i = 0;
            while (i < plots.Count)
            {
                sides++;
                while (i < plots.Count - 1 && plots[i].Row == plots[i + 1].Row && plots[i].Col + 1 == plots[i + 1].Col)
                {
                    i++;
                }
                
                if (i < plots.Count)
                {
                    i++;
                }
            }
        }
        else
        {
            sides = 1;
        }
        return sides;
    }

    private int CountVerticalSides(List<Plot> plots, VerticalComparer vComp)
    {
        plots.Sort(vComp);
        int sides = 0;
        if (plots.Count > 0)
        {
            int i = 0;
            while (i < plots.Count)
            {
                sides++;
                while (i < plots.Count - 1 && plots[i].Col == plots[i + 1].Col && plots[i].Row + 1 == plots[i + 1].Row)
                {
                    i++;
                }

                if (i < plots.Count)
                {
                    i++;
                }
            }
        }
        else
        {
            sides = 1;
        }
        return sides;
    }

    private void RecurseRegion(Region region, int row, int col)
    {
        if (Garden[row, col].Used || Garden[row, col].ID != region.ID)
            return;
        Plot plot = Garden[row, col];
        plot.Used = true;
        plot.FenceLeft = Garden[row, col - 1].ID == region.ID ? 0 : 1;
        plot.FenceRight = Garden[row, col + 1].ID == region.ID ? 0 : 1;
        plot.FenceTop = Garden[row - 1, col].ID == region.ID ? 0 : 1;
        plot.FenceBottom = Garden[row + 1, col].ID == region.ID ? 0 : 1;
        plot.FenceSides = plot.FenceLeft + plot.FenceRight + plot.FenceTop + plot.FenceBottom;
        region.Plots.Add(plot);
        RecurseRegion(region, row, col - 1);
        RecurseRegion(region, row, col + 1);
        RecurseRegion(region, row - 1, col);
        RecurseRegion(region, row + 1, col);
    }

    private void LoadData()
    {
        Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";
        if (File.Exists(inputFile))
        {
            int row;
            int lineLen;
            string? line;
            StreamReader file = new StreamReader(inputFile);
            line = file.ReadLine();
            lineLen = line.Length;
            Garden = new Plot[lineLen + 2, lineLen + 2];
            row = 0;
            string pad = new string('#', lineLen + 2);
            StoreLine(pad, row++);
            StoreLine("#" + line + "#", row++);
            while ((line = file.ReadLine()) != null)
            {
                StoreLine("#" + line + "#", row++);
            }
            StoreLine(pad, row);

            file.Close();
        }
    }

    private void StoreLine(string line, int row)
    {
        for (int col = 0; col < line.Length; col++)
        {
            char c = line[col];
            Garden[row, col] = new Plot(line[col], row, col, c == '#');
        }
    }

}
