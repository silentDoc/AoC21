using AoC21.Common;

namespace AoC21.Day19
{
    delegate Coord3D Transform(Coord3D point);

    static class RotationUtils
    {
        // Helper function to compose two Transform functions
        static Transform Compose(Transform t1, Transform t2)
            => coord => t2(t1(coord));

        // Null transform (identity transformation) adn basic rotations
        static readonly Transform NullTrans = coord => coord;
        static readonly Transform RotX = point => new Coord3D(point.x, -point.z, point.y);
        static readonly Transform RotY = point => new Coord3D(point.z, point.y, -point.x);
        static readonly Transform RotZ = point => new Coord3D(-point.y, point.x, point.z);

        // Rotation in 3D space is an endomorphism of the 3D space, i.e. a function from R^3 to R^3
        // We can represent it as a 3x3 matrix, but we can also represent it as a composition of simpler transformations
        public static IEnumerable<Coord3D> GetAllRotations(Coord3D point)
        {
            // Define rotations as combinations of ras and rbs
            List<Transform> ras = [  NullTrans,
                                     RotY, Compose(RotY, RotY), Compose(Compose(RotY, RotY), RotY),
                                     RotZ, Compose(RotZ, Compose(RotZ, RotZ)) ];

            List<Transform> rbs = [  NullTrans,
                                     RotX, Compose(RotX, RotX), Compose(Compose(RotX, RotX), RotX)];

            // Apply all combinations of ras and rbs to the point
            foreach (var ra in ras)
                foreach (var rb in rbs)
                    yield return rb(ra(point));
        }
    }

    class Scan
    {
        public int Id = -1;
        public Coord3D scannerPosition = (0, 0, 0);
        public List<Coord3D> beacons = new List<Coord3D>();
        public double[][] distances;

        // Beacons normalized to the first scanner position
        public int RotationIndex = -1;
        public List<Coord3D> normalizedBeacons = new List<Coord3D>();
        public Coord3D normalizedScannerPos = new Coord3D(0, 0, 0);

        public Scan(List<string> inputSection)
        {
            var idStr = inputSection[0].Replace("--- scanner ", "").Replace(" ---", "");
            Id = int.Parse(idStr);

            var inputValues = inputSection.Skip(1).Select(x => x.Split(',').Select(int.Parse).ToList()).ToList();
            inputValues.ForEach(x => beacons.Add(new Coord3D(x[0], x[1], x[2])));

            distances = new double[beacons.Count][];
            
            // Distances between all beacons are key - if we have a subset of beacons that are visible by two scanners
            // the distances between them will be the same
            for (int i = 0; i < beacons.Count; i++)
            {
                distances[i] = new double[beacons.Count];
                for (int j = 0; j < beacons.Count; j++)
                    distances[i][j] = beacons[i].DistanceTo(beacons[j]);
            }
        }

        // Normalize the beacons to the first scanner position (0,0,0)
        public void NormalizeBeacons()
            => normalizedBeacons = beacons.Select(x => RotationUtils.GetAllRotations(x).ToList()[RotationIndex] + normalizedScannerPos).ToList();

        public Coord3D? Register(Scan other)
        {
            // Step 1 - Find the beacons that are in the same relative position in both scanners
            //
            // Find the points from the other scanner that have the same distance between them
            // as the points in this scanner. This means that there will be rows or columns that
            // will have more than 3 elements with the same values

            List<Coord3D> goodBeacons = new();
            List<Coord3D> goodBeaconsOther = new();

            foreach (var (index, row) in distances.Index())
            {
                // Each row has the distance from the current beacon to all other beacons
                var rowValues = row.Select(x => Math.Round(x, 2)).ToArray();

                foreach (var (indexOther, otherRow) in other.distances.Index())
                {
                    var otherRowValues = otherRow.Select(x => Math.Round(x, 2)).ToArray();
                    var intersection = otherRowValues.Intersect(rowValues).ToList();

                    // If we find a row of the other scanner that has at least 12 elements in common with the current row
                    // we consider it interesting, because it means that the beacons are in the same relative position
                    if (otherRowValues.Intersect(rowValues).Count() >= 12)
                    {
                        goodBeacons.Add(normalizedBeacons[index]);          // We take the already normalized beacons , to help with next step
                        goodBeaconsOther.Add(other.beacons[indexOther]);
                    }
                }
            }

            // If we don't have enough beacons in common, the scanners do not overlap
            if (goodBeacons.Count < 12)
                return null;

            // Step 2 - If we have a good number of beacons, find the exact rotation that allow us to locate the scanner
            List<List<Coord3D>> matchingBeaconsRotations = goodBeaconsOther.Select(x => RotationUtils.GetAllRotations(x).ToList()).ToList();
            int numRotations = matchingBeaconsRotations[0].Count;
            List<Coord3D> possibleOtherScannerPositions = new();

            // The trick here is that when we find the right rotation, there will be only one possible scanner poisition
            // if the rotation is bad, the x coords will match with y or Z and then there will be 12 different potential positions
            for (int i = 0; i < numRotations; i++)
            {
                List<Coord3D> rotatedBeacons = matchingBeaconsRotations.Select(x => x[i]).ToList(); // get the i-th rotation for all beacons
                possibleOtherScannerPositions = new();

                // Find the real position of the beacon with respect to our beacon scanner pos (known)
                var realBeaconPosition = goodBeacons.Select(x => x + scannerPosition).ToList();
                possibleOtherScannerPositions = realBeaconPosition.Zip(rotatedBeacons, (real, rotated) => real-rotated).ToList();

                if (possibleOtherScannerPositions.Distinct().Count() == 1)
                {
                    // The twelve beacons yield a single possible position :)
                    // The registration ends when we translate the beacons of the other scanner
                    // to the same reference coodinates and rotation as the registered scanner
                    other.RotationIndex = i;
                    other.normalizedScannerPos = possibleOtherScannerPositions[0];
                    other.NormalizeBeacons();
                    break;
                }
            }

            return other.normalizedScannerPos;
        }
    }

    internal class BeaconScanner
    {
        List<Scan> scanList = [];

        public void ParseInput(List<string> input)
        {
            var sections = ParseUtils.SplitBy(input, "");
            sections.ForEach(x => scanList.Add(new Scan(x)));

            // The first scanner is the reference scanner
            scanList[0].RotationIndex = 0;
            scanList[0].normalizedScannerPos = (0, 0, 0);
            scanList[0].NormalizeBeacons();
        }

        int RegisterScans()
        {
            List<Scan> registeredScanners = [scanList[0]];
            List<Scan> nonRegisteredScanners = scanList.Skip(1).ToList();

            while (nonRegisteredScanners.Count > 0)
            {
                List<Scan> addedRegScanners = [];

                foreach (var nonRegScan in nonRegisteredScanners)
                    foreach (var regScan in registeredScanners)
                    {
                        if(regScan.Register(nonRegScan) is not null)
                            addedRegScanners.Add(nonRegScan);
                    }
                
                // We will retry with the new ones, as the missing non registered have already been tested with the previous registered scanners
                registeredScanners = addedRegScanners;  
                nonRegisteredScanners = scanList.Where(x => x.RotationIndex == -1).ToList();
            }
            
            return scanList.SelectMany(x => x.normalizedBeacons).Distinct().Count();
        }

        int FindLargestManhattan()
        { 
            RegisterScans();    // Register the scanners and get their normalized position
            var normalizedScannerPositions = scanList.Select(x => x.normalizedScannerPos).ToList();
            return normalizedScannerPositions.SelectMany(x => normalizedScannerPositions.Select(y => y.Manhattan(x))).Max();
        }

        public int Solve(int part = 1)
            => part == 1? RegisterScans() : FindLargestManhattan();
    }
}
