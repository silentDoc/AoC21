using AoC21.Common;

namespace AoC21.Day17
{
    class Area
    {
        public Coord2D TopLeft = (0, 0);
        public Coord2D BottomRight = (0, 0);

        public Area(int x0, int y0, int x1, int y1)
        { 
            TopLeft = new Coord2D(x0, y0);
            BottomRight = new Coord2D(x1, y1);
        }

        public bool IsInArea(Coord2D point)
            => point.x >= TopLeft.x && point.x <= BottomRight.x &&
               point.y <= TopLeft.y && point.y >= BottomRight.y;
    }

    internal class ProbeLauncher
    {
        Area area = new(0, 0, 0, 0);
        public void ParseInput(List<string> input)
        {
            var str = input[0].Replace("target area: x=", "").Replace(" y=", "");
            var nums = str.Split(',').Select(x => x.Split("..")).SelectMany(x => x).Select(int.Parse).ToList();
            area = new(nums[0], nums[3], nums[1], nums[2]);
        }

        int SimLaunch(Coord2D velocity, int steps)
        {
            Coord2D pos = new(0, 0);
            Coord2D vel = velocity;

            int maxY = int.MinValue;
            bool hit = false;

            for(int i=0; i<steps; i++)
            {
                pos += vel;

                if (pos.x > area.BottomRight.x)
                    break;

                vel.x += vel.x > 0 ? -1 : vel.x < 0 ? 1 : 0;
                vel.y--;

                if (area.IsInArea(pos))
                    hit = true;

                if (pos.y > maxY)
                    maxY = pos.y;
            }
            return hit ? maxY : int.MinValue;
        }

        int FindTrajectory(int part = 1)
        {
            // Bruteforce ftw
            int maxHeight = -1;
            int numSetups = 0;

            for (int x = 0; x < 500; x++)
                for (int y = -500; y < 500; y++)
                {
                    var height = SimLaunch(new Coord2D(x, y), 500);
                    if (height > int.MinValue)
                        numSetups++;

                    if (height > maxHeight)
                        maxHeight = height;
                }
            
            return part == 1 ? maxHeight : numSetups;
        }


        public int Solve(int part = 1)
            => FindTrajectory(part);
    }
}
