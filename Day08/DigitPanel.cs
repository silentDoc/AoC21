namespace AoC21.Day08
{
    class SignalTest
    { 
        public List<string> inputSignals = new List<string>();
        public List<string> outputValues = new List<string>();

        public SignalTest(string input)
        { 
            var parts = input.Split(" | ");
            var temp = parts[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
            inputSignals = temp.Select(x => string.Concat(x.OrderBy(x => x))).ToList();
            temp = parts[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
            outputValues = temp.Select(x => string.Concat(x.OrderBy(x => x))).ToList();
        }

        public int Value()
        {
            // Work out the connections
            var dict  = new Dictionary<string, int>();
            var one   = inputSignals.Single(x => x.Length == 2);
            dict[one] = 1;
            var seven = inputSignals.Single(x => x.Length == 3);
            dict[seven] = 7;
            var four  = inputSignals.Single(x => x.Length == 4);
            dict[four] = 4;
            var eight = inputSignals.Single(x => x.Length == 7);
            dict[eight] = 8;
            var three = inputSignals.Single(x => x.Length == 5 && x.Intersect(seven).Count() == 3); 
            dict[three] = 3;
            var two   = inputSignals.Single(x => x.Length == 5 && x.Intersect(four).Count() == 2);
            dict[two] = 2;
            var nine = inputSignals.Single(x => x.Length == 6 && x.Intersect(three).Count() == 5);
            dict[nine] = 9;
            var five = inputSignals.Single(x => x.Length == 5 && !dict.ContainsKey(x));
            dict[five] = 5;
            var six   = inputSignals.Single(x => x.Length == 6 && x.Intersect(one).Count() != 2);
            dict[six] = 6;
            var zero  = inputSignals.Single(x => !dict.ContainsKey(x));
            dict[zero] = 0;

            string retStr = "";
            foreach(var digit in outputValues)
                retStr += dict[digit].ToString();

            return int.Parse(retStr);
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
            => part == 1 ? SolvePart1() : signals.Sum(x => x.Value());
    }
}
