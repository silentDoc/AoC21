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

        long SolveByDays(int rounds)
        {
            var daysToNums = new Dictionary<int, long>();

            daysToNums[0] = 0;
            daysToNums[1] = 0;
            daysToNums[2] = 0;
            daysToNums[3] = 0;
            daysToNums[4] = 0;
            daysToNums[5] = 0;
            daysToNums[6] = 0;
            daysToNums[7] = 0;
            daysToNums[8] = 0;
            daysToNums[9] = 0;

            foreach (var num in state)
                daysToNums[num]++;

            for (int round = 0; round < rounds; round++)
            {
                daysToNums[7] += daysToNums[0];
                daysToNums[9] += daysToNums[0];

                for(int i=1;i<=9;i++)
                    daysToNums[i-1] = daysToNums[i];

                daysToNums[9] = 0;
            }

            long retVal = 0;
            foreach(var key in daysToNums.Keys)
                retVal += daysToNums[key];
            
            return retVal;
        }

        public long Solve(int part = 0)
            => part == 1? SolvePart1(80) : SolveByDays(256);
    }
}
