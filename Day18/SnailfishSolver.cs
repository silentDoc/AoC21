#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8629 // Un tipo que acepta valores NULL puede ser nulo.

using AoC21.Common;


namespace AoC21.Day18
{
    class SnailNode
    {
        public int? NumericValue;
        public List<SnailNode> Children = new();
        public SnailNode? Parent = null;
        //public int Depth = 0;             // Stuck two days trying to parse the depth and then managing the changes, until realized it's better to add a method to do so

        public int Depth()
        {
            var depth = 0;
            var temp = this.Parent;

            while (temp != null)
            {
                temp = temp.Parent;
                depth++;
            }

            return depth;
        }

        public SnailNode? Left
            => Children.Count == 0 ? null : Children[0];
        public SnailNode? Right
            => Children.Count == 0 ? null : Children[1];

        public override string ToString()
        {
            var result = "";

            if (Children.Count == 0)
                return NumericValue.ToString();

            if (Children.Count == 2)
                result += "[";

            result += Children.First().ToString();
            result += ",";
            result += Children.Last().ToString();
            result += "]";

            return result;
        }
    }

    class SnailNum
    {
        public SnailNode? root = null;
        public List<SnailNode> Numbers = new();

        public SnailNum()
        { }

        public SnailNum(string inputLine)
        { 
            var stack = new Stack<SnailNode>();
            List<char> digits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

            for (int i = 0; i < inputLine.Length; i++)
            {
                var currChar = inputLine[i];
                var parent = stack.Any() ? stack.Peek() : null;

                if (currChar == ',')
                    continue;

                if (currChar == '[')
                {
                    var node = new SnailNode();

                    if (parent != null)
                    {
                        parent.Children.Add(node);
                        node.Parent = parent;
                    }
                    else
                        root = node;
                    stack.Push(node);
                }
                else if (digits.Contains(currChar))
                {
                    var node = new SnailNode();
                    node.NumericValue = int.Parse(currChar.ToString());
                    node.Parent = parent;

                    parent.Children.Add(node);
                    Numbers.Add(node);
                }
                else if (currChar == ']')
                    stack.Pop();
            }
        }

        public SnailNum Add(SnailNum right)
        {
            var sumStr = "[" + root.ToString() + ","+ right.root.ToString() + "]";
            var sum = new SnailNum(sumStr);
            sum.Reduce();
            return sum;
        }

        private void Explode(List<SnailNode> pair)
        {
            var pairLeft = pair[0];
            var pairRight = pair[1];

            var numAtLeft = Numbers.ElementAtOrDefault(Numbers.IndexOf(pairLeft) - 1);
            var numAtRight = Numbers.ElementAtOrDefault(Numbers.IndexOf(pairRight) + 1);

            if (numAtLeft != null)
                numAtLeft.NumericValue += pairLeft.NumericValue;

            if (numAtRight != null)
                numAtRight.NumericValue += pairRight.NumericValue;

            pairLeft.Parent.NumericValue = 0;

            Numbers.Insert(Numbers.IndexOf(pairLeft), pairLeft.Parent);
            Numbers.Remove(pairLeft);
            Numbers.Remove(pairRight);
            pairLeft.Parent.Children.Remove(pairLeft);
            pairRight.Parent.Children.Remove(pairRight);

        }

        private void Split(SnailNode numberToSplit)
        {
            var value = numberToSplit.NumericValue.Value;
            var index = Numbers.IndexOf(numberToSplit);

            var newNumLeft = new SnailNode();
            newNumLeft.NumericValue = value / 2;

            var newNumRight = new SnailNode();
            newNumRight.NumericValue = (value+1) / 2;

            Numbers.Insert(index + 1, newNumLeft);
            Numbers.Insert(index + 2, newNumRight);

            numberToSplit.Children.Add(newNumLeft);
            numberToSplit.Children.Add(newNumRight);

            newNumLeft.Parent = numberToSplit;
            newNumRight.Parent = numberToSplit;
            numberToSplit.NumericValue = null;

            Numbers.RemoveAt(index);
        }

        public void Reduce()
        {
            var worked = true;

            while (worked)
            {
                worked = false;
                var q1 = Numbers.Where(n => n.Depth() > 4).ToList();  // to Explode
                var pairs = q1.Windowed(2).Where(p => p[0].Parent == p[1].Parent).ToList();
                if (pairs.Count() > 0)
                {
                    var pairToExplode = pairs.First().ToList();
                    Explode(pairToExplode);
                    worked = true;
                    continue;
                }

                var q2 = Numbers.Where(n => n.NumericValue != null && n.NumericValue > 9).ToList(); // to Split
                if (q2.Count() > 0)
                {
                    var numberToSplit = q2.First();
                    Split(numberToSplit);
                    worked = true;
                }
            }
        }

        private int getNodeMagnitude(SnailNode node)
        {
            if (node.NumericValue.HasValue)
                return node.NumericValue.Value;

            return 3 * getNodeMagnitude(node.Left) + 2 * getNodeMagnitude(node.Right);
        }

        public int Magnitude
            => getNodeMagnitude(root);
    }

    internal class SnailfishSolver
    {
        List<SnailNum> snailNumbers = new List<SnailNum>();

        public void ParseInput(List<string> lines)
            => lines.ForEach(x => snailNumbers.Add(new SnailNum(x)));

        int SumAll()
        {
            SnailNum acum = snailNumbers[0];
            SnailNum number = null;

            foreach (var num in snailNumbers.Skip(1))
            {
                var sum = acum.Add(num);
                acum = sum;
            }
            return acum.Magnitude;
        }

        public int Solve(int part = 1)
            => SumAll();
    }
}

#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8629 // Un tipo que acepta valores NULL puede ser nulo.