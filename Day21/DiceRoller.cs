namespace AoC21.Day21
{
    static class DiracDice
    {
        public static IEnumerable<int> ThrowDice() => new[] { 1, 2, 3 }
                                                     .SelectMany(i => new[] { 1, 2, 3 }, (i, j) => new { i, j })
                                                     .SelectMany(pair => new[] { 1, 2, 3 }, (pair, k) => pair.i + pair.j + k);
    }

    static class D100Dice
    {
        public static IEnumerable<int> ThrowDice()
        {
                for (int i = 1;i<10000;i++)
                    yield return (i-1) % 100 + 1;
        }
    }

    class Player
    {
        public int Pos = 0;
        public int Score = 0;

        public Player(int pos, int score)
        { 
            Pos = pos;
            Score = score;
        }

        public Player Move(int steps)
        {
            var newPos = (Pos - 1 + steps) % 10 + 1;
            return new Player(newPos, Score + newPos);
        }
    }


    internal class DiceRoller
    {
        Player player1;
        Player player2;

        public void ParseInput(List<string> input)
        {
            int pos1 = 0; int pos2 = 0;
            pos1 = int.Parse(input[0].Replace("Player 1 starting position: ", ""));
            pos2 = int.Parse(input[1].Replace("Player 2 starting position: ", ""));

            player1 = new Player(pos1,0);
            player2 = new Player(pos2,0);

        }

        int PlayGame()
        {
            int rounds = 0;
            var steps = D100Dice.ThrowDice().Chunk(3).Select(x => x.Sum());

            foreach (var roll in steps)
            {
                rounds++;
                
                if(rounds % 2 == 1)
                    player1 = player1.Move(roll);
                else
                    player2 = player2.Move(roll);

                if (player1.Score >= 1000 || player2.Score >= 1000)
                    break;
            }

            var loserScore = (player1.Score >= 1000) ? player2.Score : player1.Score;
            return loserScore * rounds * 3; // Each round has 3 rolls
        }

        public int Solve(int part = 1)
            => PlayGame();
    }
}
