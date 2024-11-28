namespace AoC21.Day08
{
    class SignalTest
    { 
        public List<string> inputSignals = new List<string>();
        public List<string> outputValues = new List<string>();

        public SignalTest(string input)
        { 
            var parts = input.Split(" | ");
            inputSignals = parts[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
            outputValues = parts[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }

    internal class DigitPanel
    {
        List<SignalTest> signals = new();
        public void ParseInput(List<string> input)
            => input.ForEach(x => signals.Add(new(x)));

        int SolvePart1()
        {
            Dictionary<int, int> numToAppearances = new();
            for (int i = 0; i < 10; i++)
                numToAppearances[i] = 0;

            foreach (var signal in signals)
            {
                numToAppearances[1] += signal.outputValues.Count(x => x.Length == 2);
                numToAppearances[7] += signal.outputValues.Count(x => x.Length == 3);
                numToAppearances[4] += signal.outputValues.Count(x => x.Length == 4);
                numToAppearances[8] += signal.outputValues.Count(x => x.Length == 7);
            }

            return numToAppearances.Values.Sum();
        }


        public int Solve(int part = 1)
            => SolvePart1();
    }
}
