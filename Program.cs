using System.Diagnostics;

namespace AoC21
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int day = 16;
            int part = 1;
            bool test = false;
            int testNum = 6;

            string input = "./Input/day" + day.ToString("00");
            input += (test) ? "_test" + (testNum > 0 ? testNum.ToString() : "") + ".txt" : ".txt";

            Console.WriteLine("AoC 2021 - Day {0} , Part {1} - Test Data {2}", day, part, test);
            Stopwatch st = new();
            st.Start();
            string result = day switch
            {
                1 => day1(input, part),
                2 => day2(input, part),
                3 => day3(input, part),
                4 => day4(input, part),
                5 => day5(input, part),
                6 => day6(input, part),
                7 => day7(input, part),
                8 => day8(input, part),
                9 => day9(input, part),
                10 => day10(input, part),
                11 => day11(input, part),
                12 => day12(input, part),
                13 => day13(input, part),
                14 => day14(input, part),
                15 => day15(input, part),
                16 => day16(input, part),
                _ => throw new ArgumentException("Wrong day number - unimplemented")
            };
            st.Stop();
            Console.WriteLine("Result : {0}", result);
            Console.WriteLine("Elapsed : {0}", st.Elapsed.TotalSeconds);
        }

        static string day1(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day01.SonarSweep sweep = new();
            sweep.ParseInput(lines);
            return sweep.Solve(part).ToString();
        }

        static string day2(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day02.SubmarineNavi sub = new();
            sub.ParseInput(lines);
            return sub.Solve(part).ToString();
        }

        static string day3(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day03.SubDiagnostic diag = new();
            diag.ParseInput(lines);
            return diag.Solve(part).ToString();
        }

        static string day4(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day04.Bingo bingo = new();
            bingo.ParseInput(lines);
            return bingo.Solve(part).ToString();
        }

        static string day5(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day05.Liner liner = new();
            liner.ParseInput(lines);
            return liner.Solve(part).ToString();
        }

        static string day6(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day06.LanternFisher fisher = new();
            fisher.ParseInput(lines);
            return fisher.Solve(part).ToString();
        }

        static string day7(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day07.CrabAligner aligner = new();
            aligner.ParseInput(lines);
            return aligner.Solve(part).ToString();
        }

        static string day8(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day08.DigitPanel panel = new();
            panel.ParseInput(lines);
            return panel.Solve(part).ToString();
        }

        static string day9(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day09.SmokeBasin basin = new();
            basin.ParseInput(lines);
            return basin.Solve(part).ToString();
        }

        static string day10(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day10.SyntaxChecker daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day11(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day11.OcutpusNavi daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day12(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day12.CaveExplorer daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day13(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day13.OrigamiFolder daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day14(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day14.Polymerizator daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day15(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day15.CaveNavigator daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }

        static string day16(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day16.PacketAnalyzer daySolver = new();
            daySolver.ParseInput(lines);
            return daySolver.Solve(part).ToString();
        }
    }
}
