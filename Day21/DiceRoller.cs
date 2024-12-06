using System.Numerics;

namespace AoC21.Day21
{
    internal class DiceRoller
    {
        int p1Start = 0;  int p2Start = 0;
        int p1Score = 0;  int p2Score = 0;
        int diceResult = 0; int diceRolls = 0;

        public void ParseInput(List<string> input)
        {
            p1Start = int.Parse(input[0].Replace("Player 1 starting position: ", "")) -1;   // 0 based for simplicity
            p2Start = int.Parse(input[1].Replace("Player 2 starting position: ", "")) -1;
        }

        int Roll()
        {
            diceRolls++;
            var res = diceResult;
            diceResult = (diceResult + 1) % 100;
            return res + 1;
        }

        int PlayGame()
        {
            var p1Pos = p1Start;
            var p2Pos = p2Start;

            while (p1Score < 1000 && p2Score < 1000)
            {
                var p1Move = Roll() + Roll() + Roll();
                p1Pos = (p1Pos + p1Move) % 10;
                p1Score = p1Score + (p1Pos + 1);

                if (p1Score >= 1000)
                    break;
                
                var p2Move = Roll() + Roll() + Roll();
                p2Pos = (p2Pos + p2Move) % 10;
                p2Score = p2Score + (p2Pos + 1);
            }

            var loserScore = (p1Score >= 1000) ? p2Score : p1Score;
            return loserScore * diceRolls;
        }

        public int Solve(int part = 1)
            => PlayGame();
    }
}
