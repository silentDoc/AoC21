using AoC21.Common;
using System.Text;

namespace AoC21.Day20
{
    internal class ImageEnhancer
    {
        Dictionary<Coord2D, int> image = new();
        char[] algorithm = { };

        public void ParseInput(List<string> lines)
        {
            var sections = ParseUtils.SplitBy(lines, "");
            algorithm = sections[0][0].ToCharArray();

            foreach (var element in sections[1].Index())
                for (int i= 0; i < element.Item.Length; i++)
                    image[(i, element.Index)] = element.Item[i] == '#' ? 1 : 0;
        }

        void RunEnhancer(int round =0)
        {
            int minX = image.Keys.Min(k => k.x); int maxX = image.Keys.Max(k => k.x);
            int minY = image.Keys.Min(k => k.y); int maxY = image.Keys.Max(k => k.y);
            List<int> binaryWeights = [256, 128, 64, 32, 8, 4, 2, 1];
            var newImage = new Dictionary<Coord2D, int>();

             // The first element of the algorithm indicates if the empty space changes parity in each round
            var defaultValue = algorithm[0] == '#' ? round % 2 : 0;  

            for (int j = minY-1; j <= maxY+1; j++)
                for (int i = minX-1; i <= maxX+1; i++)
                {
                    Coord2D current = (i, j);
                    var neighs = current.GetNeighbors8().Select(x => image.GetValueOrDefault(x, defaultValue));
                    var position = neighs.Zip(binaryWeights, (x, y) => x * y).Sum();
                    position += image.GetValueOrDefault(current, defaultValue) * 16;   // Get neighbors doesn't contain our value
                    newImage[current] = algorithm[position] == '#' ? 1 :0; 
                }

            var newPixelPositions = image.Keys.Where(x => !newImage.ContainsKey(x)).ToList();
            newPixelPositions.ForEach(k => newImage[k] = image[k]);
            image = newImage;
        }

        void Display()
        {
            int minX = image.Keys.Min(k => k.x); int maxX = image.Keys.Max(k => k.x);
            int minY = image.Keys.Min(k => k.y); int maxY = image.Keys.Max(k => k.y);

            for (int j = minY; j <= maxY; j++)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = minX; i <= maxX; i++)
                    sb.Append(image[(i, j)]);
                Console.WriteLine(sb.ToString());
            }
            Console.WriteLine("");
        }

        int FindPixelsLit(int numTimes)
        {
            for (int i = 0; i < numTimes; i++)
                RunEnhancer(i);

           return image.Values.Count(x => x == 1);
        }

        public int Solve(int part = 1)
            => part == 1 ? FindPixelsLit(2) : FindPixelsLit(50);
    }
}
