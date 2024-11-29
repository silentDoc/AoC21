namespace AoC21.Day10
{
    internal class SyntaxChecker
    {
        List<string> lines = [];
        HashSet<char> opening = [ '(', '[', '{', '<' ];

        public void ParseInput(List<string> input)
            => lines = input;

        bool isMatch(char open, char close)
            => (open, close) switch
            {
                ('(',')') => true,
                ('{', '}') => true,
                ('[', ']') => true,
                ('<', '>') => true,
                _ => false
            };
       
        int charScore(char delimiter)
            => delimiter switch
                {
                    ')' => 3,
                    ']' => 57,
                    '}' => 1197,
                    '>' => 25137,
                    _ => throw new Exception("Unknown delimiter " + delimiter.ToString())
                };

        int closerScore(char delimiter)
            => delimiter switch
            {
                '(' => 1,
                '[' => 2,
                '{' => 3,
                '<' => 4,
                _ => throw new Exception("Unknown delimiter " + delimiter.ToString())
            };

        int GetScore(string line)
        { 
            Stack<char> stack = new();

            foreach (var delimiter in line)
            {
                if (opening.Contains(delimiter))
                {
                    stack.Push(delimiter);
                    continue;
                }

                var c = stack.Pop();
                if (isMatch(c, delimiter))
                    continue;

                return charScore(delimiter);
            }
            return 0;
        }

        long CompleteLine(string line)
        {
            Stack<char> stack = new();

            foreach (var delimiter in line)
                if (opening.Contains(delimiter))
                    stack.Push(delimiter);
                else
                    stack.Pop();

            long score = 0;
            while (stack.Count > 0)
                score = score*5 + closerScore(stack.Pop());

            return score;
        }

        long AutocompleteScore()
        {
            var sortedScores = lines.Where(x => GetScore(x) == 0)
                                    .Select(l => CompleteLine(l))
                                    .OrderBy(x => x);

            return sortedScores.Skip(sortedScores.Count() / 2).First();
        }

        public long Solve(int part = 1)
            => part == 1 ? lines.Sum(x => GetScore(x)) : AutocompleteScore();
    }
}
