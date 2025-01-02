// See https://aka.ms/new-console-template for more information
using Day24;
using System.Diagnostics;

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
    Dictionary<string, int> _inputs = new(); 
    List<Gate> _gateList = new();
    string _binary;
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        WireGates();
        List<Gate> zout = _gateList.Where(g => g.OutName[0] == 'z').OrderBy(g => g.OutName).ToList();
        _binary = "";
        foreach (Gate g in zout)
        {
            _binary = g.OutValue.ToString() + _binary;
        }
        long rslt = Convert.ToInt64(_binary, 2);

        Console.WriteLine("Part1: {0}", rslt);
    }

    public void Part2()
    {
        List<string> badOutputs = new();
        MarkAlias();
        _gateList.Sort((a, b) => a.AliasOut.CompareTo(b.AliasOut));
        Gate anchor = _gateList[0];
        foreach (Gate g in _gateList)
        {
            if (g.AliasOut == "AND00")
                g.AliasOut = "CARRY00";
            if (g.In1 == anchor.OutName)
                g.Alias1 = "CARRY00";
            if (g.In2 == anchor.OutName)
                g.Alias2 = "CARRY00";
        }
        for (int i = 0; i < 4; i++)
        {
            Substitute();
            // look for a broken
            Gate? misWired = _gateList.FirstOrDefault(g => g is XOR && (g.Alias1.StartsWith("CARRY") || g.Alias2.StartsWith("CARRY")) && g.OutName[0] != 'z');
            // swap the incorrect output of g2 with the gate that contains the z output we need
            if (misWired != null)
            {
                string n = "";
                string z;
                string wrongOut;
                if (misWired.Alias1.StartsWith("XOR"))
                    n = misWired.Alias1.Substring(3);
                else if (misWired.Alias2.StartsWith("XOR"))
                    n = misWired.Alias2.Substring(3);
                z = "z" + n;
                wrongOut = misWired.OutName;
                badOutputs.Add(z);
                badOutputs.Add(wrongOut);
                Gate? misWired2 = _gateList.First(g => g.OutName == z); // misWired 2 currently goes to z(n). Wrong.
                misWired.OutName = z;
                misWired.AliasOut = z;
                misWired2.OutName = wrongOut;
                misWired2.AliasOut = wrongOut;

                //for (int num = 1; num < 45; num++)
                //{
                //    DumpAdder(num);
                //}
                //Console.ReadLine();
            }
        }


        // look for a z result that isn't formed as XOR(n) xor CARRY(n-1)
        Gate? badZ = _gateList.FirstOrDefault(g => g.AliasOut[0] == 'z' && (!(g.Alias1.StartsWith("XOR") || g.Alias1.StartsWith("CARRY")) ||
                                                                          !(g.Alias2.StartsWith("XOR") || g.Alias2.StartsWith("CARRY"))));
        // badZ is a malformed assignment to the z value
        // it's components should be XOR(n) AND CARRY(n-1) -> z(n)
        // find which name is wrong & store it.
        if (badZ != null)
        {
            string badWire = "";
            if (badZ.Alias1.StartsWith("XOR") || badZ.Alias1.StartsWith("CARRY"))
            {
                // Alias1 is ok so Alias2 is bad
                badWire = badZ.In2;
            }
            else
            {
                badWire = badZ.In1;
            }
            badOutputs.Add(badWire);
            // the bad wire should be XOR(n) -> find it and record it's actual name
            string xorN = "XOR" + badZ.OutName.Substring(1);
            Gate? good = _gateList.FirstOrDefault(g => g.AliasOut == xorN);
            if (good != null)
            {
                badOutputs.Add(good.OutName);
            }
        }
        badOutputs.Sort();
        string swapList = string.Join(",", badOutputs);

        Console.WriteLine("Part2: {0}", swapList);

    }

    private void DumpAdder(int n)
    {
        string xn = string.Format("x{0:D2}", n);
        string yn = string.Format("y{0:D2}", n);
        string xorn = string.Format("XOR{0:D2}", n);
        string andn = string.Format("AND{0:D2}", n);
        string carryLess = string.Format("CARRY{0:D2}", n - 1);
        string carry = string.Format("CARRY{0:D2}", n);
        string carryInter = string.Format("CARRY_INTERMEDIATE{0:D2}", n);

        Gate? g = _gateList.FirstOrDefault(g => g is XOR && (g.Alias1 == xn || g.Alias1 == yn) && (g.Alias2 == xn || g.Alias2 == yn));
        if (g != null)
            Console.WriteLine("{0} XOR {1} -> {2}", g.Alias1, g.Alias2, g.AliasOut);
        g = _gateList.FirstOrDefault(g => g is AND && (g.Alias1 == xn || g.Alias1 == yn) && (g.Alias2 == xn || g.Alias2 == yn));
        if (g != null)
            Console.WriteLine("{0} AND {1} -> {2}", g.Alias1, g.Alias2, g.AliasOut);
        g = _gateList.FirstOrDefault(g => g is XOR && (g.Alias1 == xorn || g.Alias1 == carryLess) && (g.Alias2 == xorn || g.Alias2 == carryLess));
        if (g != null)
            Console.WriteLine("{0} XOR {1} -> {2}", g.Alias1, g.Alias2, g.AliasOut);
        g = _gateList.FirstOrDefault(g => g is AND && (g.Alias1 == xorn || g.Alias1 == carryLess) && (g.Alias2 == xorn || g.Alias2 == carryLess));
        if (g != null)
            Console.WriteLine("{0} AND {1} -> {2}", g.Alias1, g.Alias2, g.AliasOut);
        g = _gateList.FirstOrDefault(g => g is OR && (g.Alias1 == andn || g.Alias1 == carryInter) && (g.Alias2 == andn || g.Alias2 == carryInter));
        if (g != null)
            Console.WriteLine("{0} AND {1} -> {2}", g.Alias1, g.Alias2, g.AliasOut);
        Console.WriteLine();

    }
    private void Substitute()
    {
        Gate? g1;
        for (int n = 0; n <= 45; n++)
        {
            string srch = string.Format("CARRY{0:D2}", n);
            string target = string.Format("CARRY_INTERMEDIATE{0:D2}", n + 1);
            g1 = _gateList.FirstOrDefault(g => g is AND && (g.Alias1 == srch || g.Alias2 == srch) && g.AliasOut.StartsWith("CARRY") == false);
            if (g1 != null)
            {
                g1.AliasOut = target;
                foreach (Gate g in _gateList)
                {
                    if (g.In1 == g1.OutName)
                        g.Alias1 = target;
                    if (g.In2 == g1.OutName)
                        g.Alias2 = target;
                }
            }
            srch = target;
            target = string.Format("CARRY{0:D2}", n + 1);
            g1 = _gateList.FirstOrDefault(g => g is OR && (g.Alias1 == srch || g.Alias2 == srch) && g.AliasOut.StartsWith("CARRY") == false);
            if (g1 != null)
            {
                g1.AliasOut = target;
                foreach (Gate g in _gateList)
                {
                    if (g.In1 == g1.OutName)
                        g.Alias1 = target;
                    if (g.In2 == g1.OutName)
                        g.Alias2 = target;
                }
            }
        }
    }

    private void MarkAlias()
    {
        foreach (Gate g in _gateList)
        {
            if ((g.In1[0] == 'x' || g.In1[0] == 'y') && (g.In2[0] == 'x' || g.In2[0] == 'y'))
            {
                string num1 = g.In1.Substring(1);
                string num2 = g.In2.Substring(1);
                if (num1 == num2)
                {
                    g.AliasOut = g.GetType().Name + num1;
                    foreach (Gate g2 in _gateList)
                    {
                        if (g2.In1 == g.OutName)
                        {
                            g2.Alias1 = g.AliasOut;
                        }
                        if (g2.In2 == g.OutName)
                        {
                            g2.Alias2 = g.AliasOut;
                        }
                    }
                }
            }
                
        }

    }
    private void WireGates()
    {
        // wire the direct inputs
        foreach (string input in _inputs.Keys)
        {
            foreach (Gate gate in _gateList)
            {
                if (gate.In1 == input && gate.In1Value.HasValue == false)
                {
                    gate.In1Value = _inputs[input];
                }
                if (gate.In2 == input && gate.In2Value.HasValue == false)
                {
                    gate.In2Value = _inputs[input];
                }
            }
        }

        // now loop through and wire the gates that rely on other gates
        bool changesMade = true;
        while (changesMade)
        {
            changesMade = false;
            List<Gate> complete = _gateList.Where(g => g.OutValue.HasValue).ToList();
            foreach (Gate completeGate in complete)
            {
                foreach (Gate partialGate in _gateList.Where(g => g.In1 == completeGate.OutName && g.In1Value == null))
                {
                    partialGate.In1Value = completeGate.OutValue;
                    changesMade = true;
                }
                foreach (Gate partialGate in _gateList.Where(g => g.In2 == completeGate.OutName && g.In2Value == null))
                {
                    partialGate.In2Value = completeGate.OutValue;
                    changesMade = true;
                }
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
            Gate gate = new Gate("dummy", "dummy", "dummy");
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split(':', ' ', '-', '>');
                if (parts.Length == 7)
                {
                    switch (parts[1])
                    {
                        case "AND":
                            gate = new AND(parts[0], parts[2], parts[6]);
                            break;
                        case "OR":
                            gate = new OR(parts[0], parts[2], parts[6]);
                            break;
                        case "XOR":
                            gate = new XOR(parts[0], parts[2], parts[6]);
                            break;
                    }
                    _gateList.Add(gate);
                }
                else if (parts.Length == 3)
                {
                    _inputs[parts[0]] = int.Parse(parts[2]);
                }
            }

            file.Close();
        }
    }

}
