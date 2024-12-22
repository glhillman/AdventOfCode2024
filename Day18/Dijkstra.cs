using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    internal class Dijkstra
    {
        public static int FindShortestPath(char[,] maze, (int row, int col) start, (int row, int col) end)
        {
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);

            // Directions for moving up, down, left, and right
            var directions = new (int dRow, int dCol)[] {
            (-1, 0), (1, 0), (0, -1), (0, 1)
            };

            // Priority queue to hold nodes to explore
            var priorityQueue = new SortedSet<(int distance, (int row, int col) position)>();
            priorityQueue.Add((0, start));

            // Distance dictionary to track shortest distance to each cell
            var distances = new Dictionary<(int, int), int> { [start] = 0 };

            while (priorityQueue.Count > 0)
            {
                var (currentDistance, currentPos) = priorityQueue.Min;
                priorityQueue.Remove(priorityQueue.Min);

                if (currentPos == end)
                    return currentDistance;

                foreach (var (dRow, dCol) in directions)
                {
                    int newRow = currentPos.row + dRow;
                    int newCol = currentPos.col + dCol;

                    if (newRow < 0 || newCol < 0 || newRow >= rows || newCol >= cols)
                        continue; // Out of bounds

                    if (maze[newRow, newCol] == '#')
                        continue; // Wall or obstacle

                    var neighbor = (newRow, newCol);
                    int newDist = currentDistance + 1;

                    if (!distances.ContainsKey(neighbor) || newDist < distances[neighbor])
                    {
                        if (distances.ContainsKey(neighbor))
                            priorityQueue.Remove((distances[neighbor], neighbor));

                        distances[neighbor] = newDist;
                        priorityQueue.Add((newDist, neighbor));
                    }
                }
            }

            return -1; // No path found
        }
    }
}
