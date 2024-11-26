using AoC20.Common;

namespace AoC21.Day04
{
    class Board
    {
        Dictionary<Coord2D, int> numbers = new();
        List<int> drawnNumbers = new();

        List<List<Coord2D>> lines = [ [(0, 0), (1, 0), (2, 0), (3, 0), (4, 0)], [(0, 1), (1, 1), (2, 1), (3, 1), (4, 1)], [(0, 2), (1, 2), (2, 2), (3, 2), (4, 2)],
                                         [(0, 3), (1, 3), (2, 3), (3, 3), (4, 3)], [(0, 4), (1, 4), (2, 4), (3, 4), (4, 4)] ];

        List<List<Coord2D>> columns = [ [(0,0) , (0, 1), (0, 2), (0, 3), (0, 4)], [(1, 0), (1, 1), (1, 2), (1, 3), (1, 4)], [(2, 0), (2, 1), (2, 2), (2, 3), (2, 4)],
                                           [(3,0) , (3, 1), (3, 2), (3, 3), (3, 4)],[(4,0) , (4, 1), (4, 2), (4, 3), (4, 4)]];

        public Board(List<string> input)
        {
            for (int j = 0; j < input.Count; j++)
            {
                var line = input[j].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                for (int i = 0; i < line.Count; i++)
                    numbers[(i, j)] = line[i];
            }
        }

        public void Receive(int number)
            => drawnNumbers.Add(number);

        public bool Winner()
            => lines.Any(x => x.All(k => drawnNumbers.Contains(numbers[k]))) || columns.Any(x => x.All(k => drawnNumbers.Contains(numbers[k])));

        public int Score
            => numbers.Values.Where(x => !drawnNumbers.Contains(x)).Sum() * drawnNumbers.Last();
    }

    internal class Bingo
    {
        List<int> numbers = new();
        List<Board> boards = new();

        public void ParseInput(List<string> input)
        {
            var sections = ParseUtils.SplitBy(input, "");
            numbers = sections[0][0].Split(",").Select(int.Parse).ToList();

            foreach (var item in sections.Skip(1))
                boards.Add(new Board(item));
        }

        int FindWinner()
        {
            foreach (var number in numbers)
            {
                boards.ForEach(b => b.Receive(number));
                var win = boards.FirstOrDefault(x => x.Winner());

                if (win != null)
                    return win.Score;
            }
            return -1;
        }

        public int Solve(int part = 1)
            => FindWinner();
    }
}
