namespace AoC21.Day06
{
    internal class LanternFisher
    {
        List<int> state = new();
        public void ParseInput(List<string> input)
            => state = input[0].Split(",").Select(int.Parse).ToList();

        long FindFishes(int rounds)
        {
            var dayNumToAmounts = new Dictionary<int, long>();

            for(int i=0;i<10;i++)
                dayNumToAmounts[i] = 0;

            foreach (var num in state)
                dayNumToAmounts[num]++;

            for (int round = 0; round < rounds; round++)
            {
                dayNumToAmounts[7] += dayNumToAmounts[0];
                dayNumToAmounts[9] += dayNumToAmounts[0];

                for(int i=1;i<=9;i++)
                    dayNumToAmounts[i-1] = dayNumToAmounts[i];

                dayNumToAmounts[9] = 0;
            }

            long retVal = 0;
            foreach(var key in dayNumToAmounts.Keys)
                retVal += dayNumToAmounts[key];
            
            return retVal;
        }

        public long Solve(int part = 0)
            => part == 1? FindFishes(80) : FindFishes(256);
    }
}
