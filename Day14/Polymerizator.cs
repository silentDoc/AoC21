using AoC21.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public long SolvePart2()
        {
            // Part 2 makes us realize we don't neet the actual string for anything
            var polymer = template;
            var pairs = polymer.Windowed(2).ToList();

            Dictionary<(char, char), long> pairToAmount = new();

            foreach (var pair in pairs)
            { 
                var el = new string(pair);

                if (!pairToAmount.ContainsKey((el[0], el[1])))
                    pairToAmount[(el[0], el[1])] = 0;
                
                pairToAmount[(el[0], el[1])]++;
            }

            HashSet<char> distinctLetters = new();
            for (int i = 0; i < 40; i++)
            {
                Dictionary<(char, char), long> newPairToAmount = new();

                foreach (var el in pairToAmount.Keys)
                {
                    var ruleKey = new string( [ el.Item1, el.Item2]);

                    var newElement1 = (el.Item1, rules[ruleKey]);
                    var newElement2 = (rules[ruleKey] , el.Item2);

                    distinctLetters.Add(el.Item1);
                    distinctLetters.Add(el.Item2);
                    distinctLetters.Add(rules[ruleKey]);

                    if (!newPairToAmount.ContainsKey(newElement1))
                        newPairToAmount[newElement1] = 0;
                    if (!newPairToAmount.ContainsKey(newElement2))
                        newPairToAmount[newElement2] = 0;

                    newPairToAmount[newElement1] += pairToAmount[el];
                    newPairToAmount[newElement2] += pairToAmount[el];
                }
                pairToAmount = newPairToAmount;
            }

            var letters = distinctLetters.ToList();
            Dictionary<char, long> appearances = new();

            foreach (var letter in letters)
            {
                var appsLetterAsFirst = pairToAmount.Keys.Where(x => x.Item1 == letter).ToList();
                var appsLetterAsSecond = pairToAmount.Keys.Where(x => x.Item1 == letter).ToList();

                var sumFirst = appsLetterAsFirst.Sum(x => pairToAmount[x]);
                var sumSecond = appsLetterAsSecond.Sum(x => pairToAmount[x]);

                appearances[letter] = Math.Max(sumFirst, sumSecond);
            }
            appearances[template.Last()]++;

            return appearances.Values.Max() - appearances.Values.Min();
        }

        public long Solve(int part)
            => part == 1 ? SolvePart1() : SolvePart2();
    }
}
