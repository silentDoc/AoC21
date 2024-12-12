using System.Text.RegularExpressions;

namespace AoC21.Day22
{
    // Refactored to take the approach of encse - much cleaner than my previous version of Cuboid Range
    record Dim(long start, long end)
    {
        public bool IsEmpty => start > end;
        public long Length => IsEmpty ? 0 : end - start + 1;

        public Dim Intersect(Dim other) =>
            new Dim(Math.Max(start, other.start), Math.Min(end, other.end));
    }

    record Cuboid(Dim x, Dim y, Dim z)
    {
        public bool IsEmpty => x.IsEmpty || y.IsEmpty || z.IsEmpty;
        public long Volume => x.Length * y.Length * z.Length;

        public Cuboid Intersect(Cuboid other)
            => new Cuboid(x.Intersect(other.x), y.Intersect(other.y), z.Intersect(other.z));
    }

    internal class CuboidRebooter
    {
        Regex regex = new(@"^(on|off) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)");
        List<(bool turnOn, Cuboid cuboid)> cuboids = new();

        public void ParseInput(List<string> input)
            => input.ForEach(ParseLine);

        void ParseLine(string line)
        {
            var groups = regex.Match(line).Groups;
            var action = groups[1].Value == "on" ? true : false;
            var vals = groups.Cast<Group>().Skip(2).Select(g => int.Parse(g.Value)).ToList();

            var dimX = new Dim(Math.Min(vals[0], vals[1]), Math.Max(vals[0], vals[1]));
            var dimY = new Dim(Math.Min(vals[2], vals[3]), Math.Max(vals[2], vals[3]));
            var dimZ = new Dim(Math.Min(vals[4], vals[5]), Math.Max(vals[4], vals[5]));

            cuboids.Add((action, new Cuboid(dimX, dimY, dimZ)));
        }

        long LitAfterCuboid(int index, Cuboid region)
        {
            if (region.IsEmpty || index < 0)
                return 0;

            var intersection = region.Intersect(cuboids[index].cuboid);
            var litInRegion = LitAfterCuboid(index - 1, region);
            var litInIntersect = LitAfterCuboid(index - 1, intersection);
            var litOutIntersect = litInRegion - litInIntersect;

            return cuboids[index].turnOn ? litOutIntersect + intersection.Volume : litOutIntersect;
        }

        public long FindActiveInCuboid(int cubRange)
        {
            var cuboid = new Cuboid(new Dim(-cubRange, cubRange), new Dim(-cubRange, cubRange), new Dim(-cubRange, cubRange));
            return LitAfterCuboid(cuboids.Count-1, cuboid);
        }

        public long Solve(int part = 1)
            => part == 1 ? FindActiveInCuboid(50) : FindActiveInCuboid(1000000);
    }
}
