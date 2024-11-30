using AoC21.Common;

namespace AoC21.Day13
{
    internal class OrigamiFolder
    {
        HashSet<Coord2D> dots = new();
        List<(char dir, int value)> folds = new();

        public void ParseInput(List<string> lines)
        {
            var sections = ParseUtils.SplitBy(lines, "");

            foreach (var line in sections[0])
            {
                var values = line.Split(',').Select(int.Parse).ToList();
                dots.Add(new(values[0], values[1]));
            }

            foreach (var line in sections[1])
            {
                var parts = line.Replace("fold along ", "").Split('=');
                folds.Add((parts[0][0], int.Parse(parts[1])) );
            }
        }

        HashSet<Coord2D> FoldHorizontal(HashSet<Coord2D> dots, int row)
        {
            HashSet<Coord2D> newOrigami = new();

            dots.Where(d => d.y < row).ToList().ForEach(d => newOrigami.Add(d));

            var foldedDots = dots.Where(d => d.y > row).ToList();
            foreach (var dot in foldedDots)
            { 
                var dif = dot.y - row;
                newOrigami.Add(new(dot.x, row - dif));
            }
            return newOrigami;
        }

        HashSet<Coord2D> FoldVertical(HashSet<Coord2D> dots, int column)
        {
            HashSet<Coord2D> newOrigami = new();

            dots.Where(d => d.x < column).ToList().ForEach(d => newOrigami.Add(d));

            var foldedDots = dots.Where(d => d.x > column).ToList();
            foreach (var dot in foldedDots)
            {
                var dif = dot.x - column;
                newOrigami.Add(new(column - dif, dot.y));
            }
            return newOrigami;
        }

        int DoFoldings(int part = 1)
        {
            var origami = dots;

            foreach (var fold in folds)
            {
                origami = (fold.dir == 'x') ? FoldVertical(origami, fold.value) : FoldHorizontal(origami, fold.value);
                if (part == 1)
                    break;
            }

            return origami.Count;
        }

        public int Solve(int part = 1)
            => DoFoldings(part);
    }
}
