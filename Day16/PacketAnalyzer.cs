namespace AoC21.Day16
{
    class Packet
    {
        public int Version;
        public int Type;
        public long Value;
        public Packet[] Children;

        public Packet(int ver, int type, long value, Packet[] children)
        {
            Version = ver;
            Type = type;
            Value = value;
            Children = children;
        }

        public int GetVersionSum
            => Version + Children.Select(x => x.GetVersionSum).Sum();

        public int GetCount
           => 1 + Children.Select(x => x.GetCount).Sum();
    }

    class BitReader
    {
        string bits = "";
        public int ptr = 0;

        string hexToBin(char hexChar)
            => hexChar switch
            {
                '0' => "0000", '1' => "0001", '2' => "0010", '3' => "0011",
                '4' => "0100", '5' => "0101", '6' => "0110", '7' => "0111",
                '8' => "1000", '9' => "1001", 'A' => "1010", 'B' => "1011",
                'C' => "1100", 'D' => "1101", 'E' => "1110", 'F' => "1111",
                _ => throw new Exception("Invalid input char : " + hexChar.ToString())
            };

        public BitReader(string input, bool fromBinary = false)
        {
            if (!fromBinary)
            {
                List<string> binaryBatches = input.Select(x => hexToBin(x)).ToList();
                bits = string.Concat(binaryBatches);
            }
            else
                bits = input;
        }

        public bool Any()
            => ptr < bits.Length;

        public BitReader GetBitReader(int bitCount)
        {
            var subString = bits.Substring(ptr, bitCount);
            ptr += bitCount;
            return new BitReader(subString, true);
        }

        public int GetLength
            => bits.Length;

        public int ReadInt(int bitCount)
        {
            var retVal = Convert.ToInt32(bits.Substring(ptr, bitCount), 2);
            ptr += bitCount;
            return retVal;
        }
    }

    class PacketParser
    {
        Packet ParsePacket(BitReader reader)
        {
            var version = reader.ReadInt(3);
            var type = reader.ReadInt(3);
            var children = new List<Packet>();
            var literalValue = 0L;

            if (type == 4) // literal
            {
                while (true)
                {
                    var isLast = reader.ReadInt(1) == 0;
                    literalValue = literalValue * 16 + reader.ReadInt(4); // Binary - 4 digits means 0 to 16.
                    if (isLast)
                        break;
                }
            }
            else if (reader.ReadInt(1) == 0)
            {
                var length = reader.ReadInt(15);
                var childrenReader = reader.GetBitReader(length);

                while (childrenReader.Any())
                    children.Add(ParsePacket(childrenReader));
            }
            else
            {
                var packetCount = reader.ReadInt(11);

                for (int i = 0; i < packetCount; i++)
                    children.Add(ParsePacket(reader));
            }
            return new Packet(version, type, literalValue, children.ToArray());
        }

        public Packet Parse(string input)
        { 
            BitReader reader = new BitReader(input);
            return ParsePacket(reader);
        }
    }


    internal class PacketAnalyzer
    {
        string input = "";
        public void ParseInput(List<string> lines)
            => input = lines[0];

        int FindVersionSum()
        {
            PacketParser parser = new();
            var packet = parser.Parse(input);
            return packet.GetVersionSum;
        }

        public int Solve(int part = 1)
            => FindVersionSum();
    }
}
