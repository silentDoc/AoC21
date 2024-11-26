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

        public int Solve(int part = 1)
            => SolvePart1();
    }
}
