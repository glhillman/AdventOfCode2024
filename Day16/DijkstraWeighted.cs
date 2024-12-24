using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class DijkstraWeighted
    {
        private static readonly (int dx, int dy)[] Directions = {
        (1, 0), // Right
        (-1, 0), // Left
        (0, 1), // Down
        (0, -1), // Up
    };

        public class Node : IComparable<Node>
        {
            public int X { get; }
            public int Y { get; }
            public int Direction { get; }
            public int Cost { get; }
            public Node? Prev { get; set; }

            public Node(int x, int y, int direction, int cost, Node? prev)
            {
                X = x;
                Y = y;
                Direction = direction;
                Cost = cost;
                Prev = prev;
            }

            public int CompareTo(Node other) => Cost.CompareTo(other.Cost);
            public override string ToString()
            {
                return string.Format("X/Y:{0}/{1}, Dir:{2}, Cost:{3}", X, Y, Direction, Cost);
            }
        }

        public static List<Node> Dijkstra(char[,] maze, (int x, int y) start, (int x, int y) end, int turnCost, bool all)
        {
            var dist = new Dictionary<(int x, int y, int direction), int>();
            var pq = new PriorityQueue<Node, int>();

            List<Node> paths = new();

            for (int i = 0; i < 4; i++)
            {
                pq.Enqueue(new Node(start.x, start.y, i, i == 0 ? 0 : turnCost, null), 0);
                dist[(start.x, start.y, i)] = 0;
            }

            while (pq.Count > 0)
            {
                var current = pq.Dequeue();

                if ((current.X, current.Y) == end)
                {
                    paths.Add(current);
                    if (all)
                    {
                        while (pq.Count > 0 && pq.Peek().Cost == current.Cost)
                        {
                            current = pq.Dequeue();
                            if ((current.X, current.Y) == end)
                                paths.Add(current);
                        }
                    }
                    return paths;
                }

                for (int dir = 0; dir < Directions.Length; dir++)
                {
                    int nx = current.X + Directions[dir].dx;
                    int ny = current.Y + Directions[dir].dy;

                    if (maze[nx, ny] == '#')
                        continue;

                    int newCost = current.Cost + (current.Direction == dir ? 1 : turnCost + 1);

                    if (!dist.ContainsKey((nx, ny, dir)) || (all ? newCost <= dist[(nx, ny, dir)] : newCost < dist[(nx, ny, dir)])) // < is best, <= all best
                    {
                        dist[(nx, ny, dir)] = newCost;
                        pq.Enqueue(new Node(nx, ny, dir, newCost, current), newCost);
                    }
                }
            }

            return paths;
        }
    }
}
