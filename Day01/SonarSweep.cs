using AoC20.Common;

namespace AoC21.Day01
{
    internal class SonarSweep
    {
        List<int> readings = new();
        public void ParseInput(List<string> input)
            => readings = input.Select(int.Parse).ToList();

        int SolvePart2()
        {
            var windows = readings.Windowed(3).Select(x => x.Sum()).ToList();   // Common - Listutils
            return windows.Zip(windows.Skip(1), (x, y) => y - x).Count(r => r > 0);
        }

        public int Solve(int part = 1)
            => part == 1 ? readings.Zip(readings.Skip(1), (x,y) => y-x).Count(r => r > 0)
                         : SolvePart2(); 
    }
}
