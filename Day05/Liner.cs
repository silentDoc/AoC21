using AoC20.Common;

namespace AoC21.Day05
{
    class Line
    {
        Coord2D start;
        Coord2D end;
        HashSet<Coord2D> points = new();

        public Line(string input)
        {
            var values = input.Split(" -> ").Select(x => x.Split(',')).SelectMany(x =>x).Select(int.Parse).ToList();
            start = (values[0], values[1]);
            end = (values[2], values[3]);
        }

        public bool VerticalOrHorizontal
            => (start.x == end.x || start.y == end.y);

        public HashSet<Coord2D> Points()
        {
            if (!VerticalOrHorizontal)
                return points;

            if (points.Count() > 0)
                return points;

            if (start.x == end.x)
            {
                var p0 = (start.y < end.y) ? start.y : end.y;
                var p1 = (start.y < end.y) ? end.y : start.y;

                for (int i = p0; i <= p1; i++)
                    points.Add((start.x, i));
            }
            else
            {
                var p0 = (start.x < end.x) ? start.x : end.x;
                var p1 = (start.x < end.x) ? end.x : start.x;

                for (int i = p0; i <= p1; i++)
                    points.Add((i, start.y));
            }
            
            return points;
        }
    }

    internal class Liner
    {
        List<Line> lines = new();
        public void ParseInput(List<string> input)
            => input.ForEach(x => lines.Add(new Line(x)));

        int SolvePart1()
        { 
            var hv = lines.Where(x => x.VerticalOrHorizontal).ToList();
            var allPoints = hv.SelectMany(x => x.Points());
            var pointDict = allPoints.GroupBy(x => x).Select(g => new { Point = g.Key, Amount = g.Count() })
                                     .ToDictionary(x => x.Point, x => x.Amount);

            return pointDict.Values.Count(x => x > 1);
        }

        public int Solve(int part = 1)
            => SolvePart1();
    }
}
