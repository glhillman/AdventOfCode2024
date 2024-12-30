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
    Dictionary<string, List<string>> _map = new();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        HashSet<(string, string, string)> sets = new();
        List<string> set = new();
        foreach (string one in _map.Keys.Where(k => k.StartsWith("t")))
        {
            foreach (string two in _map.Keys)
            {
                foreach (string three in _map.Keys)
                {
                    if (_map[one].Contains(two) && _map[one].Contains(three) && 
                        _map[two].Contains(one) && _map[two].Contains(three) &&
                        _map[three].Contains(one) && _map[three].Contains(two))
                    {
                        set.Clear();
                        set.Add(one);
                        set.Add(two);
                        set.Add(three);
                        set.Sort();
                        sets.Add((set[0], set[1], set[2]));
                    }
                }
            }
        }

        Console.WriteLine("Part1: {0}", sets.Count);
    }

    public void Part2()
    {
        HashSet<string> currentClique = new HashSet<string>(); // Current clique
        HashSet<string> potentialNodes = new HashSet<string>(_map.Keys); // Potential nodes
        HashSet<string> excludedNodes = new HashSet<string>(); // Excluded nodes
        HashSet<string> maxSet = new();
        
        FindCliques(_map, currentClique, potentialNodes, excludedNodes, ref maxSet);
        
        List<string> maxList = new List<string>(maxSet);
        maxList.Sort();
        string max = string.Join(",", maxList);
        
        Console.WriteLine("Part2: {0}", max);
    }

    //BronKerbosh algorithm
    private void FindCliques(Dictionary<string, List<string>> graph, HashSet<string> currentClique, HashSet<string> potentialNodes, HashSet<string> excludedNodes, ref HashSet<string> maxSet)
    {
        if (!potentialNodes.Any() && !excludedNodes.Any())
        {
            if (currentClique.Count() > maxSet.Count())
            {
                maxSet = new HashSet<string>(currentClique);
            }
            return;
        }

        HashSet<string> pCopy = new HashSet<string>(potentialNodes);

        foreach (string node in pCopy)
        {
            currentClique.Add(node);
            HashSet<string> newP = new HashSet<string>(potentialNodes.Intersect(graph[node]));
            HashSet<string> newX = new HashSet<string>(excludedNodes.Intersect(graph[node]));

            FindCliques(graph, currentClique, newP, newX, ref maxSet);

            currentClique.Remove(node);
            potentialNodes.Remove(node);
            excludedNodes.Add(node);
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
                string[] parts = line.Split('-');
                if (_map.ContainsKey(parts[0]) == false)
                {
                    _map[parts[0]] = new List<string>();
                }
                _map[parts[0]].Add(parts[1]);
                if (_map.ContainsKey(parts[1]) == false)
                {
                    _map[parts[1]] = new List<string>();
                }
                _map[parts[1]].Add(parts[0]);
            }

            file.Close();
        }
    }

}
