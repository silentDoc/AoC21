using AoC20.Common;

namespace AoC21.Day02
{
    class Command
    {
        public string What;
        public int Amount;

        public Command(string inputLine)
        {
            var parts = inputLine.Split(' ');
            What = parts[0];
            Amount = int.Parse(parts[1]);
        }

        public Coord2D Do(Coord2D current)
            => What switch
            {
                "forward" => (current.x + Amount, current.y),
                "down" => (current.x, current.y + Amount),
                "up" => (current.x, current.y - Amount),
                _ => throw new Exception("Invalid command " + What)
            };

        public (Coord2D, int) DoAim(Coord2D current, int aimCurrent)
        => What switch
            {
                "forward" =>( (current.x + Amount, current.y + aimCurrent*Amount) , aimCurrent),
                "down" => ( (current.x, current.y) , aimCurrent + Amount),
                "up" => ((current.x, current.y), aimCurrent - Amount),
                _ => throw new Exception("Invalid command " + What)
            };
    }

    internal class SubmarineNavi
    {
        List<Command> commands = new();

        public void ParseInput(List<string> input)
            => input.ForEach(x => commands.Add(new(x)));


        int SolvePart1()
        {
            Coord2D pos = (0, 0);
            foreach (var command in commands)
                pos = command.Do(pos);
            
            return pos.x * pos.y;
        }

        int SolvePart2()
        {
            Coord2D pos = (0, 0);
            int aim = 0;
            foreach (var command in commands)
                (pos, aim) = command.DoAim(pos, aim);

            return pos.x * pos.y;
        }

        public int Solve(int part = 1)
            => part == 1 ? SolvePart1() : SolvePart2();
    }
}
