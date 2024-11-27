namespace AoC21.Day07
{
    internal class CrabAligner
    {
        List<int> crabs = new();
        public void ParseInput(List<string> input)
            => crabs = input[0].Split(",").Select(int.Parse).ToList();

        int Cost(int pos)
            => crabs.Select(x => Math.Abs(x - pos)).Sum();

        int AlignCrabs()
            => Enumerable.Range(crabs.Min(), crabs.Max() - crabs.Min()).ToList().Min(x => Cost(x));

        public int Solve(int part = 0)
            => AlignCrabs();
    }
}
