namespace AoC21.Day12
{
    internal class CaveExplorer
    {
        Dictionary<string, HashSet<string>> paths = new();

        void ParseLine(string line)
        { 
            var parts = line.Split('-');

            if (paths.ContainsKey(parts[0]))
                paths[parts[0]].Add(parts[1]);
            else
                paths[parts[0]] = new HashSet<string> { parts[1] };

            if (paths.ContainsKey(parts[1]))
                paths[parts[1]].Add(parts[0]);
            else
                paths[parts[1]] = new HashSet<string> { parts[0] };
        }

        public void ParseInput(List<string> lines)
            => lines.ForEach(ParseLine);

        bool isSmallCave(string node)
            => node.All(x => char.IsLower(x));

        int Traverse(string currentNode, string[] visitedSmallCaves, bool visitedTwice)
        {
            bool newVisitedTwice = visitedTwice;

            if (currentNode == "end")
                return 1;

            if (visitedSmallCaves.Contains(currentNode) && currentNode == "start")
                return 0;

            if (visitedSmallCaves.Contains(currentNode) && visitedTwice)
                return 0;

            if(visitedSmallCaves.Contains(currentNode) && !visitedTwice)
                newVisitedTwice = true;

            int retVal = 0;
            var connections = paths[currentNode];
            string[] newVisited = isSmallCave(currentNode) ? [.. visitedSmallCaves, currentNode] : [.. visitedSmallCaves];

            foreach (var connection in connections)
                retVal += Traverse(connection, newVisited, newVisitedTwice);

            return retVal;
        }

        public int Solve(int part = 1)
            => part == 1 ? Traverse("start", [], true) : Traverse("start", [], false);
    }
}
