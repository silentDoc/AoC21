using AoC21.Common;

namespace AoC21.Day09
{
    internal class SmokeBasin
    {
        Dictionary<Coord2D, int> caves = new();

        public void ParseInput(List<string> input)
        { 
            for(int row=0; row<input.Count; row++)
                for(int col=0; col<input[row].Length; col++)
                    caves[(col, row)] = int.Parse( input[row][col].ToString());
        }

        List<Coord2D> LowPoints()
        {
            HashSet<Coord2D> lowPoints = new();
            foreach (var key in caves.Keys)
            {
                var surroundings = key.GetNeighbors().Where(x => caves.ContainsKey(x));
                if (surroundings.Any(x => caves[x] <= caves[key]))
                    continue;
                lowPoints.Add(key);
            }

            return lowPoints.ToList();
        }

        int ThreeLargestBasins()
        {
            var lowPoints = LowPoints();
            Dictionary<Coord2D, int> basinToSize = new();

            foreach (var key in lowPoints)
            {
                List<Coord2D> basinPoints = new() { key };
                var candidates = key.GetNeighbors().Where(x => caves.ContainsKey(x) && caves[x] != 9).ToList();

                while (candidates.Any())
                {
                    basinPoints.AddRange(candidates);
                    candidates = basinPoints.SelectMany(x => x.GetNeighbors()).Distinct().Where(x => !basinPoints.Contains(x) && caves.ContainsKey(x) && caves[x] != 9).ToList();
                }

                basinToSize[key] = basinPoints.Count;
            }

            var values = basinToSize.Values.OrderByDescending(x => x).ToList();
            return values[0] * values[1] * values[2];  
        }

        public int Solve(int part = 1)
            => part == 1 ? LowPoints().Sum(x => caves[x] + 1) : ThreeLargestBasins();
    }
}
