namespace AoC21.Day21
{
    static class DiracDice
    {
        public static IEnumerable<int> ThrowDice() 
            => from i in new[] { 1, 2, 3 }
               from j in new[] { 1, 2, 3 }
               from k in new[] { 1, 2, 3 }
               select i + j + k;
    }

    static class D100Dice
    {
        public static IEnumerable<int> ThrowDice()
        {
                for (int i = 1;i<10000;i++)
                    yield return (i-1) % 100 + 1;
        }
    }

    record Player(int Pos, int Score)
    {
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
        new Dictionary<(Player, Player), (long, long)> memoize = new(); // for part 2

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

        (long activeWins, long otherWins) WinCounter( (Player active, Player other) players)
        {
            if (players.other.Score >= 21)
                return (0, 1);

            if (!memoize.ContainsKey(players))
            {
                var (activeWins, otherWins) = (0L, 0L);
                foreach (var steps in DiracDice.ThrowDice())
                {
                    // We recurse switching active and other - so we address it at the sum below
                    var wins = WinCounter((players.other, players.active.Move(steps)));
                    activeWins += wins.otherWins;   
                    otherWins += wins.activeWins;
                }
                memoize[players] = (activeWins, otherWins);
            }
            return memoize[players];
        }

        long PlayGameDirac()
        {
            var gamesWon = WinCounter((player1, player2));
            return Math.Max(gamesWon.activeWins, gamesWon.otherWins);
        }

        public long Solve(int part = 1)
            => part == 1 ? PlayGame() : PlayGameDirac();
    }
}
