using AoC21.Common;

namespace AoC21.Day15
{
    internal class CaveNavigator
    {
        Dictionary<Coord2D, int> cave = new();
        Dictionary<Coord2D, int> distances = new();

        public void ParseInput(List<string> input)
        {
            for (int j = 0; j < input.Count; j++)
                for (int i = 0; i < input[0].Length; i++)
                    cave[(i, j)] = int.Parse(input[j][i].ToString());
        }

        // "to determine the total risk of an entire path, add up the risk levels of each position you enter"
        // That can be seen as an edge -> Dijsktra ? 
        int Dijsktra()
        {
            Coord2D start = new Coord2D(0, 0);
            Coord2D end = new Coord2D(cave.Keys.Max(k => k.x), cave.Keys.Max(k => k.y));
            PriorityQueue<Coord2D, int> priorityQueue = new();

            foreach (var key in cave.Keys)
                distances[key] = int.MaxValue;

            distances[start] = 0;
            priorityQueue.Enqueue(start, 0);

            while (priorityQueue.Count > 0)
            {
                var current = priorityQueue.Dequeue();
                
                if (current.Equals(end))
                    break;
            
                foreach (var neighbor in current.GetNeighbors().Where(n => cave.ContainsKey(n)))
                {
                    int newDist = distances[current] + cave[neighbor];
                    if (newDist < distances[neighbor])
                    {
                        distances[neighbor] = newDist;
                        priorityQueue.Enqueue(neighbor, newDist);
                    }
                }
            }

            // The shortest path cost to the end position
            return distances[end];
        }

        public int Solve(int part = 1)
            => Dijsktra();
    }
}
