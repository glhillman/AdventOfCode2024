// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;
using System.Text;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

public class Computer
{
    public Computer(ulong a, ulong b, ulong c, int[] instructions)
    {
        A = a;
        B = b;
        C = c;
        Instructions = instructions;
    }

    public ulong A { get; set; }
    public ulong B { get; set; }
    public ulong C { get; set; }
    public int[] Instructions { get; set; }
    private ulong Combo(ulong value)
    {
        switch (value)
        {
            case 4:
                return A;
            case 5:
                return B;
            case 6:
                return C;
            default:
                //case 0:
                //case 1:
                //case 2:
                //case 3:
                return value;
        }
    }
    public List<int> Run(ulong aValue, List<int> output)
    {
        output.Clear();
        A = aValue;
        B = 0;
        C = 0;
        int ip = 0;
        while (ip < Instructions.Length-1)
        {
            int instruction = Instructions[ip++];
            ulong operand = (ulong)Instructions[ip++];
            switch (instruction)
            {
                case 0: // adv: A / 2^combo(operand) -> A
                    A >>>= (int)Combo(operand);
                    break;
                case 1: // bxl: B ^ operand -> B
                    B ^= operand;
                    break;
                case 2: // bst: B = Combo(operand) % 8
                    B = Combo(operand) % 8;
                    break;
                case 3: // jnz: ip = operand if A != 0
                    if (A != 0)
                        ip = (int)operand;
                    break;
                case 4: // bxc: B ^= C
                    B ^= C;
                    break;
                case 5: // Out: outputs Combo(operand) % 8
                    {
                        int outValue = (int)(Combo(operand) % 8);
                        output.Add(outValue);

                    }
                    break;
                case 6: // bdv: A / 2^combo(operand) -> B
                    B = A >>> (int)Combo(operand);
                    break;
                case 7: // cdv:  A / 2^combo(operand) -> C
                    C = A >>> (int)Combo(operand);
                    break;
            }
        }

        return output;
    }
}
internal class DayClass
{

    public DayClass()
    {
    }

    public void Part1()
    {
        Computer computer = new Computer(35200350, 0, 0, new int[] { 2, 4, 1, 2, 7, 5, 4, 7, 1, 3, 5, 5, 0, 3, 3, 0 });
        List<int> rslt = new(); 
        computer.Run(computer.A, rslt);

        Console.Write("Part1: ");
        foreach (int i in rslt)
        {
            Console.Write(i + ",");
        }
        Console.WriteLine();
    }

    public void Part2()
    {
        int[] target = new int[] { 2, 4, 1, 2, 7, 5, 4, 7, 1, 3, 5, 5, 0, 3, 3, 0 };
        List<int> rslt = new();
        int targetLen = target.Length;
        bool finished = false;
        Computer computer = new Computer(0, 0, 0, target);
        ulong A = 0;
        while (!finished)
        {
            computer.Run(A, rslt);
            bool matched = true;
            // check to see if we match the last <rslt.Count> digits in target
            for (int i = 0, j = targetLen-rslt.Count; matched && i < rslt.Count; i++, j++)
            {
                matched = rslt[i] == target[j]; 
            }
            if (matched)
            {
                if (rslt.Count == targetLen)
                {
                    finished = true;
                }
                else
                {
                    // multiply A by 8 & continue
                    A *= 8;
                }
            }
            else
            {
                A++;
            }
        }

        Console.WriteLine("Part2: {0}", A);
    }
}
