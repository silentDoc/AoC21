namespace AoC21.Day07
{
    internal class CrabAligner
    {
        Dictionary<int, int> Lookup = new();
        List<int> crabs = new();
        public void ParseInput(List<string> input)
            => crabs = input[0].Split(",").Select(int.Parse).ToList();

        int Cost(int pos)
            => crabs.Select(x => Math.Abs(x - pos)).Sum();
        
        int CostInc(int pos)
        {
            if (!Lookup.ContainsKey(pos))
            {
                int cost = 0;
                for (int i = 0; i <= pos; i++)
                    cost += i;
                Lookup[pos] = cost;
            }

            return Lookup[pos];
        }

        int IncrementalCost(int pos)
            => crabs.Select(x => CostInc(Math.Abs(x - pos))).Sum();

        int AlignCrabs(int part = 1)
            => part == 1 ? Enumerable.Range(crabs.Min(), crabs.Max() - crabs.Min()).ToList().Min(x => Cost(x))
                         : Enumerable.Range(crabs.Min(), crabs.Max() - crabs.Min()).ToList().Min(x => IncrementalCost(x));

        public int Solve(int part = 1)
            => AlignCrabs(part);
    }
}
