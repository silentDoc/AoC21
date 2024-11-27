namespace AoC21.Day06
{
    internal class LanternFisher
    {
        List<int> state = new();
        public void ParseInput(List<string> input)
            => state = input[0].Split(",").Select(int.Parse).ToList();

        int SolvePart1(int rounds)
        {
            for (int i = 0; i < rounds; i++)
            {
                var toAdd = 0;
                
                for (int j = 0; j < state.Count; j++)
                    if (state[j] == 0)
                    {
                        toAdd++;
                        state[j] = 6;
                    }
                    else
                        state[j] = state[j] - 1;

                for (int j = 0; j < toAdd; j++)
                    state.Add(8);
            }
            return state.Count();
        }

        public int Solve(int part = 0)
            => SolvePart1(80);
    }
}
