using AoC21.Common;

namespace AoC21.Day11
{
    internal class OcutpusNavi
    {
        Dictionary<Coord2D, int> energyMap = new();
        public void ParseInput(List<string> input)
        {
            for (int row = 0; row < input.Count; row++)
                for (int col = 0; col < input[row].Length; col++)
                    energyMap[(col, row)] = int.Parse(input[row][col].ToString());
        }

        int DoStep()
        {
            HashSet<Coord2D> flashedInRound = new();

            foreach (var key in energyMap.Keys)
                energyMap[key] = (energyMap[key] + 1) % 10;

            var flashes = energyMap.Keys.Where(x => energyMap[x] == 0).ToList();
            foreach (var pos in flashes)
                flashedInRound.Add(pos);

            while (flashes.Any())
            {
                var newFlashes = new List<Coord2D>();

                foreach (var key in flashes)
                {
                    var surroundings = key.GetNeighbors8().Where(k => !flashedInRound.Contains(k) && energyMap.ContainsKey(k)).ToList();
                    surroundings.ForEach(k => energyMap[k] = (energyMap[k] + 1) % 10);
                    var nearbyFlashes = surroundings.Where(k => energyMap[k] == 0).ToList();

                    nearbyFlashes.ForEach(x => flashedInRound.Add(x));
                    nearbyFlashes.ForEach(x => newFlashes.Add(x));
                }
                flashes = newFlashes.Distinct().ToList();
            }
            return flashedInRound.Count();
        }

        int FindNumFlashes()
        {
            int retVal = 0;
            for (int step = 0; step < 100; step++)
                retVal += DoStep();

            return retVal;
        }

        int FindSyncStep()
        {
            int retVal = 0;
            while (energyMap.Values.Any(x => x != 0))
            {
                DoStep();
                retVal++;
            }

            return retVal;
        }

        public int Solve(int part = 1)
            => part == 1 ? FindNumFlashes() : FindSyncStep();
    }
}
