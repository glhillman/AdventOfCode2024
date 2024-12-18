// See https://aka.ms/new-console-template for more information
using System.Numerics;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

public class Button
{
    public Button(char id, long dx, long dy)
    {
        ID = id;
        DX = dx;
        DY = dy;
    }
    public char ID { get; private set; }
    public long DX { get; private set; }
    public long DY { get; private set; }
    public override string ToString()
    {
        return string.Format("{0} DX/DY:{1}/{2}", ID, DX, DY);
    }
}
public class Claw
{
    public Claw(Button a, Button b, long prizeX, long prizeY)
    {
        A = a;
        B = b;        
        PrizeX = prizeX;
        PrizeY = prizeY;
    }
    public Button A { get; private set; }
    public Button B { get; private set; }
    public long PrizeX { get; set; }
    public long PrizeY { get; set; }
    public override string ToString()
    {
        return string.Format("{0}, {1}, PX/PY: {2}/{3}", A, B, PrizeX, PrizeY);
    }
}

internal class DayClass
{
    List<Claw> Claws = new();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        BigInteger totalCost = 0;
        foreach (Claw claw in Claws)
        {
            totalCost += CramersRule(claw);
        }

        Console.WriteLine("Part1: {0}", totalCost);
    }

    public void Part2()
    {
        long totalCost = 0;
        foreach (Claw claw in Claws)
        {
            claw.PrizeX += 10000000000000;
            claw.PrizeY += 10000000000000;
            totalCost += CramersRule(claw);
        }

        Console.WriteLine("Part2: {0}", totalCost);
    }

    private long CramersRule(Claw claw)
    {
        bool isValid;
        
        long D = claw.A.DX * claw.B.DY - claw.A.DY * claw.B.DX;
        long D_x = claw.PrizeX * claw.B.DY - claw.PrizeY * claw.B.DX;
        long D_y = claw.A.DX * claw.PrizeY - claw.A.DY * claw.PrizeX;
        long A = D_x / D;
        long B = D_y / D;

        // A & B must be integers to have a valid result
        isValid = (A * D == D_x && B * D == D_y);
        return isValid ? (A * 3 + B) : 0;
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
                string[] parts = line.Split(' ', ',', ':', '=', 'X', 'Y');
                Button buttonA = new Button(parts[1][0], long.Parse(parts[4]), long.Parse(parts[7]));
                line = file.ReadLine();
                parts = line.Split(' ', ',', ':', '=', 'X', 'Y');
                Button buttonB = new Button(parts[1][0], long.Parse(parts[4]), long.Parse(parts[7]));
                line = file.ReadLine();
                parts = line.Split(' ', ',', ':', '=', 'X', 'Y');
                Claw claw = new Claw(buttonA, buttonB, long.Parse(parts[4]), long.Parse(parts[8]));
                line += file.ReadLine();

                Claws.Add(claw);
            }

            file.Close();
        }
    }

}

