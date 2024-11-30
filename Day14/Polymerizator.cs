using AoC21.Common;

namespace AoC21.Day14
{
    internal class Polymerizator
    {
        Dictionary<string, char> rules = new();
        string template = "";

        public void ParseInput(List<string> lines)
        {
            var sections = ParseUtils.SplitBy(lines, "");
            template = sections[0][0];

            foreach (var entry in sections[1])
            { 
                var values = entry.Split(" -> ");
                rules[values[0]] = values[1].First();
            }
        }

        public long FindResult(int steps)
        {
            // Part 2 makes us realize we don't neet the actual string for anything
            // refer to older commits to see original version of part 1
            var polymer = template;
            var pairs = polymer.Windowed(2).ToList();

            Dictionary<string, long> pairToAmount = new();

            foreach (var pair in pairs)
            {
                var element = new string(pair);

                if (!pairToAmount.ContainsKey(element))
                    pairToAmount[element] = 0;

                pairToAmount[element]++;
            }

            for (int i = 0; i < steps; i++)
            {
                Dictionary<string, long> newPairToAmount = new();

                foreach (var el in pairToAmount.Keys)
                {
                    var inserted = rules[el];
                    var newPair1 = new string([el[0], inserted]);
                    var newPair2 = new string([inserted, el[1]]);

                    if (!newPairToAmount.ContainsKey(newPair1))
                        newPairToAmount[newPair1] = 0;
                    if (!newPairToAmount.ContainsKey(newPair2))
                        newPairToAmount[newPair2] = 0;

                    newPairToAmount[newPair1] += pairToAmount[el];
                    newPairToAmount[newPair2] += pairToAmount[el];
                }
                pairToAmount = newPairToAmount;
            }

            var letters = pairToAmount.Keys.SelectMany(x => x).Distinct().ToList();
            Dictionary<char, long> appearances = new();

            foreach (var letter in letters)
            {
                // We cannot use "Contains" because a key such as "NN" would only be counting only one appearance for the N
                var keysWithLetterFirst = pairToAmount.Keys.Where(x => x[0] == letter).ToList();
                var keysWithLetterSecond = pairToAmount.Keys.Where(x => x[1] == letter).ToList();

                // Max is because the ending char has no starting sequence
                appearances[letter] = Math.Max( keysWithLetterFirst.Sum(x => pairToAmount[x]), 
                                                keysWithLetterSecond.Sum(x => pairToAmount[x])) ;
            }

            return appearances.Values.Max() - appearances.Values.Min();
        }

        public long Solve(int part)
            => FindResult(part == 1 ? 10 : 40);
    }
}
