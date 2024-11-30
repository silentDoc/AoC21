using AoC21.Common;
using System.Text;

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

        public string Step(string input)
        {
            var list = input.ToList();
            var pairs = list.Windowed(2).ToList();

            StringBuilder sb = new();
            sb.Append(pairs[0][0]);
            foreach (var pair in pairs)
            {
                var element = new string(pair);
                
                if (rules.ContainsKey(element))
                    sb.Append(rules[element]);
                sb.Append(pair[1]);
            }
            return sb.ToString();
        }

        public int SolvePart1()
        {
            var polymer = template;

            for (int i = 0; i < 10; i++)
                polymer = Step(polymer);

            var letters = polymer.Distinct().ToList();
            var appearances = letters.Select(x => polymer.Count(c => c == x));

            return appearances.Max() - appearances.Min();
        }

        public int Solve(int part)
            => SolvePart1();
    }
}
