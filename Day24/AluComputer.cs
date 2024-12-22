using AoC21.Common;
using System.Net;

namespace AoC21.Day24
{
    record Instruction(string command, string param1, string param2);

    internal class AluComputer
    {
        List<Instruction> sourceCode = new();
        Dictionary<string, int> vars = new();
        Queue<int> inputs = new();
        Instruction ParseLine(string line)
        {
            var g = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return new(g[0], g[1], g.Count() == 3 ? g[2] : "");
        }

        public void ParseInput(List<string> lines)
           => lines.ForEach(x => sourceCode.Add(ParseLine(x)));

        // This will not work, obviously - left here for the laughs
        void ExecuteIns(Instruction ins)
        {
            int param2Value = 0;
            if (ins.param2 != "")
                param2Value = (char.IsDigit(ins.param2[0]) || ins.param2[0] == '-') ? int.Parse(ins.param2) : vars[ins.param2];
          
            switch (ins.command)
            {
                case "inp":
                    vars[ins.param1] = inputs.Dequeue();
                    break;
                case "add":
                    vars[ins.param1] = vars[ins.param1] + param2Value;
                    break;
                case "mul":
                    vars[ins.param1] = vars[ins.param1] * param2Value;
                    break;
                case "div":
                    vars[ins.param1] = vars[ins.param1] / param2Value;
                    break;
                case "mod":
                    vars[ins.param1] = MathHelper.Modulo(vars[ins.param1],param2Value);
                    break;
                case "eql":
                    vars[ins.param1] = (vars[ins.param1] == param2Value) ? 1 : 0;   
                    break;
            }
        }

        int RunProgram()
        {
            foreach (var ins in sourceCode)
                ExecuteIns(ins);
            return vars["z"];
        }

        public long SolvePart1()
        {
            for (long i = 99999999999999; i >= 11111111111111; i--)
            {
                vars = new();
                vars["w"] = 0;
                vars["x"] = 0;
                vars["y"] = 0;
                vars["z"] = 0;

                inputs = new();
                var str = i.ToString();
                if (str.IndexOf('0') != -1)
                    continue;

                var digits = str.Select(x => int.Parse(x.ToString())).ToList();
                foreach (var digit in digits)
                    inputs.Enqueue(digit);

                if (RunProgram() == 0)
                    return i;
            }
            return 0;
        }


        public long Solve(int part = 1)
            => SolvePart1();
    }
}
