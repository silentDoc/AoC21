using AoC21.Common;
using System.Diagnostics;

namespace AoC21.Day23
{
    public record Amphipod(Coord2D position, char Type, int EnergyCost, int TargetCol);
    public record Move(Coord2D position, int Steps);
    public record State(List<Amphipod> Amphipods, int Energy)
    {
        public virtual bool Equals(State? other) =>
            other is not null && Amphipods.SequenceEqual(other.Amphipods);

        public override int GetHashCode() =>
            Amphipods.Aggregate(17, (hash, amphipod) => hash * 23 + amphipod.GetHashCode());
    }

    internal class AmphiMover
    {
        Dictionary<Coord2D, char> Map = new();

        void ParseLine(int row, string line)
            => line.Index().ToList().ForEach(c => Map[(c.Index, row)] = c.Item);

        public void ParseInput(List<string> input)
            => input.Index().ToList().ForEach(x => ParseLine(x.Index, x.Item));

        int SolveMap()
        {
            var initialState = GetState(Map);
            var visited = new HashSet<State>();
            var active = new PriorityQueue<State, int>();
            active.Enqueue(initialState, 0);

            List<Coord2D> hallwayPositions = [(1, 1), (2, 1), (4, 1), (6, 1), (8, 1), (10, 1), (11, 1)];

            while (active.Count > 0)
            {
                var state = active.Dequeue();
                var (amphipods, energy) = state;

                if (visited.Contains(state))
                    continue;

                visited.Add(state);

                if (IsFinalState(state))
                    return energy;

                foreach (var amphipod in amphipods)
                {
                    var possibleMoves = GetAllMoves(amphipod, amphipods, Map, hallwayPositions).ToList();

                    foreach (var move in possibleMoves)
                    {
                        var index = amphipods.IndexOf(amphipod);
                        var newAmphipod = amphipod with { position = move.position};
                        var newAmphipods = amphipods[..index]
                            .Append(newAmphipod)
                            .Concat(amphipods[(index + 1)..])
                            .ToList();

                        var newState = new State(newAmphipods, energy + amphipod.EnergyCost * move.Steps);
                        active.Enqueue(newState, newState.Energy);
                    }
                }
            }

            throw new Exception("No solution found!");
        }

        State GetState(Dictionary<Coord2D, char> map)
        {
            var amphipods = new List<Amphipod>();
            foreach (var element in map)
            {
                var pos = element.Key;
                var value = element.Value;

                Amphipod? amp = value switch
                {
                    'A' => new Amphipod(pos, value, 1, 3),
                    'B' => new Amphipod(pos, value, 10, 5),
                    'C' => new Amphipod(pos, value, 100, 7),
                    'D' => new Amphipod(pos, value, 1000, 9),
                    _ => null
                };

                if (amp is not null)
                    amphipods.Add(amp);
            }
            return new State(amphipods, 0);
        }

        IEnumerable<Move> GetAllMoves(Amphipod amphipod, List<Amphipod> amphipods, Dictionary<Coord2D, char> map, List<Coord2D> hallway)
        {
            if (IsAtFinalDestination(amphipod, amphipods, map))
                yield break;

            if (TryGetFinalDest(amphipod, amphipods, map, out var finalDestination))
                if (TryReachPos(amphipod, finalDestination, amphipods, map, out var move))
                    yield return move;

            if (amphipod.position.y == 1)
                yield break;

            foreach (var position in hallway)
                if (TryReachPos(amphipod, position, amphipods, map, out var move))
                    yield return move;
        }

        private static bool TryReachPos(Amphipod amphipod, Coord2D destination, List<Amphipod> amphipods, Dictionary<Coord2D, char> map, out Move? move)
        {
            var visited = new HashSet<Coord2D>();
            var active = new Queue<(Coord2D pos, int Steps)>();
            active.Enqueue(( amphipod.position, 0));

            while (active.Any())
            {
                var state = active.Dequeue();
                var (position, steps) = state;

                if (visited.Contains(position))
                    continue;

                visited.Add(position);

                if (position == destination)
                {
                    move = new Move(position, steps);
                    return true;
                }

                var neighbors = position.GetNeighbors().Where(n => map.ContainsKey(n) && map[n] != '#').ToList();
                foreach (var neigh in neighbors)
                {
                    if (amphipods.Any(a => a.position == neigh))
                        continue;
                    else
                        active.Enqueue((neigh, steps + 1));
                }
            }

            move = null;
            return false;
        }

        bool TryGetFinalDest(Amphipod amphipod, List<Amphipod> amphipods, Dictionary<Coord2D, char> map, out Coord2D finalDestination)
        {
            var (_, type, _, targetCol) = amphipod;
            finalDestination = (-1, -1);

            if (RoomContainsTypeThatDoesNotMatch(targetCol, type, amphipods))
                return false;
            
            var minRow = 2;
            var maxRow = 3;

            for (var row = maxRow; row >= minRow; row--)
                if (IsEmptySpace((targetCol,row), amphipods))
                {
                    finalDestination = (targetCol, row);
                    return true;
                }

            throw new UnreachableException("F!");
        }

        private static bool IsAtFinalDestination(Amphipod amphipod, List<Amphipod> amphipods, Dictionary<Coord2D, char> map)
        {
            var (position, type, _, targetCol) = amphipod;

            if (position.x != targetCol)
                return false;

            for (var row = position.y + 1; row < map.Count - 1; row++)
                if (amphipods.Any(a => a.position == (position.x ,row) && a.Type != type))
                    return false;

            return true;
        }

        bool IsFinalState(State state) 
            =>state.Amphipods.All(a => a.position.x == a.TargetCol);

        bool RoomContainsTypeThatDoesNotMatch(int targetCol, char type, IEnumerable<Amphipod> amphipods) =>
            amphipods.Any(a => a.position.x == targetCol && a.Type != type);

        bool IsEmptySpace(Coord2D position, IEnumerable<Amphipod> amphipods) =>
            !amphipods.Any(a => a.position == position);

        public int Solve(int part = 1)
            => SolveMap();
    }
}
