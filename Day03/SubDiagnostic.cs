using System.Text;

namespace AoC21.Day03
{
    internal class SubDiagnostic
    {
        List<string> report = new();

        public void ParseInput(List<string> input)
            => report = input;

        int SolvePart1()
        {
            var ones = new List<int>();
            var zeros = new List<int>();
            
            StringBuilder sbGamma = new StringBuilder();
            StringBuilder sbEpsi = new StringBuilder();

            for (int i = 0; i < report[0].Length; i++)
            {
                ones.Add(report.Select(x => x[i]).Count(c => c == '1'));
                zeros.Add(report.Count - ones[i]);
                sbGamma.Append(ones[i] > zeros[i] ? '1' : '0');
                sbEpsi.Append(ones[i] < zeros[i] ? '1' : '0');
            }

            var gamma = Convert.ToInt32(sbGamma.ToString(), 2);
            var epsi = Convert.ToInt32(sbEpsi.ToString(), 2);

            return gamma * epsi;
        }

        int MostCommon(List<string> collection, int pos)
        {
            var numOnes = collection.Select(r => r[pos]).Count(x => x == '1');
            var numZeros = collection.Count - numOnes;

            return numZeros == numOnes ? 2 : numOnes>numZeros ? 1 : 0;
        }
       
        int SolvePart2()
        {
            List<string> Oxy = new();
            List<string> Co2 = new();

            report.ForEach(Oxy.Add);
            report.ForEach(Co2.Add);

            for (int i = 0; i < report[0].Length; i++)
            {
                if (Oxy.Count() == 1)
                    break;

                var commonBit = MostCommon(Oxy, i);
                Oxy = Oxy.Where(x => x[i] == (commonBit == 0 ? '0' : '1')).ToList();
            }
            
            for (int i = 0; i < report[0].Length; i++)
            {
                if (Co2.Count() == 1)
                    break;

                var commonBit = MostCommon(Co2, i);
                Co2 = Co2.Where(x => x[i] == (commonBit == 0 ? '1' : '0')).ToList();
            }

            var OxyRating = Convert.ToInt32(Oxy[0].ToString(), 2);
            var Co2Rating = Convert.ToInt32(Co2[0].ToString(), 2);

            return OxyRating * Co2Rating;
        }

        public int Solve(int part = 1)
            => part == 1 ? SolvePart1(): SolvePart2();
    }
}
