using AoC21.Common;

namespace AoC21.Day05
{
    class Line
    {
        Coord2D start;
        Coord2D end;
        HashSet<Coord2D> points = new();

        public Line(string input)
        {
            var values = input.Replace(" -> ", ",").Split(",").Select(int.Parse).ToList();
            start = (values[0], values[1]);
            end = (values[2], values[3]);
        }

        public bool VerticalOrHorizontal
            => (start.x == end.x || start.y == end.y);

        public HashSet<Coord2D> Points()
        {
            if (points.Any())
                return points;

            var direction = end - start;
            direction = direction / (direction.x == 0 ? Math.Abs(direction.y) : Math.Abs(direction.x));

            var current = start;
            points.Add(current);
            while (current != end)
            {
                current += direction;
                points.Add(current);
            }
            
            return points;
        }
    }

    internal class Liner
    {
        List<Line> lines = new();
        public void ParseInput(List<string> input)
            => input.ForEach(x => lines.Add(new Line(x)));

        int FindOverlap(int part =1)
        { 
            var lineSet = part == 1 ? lines.Where(x => x.VerticalOrHorizontal).ToList() : lines;
            var allPoints = lineSet.SelectMany(x => x.Points());
            var pointDict = allPoints.GroupBy(x => x).Select(g => new { Point = g.Key, Amount = g.Count() })
                                     .ToDictionary(x => x.Point, x => x.Amount);

            return pointDict.Values.Count(x => x > 1);
        }

        public int Solve(int part = 1)
            => FindOverlap(part);
    }
}
