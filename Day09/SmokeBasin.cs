using AoC20.Common;

namespace AoC21.Day09
{
    internal class SmokeBasin
    {
        Dictionary<Coord2D, int> basin = new();

        public void ParseInput(List<string> input)
        { 
            for(int row=0; row<input.Count; row++)
                for(int col=0; col<input[row].Length; col++)
                    basin[(col, row)] = int.Parse( input[row][col].ToString());
        }

        int TotalRiskLevel()
        {
            HashSet<Coord2D> lowPoints = new();
            foreach (var key in basin.Keys)
            {
                var surroundings = key.GetNeighbors().Where(x => basin.ContainsKey(x));
                if (surroundings.Any(x => basin[x] <= basin[key]))
                    continue;
                lowPoints.Add(key);
            }

            return lowPoints.ToList().Sum(x => basin[x] + 1);
        }

        public int Solve(int part = 1)
            => TotalRiskLevel();
    }
}
