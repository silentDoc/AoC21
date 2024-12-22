using AoC21.Common;

namespace AoC21.Day25
{
    internal class CucumberNavi
    {
        Dictionary<Coord2D, char> map = new();
        void ParseLine(int row, string line)
            => line.Index().ToList().ForEach(x => map[(x.Index, row)] = x.Item);

        public void ParseInput(List<string> input)
            => input.Index().ToList().ForEach(x => ParseLine(x.Index, x.Item));

        Coord2D nextEast(Coord2D pos, int maxX)
            => ((pos.x + 1) % maxX, pos.y);

        Coord2D nextSouth(Coord2D pos, int maxY)
            => (pos.x,  (pos.y + 1) % maxY);

        int FindStableStep()
        {
            int maxX = map.Keys.Max(k => k.x);
            int maxY = map.Keys.Max(k => k.y);

            HashSet<Coord2D> east  = map.Keys.Where(k => map[k] == '>').ToHashSet();
            HashSet<Coord2D> south = map.Keys.Where(k => map[k] == 'v').ToHashSet();

            var stable = false;
            int step = 0;
            while (!stable)
            {
                stable = true;
                step++;
                HashSet<Coord2D> newEast = new();
                foreach (var e in east)
                {
                    var next = nextEast(e, maxX+1);
                    var stop = east.Contains(next) || south.Contains(next);

                    newEast.Add( stop ? e : next);
                    if (!stop)
                        stable = false;
                    
                }
                HashSet<Coord2D> newSouth = new();
                foreach (var s in south)
                {
                    var next = nextSouth(s, maxY+1);
                    var stop = newEast.Contains(next) || south.Contains(next);

                    newSouth.Add(stop ? s : next);
                    if (!stop)
                        stable = false;

                }

                east = newEast;
                south = newSouth;
            }
            return step;
        }

        public int Solve(int part = 1)
            => FindStableStep();
    }
}
