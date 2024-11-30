using AoC21.Common;

namespace AoC21.Day15
{
    internal class CaveNavigator
    {
        Dictionary<Coord2D, int> cave = new();
        Dictionary<Coord2D, int> distances = new();

        int rows = 0;
        int cols = 0;

        public void ParseInput(List<string> input)
        {
            for (int j = 0; j < input.Count; j++)
                for (int i = 0; i < input[0].Length; i++)
                    cave[(i, j)] = int.Parse(input[j][i].ToString());

            rows = input.Count;
            cols = input[0].Length;
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

        int DijsktraFullCave()
        {
            Coord2D start = new Coord2D(0, 0);

            int endX = cols * 5 - 1;
            int endY = rows * 5 - 1;
            Coord2D end = new Coord2D(endX, endY);
            PriorityQueue<Coord2D, int> priorityQueue = new();

            for(int i=0;i<=endX; i++)
                for (int j = 0; j <=endY; j++)
                    distances[(i,j)] = int.MaxValue;

            distances[start] = 0;
            priorityQueue.Enqueue(start, 0);

            while (priorityQueue.Count > 0)
            {
                var current = priorityQueue.Dequeue();

                if (current.Equals(end))
                    break;

                foreach (var neighbor in current.GetNeighbors().Where(n => distances.ContainsKey(n)))
                {
                    var normX = neighbor.x % cols;
                    var normY = neighbor.y % rows;

                    var quadX = neighbor.x / cols;
                    var quadY = neighbor.y / rows;

                    int distNeighbor = cave[(normX, normY)] + quadX + quadY;
                    distNeighbor = distNeighbor > 9 ? distNeighbor-9 : distNeighbor;

                    int newDist = distances[current] + distNeighbor;
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
            => part == 1 ? Dijsktra() : DijsktraFullCave();
    }
}
