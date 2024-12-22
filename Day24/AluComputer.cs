namespace AoC21.Day24
{
    record Instruction(string command, string param1, string param2);

    internal class AluComputer
    {
        List<Instruction> sourceCode = new();

        Instruction ParseLine(string line)
        {
            var g = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return new(g[0], g[1], g.Count() == 3 ? g[2] : "");
        }

        public void ParseInput(List<string> lines)
           => lines.ForEach(x => sourceCode.Add(ParseLine(x)));

        long HackAlu(int part = 1)
        {
            var digits = Enumerable.Range(1, 9).ToList();

            // Divide the instruction list into chunks of 18 lines
            var blocks = sourceCode.Chunk(18).ToList();
            var stack = new Stack<int>();

            Dictionary<int, int> maxDigits = new();
            Dictionary<int, int> minDigits = new();

            foreach (var k in Enumerable.Range(0, 14))
            {
                maxDigits[k] = -1;
                minDigits[k] = 100000;
            }

            for (var j = 0; j < 14; j++)
            {
                if (blocks[j][4].param2 == "1")     // Chunk with "div z 1" instead of "div z 26"
                    stack.Push(j);
                else
                {
                    var i = stack.Pop();
                    var increment = int.Parse(blocks[j][5].param2) + int.Parse(blocks[i][15].param2);

                    foreach (var a in digits)
                    {
                        var b = a + increment;

                        if (digits.Contains(b))
                        {
                            if (a > maxDigits[i])
                            {
                                maxDigits[i] = a;
                                maxDigits[j] = b;
                            }

                            if (a < minDigits[i])
                            {
                                minDigits[i] = a;
                                minDigits[j] = b;
                            }

                        }
                    }

                }
            }

            long res = 0;

            var digitDict = part == 1 ? maxDigits : minDigits;

            foreach (var k in Enumerable.Range(0, 14))
                res = res * 10 + digitDict[k];

            return res;
        }

        public long Solve(int part = 1)
            => HackAlu(part);
    }
}
