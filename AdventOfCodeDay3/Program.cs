namespace AdventOfCode.Day3
{
    public record Point(int X, int Y);

    public record PartNumber(int Value, Point Start, int Length);
    

    partial class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("Please pass a file to parse");
            }
            string filePath = args[0];
            if(!File.Exists(filePath))
            {
                throw new ArgumentException($"The file at {filePath} does not exist");
            }      

            string[] textFromFile = File.ReadAllLines(filePath);
            char[][] schematic = new char[textFromFile.Length][];

            int row = 0;
            List<PartNumber> partNumbers = [];
            foreach (string line in textFromFile)
            {
                var schematicRow = line.ToCharArray();
                schematic[row] = schematicRow;
                partNumbers.AddRange(ParsePartNumbers(schematicRow, row));
                row++;
            }

            var result = partNumbers.Where(x => IsPartTouchingSymbol(x, schematic))
                .Select(x => x.Value)
                .Sum();

            Console.WriteLine($"Result Part 1: {result}");

            long total = 0;
            for (int i = 0; i < schematic.Length; ++i)
            {
                for (int j = 0; j < schematic[0].Length; ++j)
                {
                    if (schematic[i][j] == '*')
                    {
                        var adjacentParts = partNumbers.Where(x => IsPartTouchingGear(x, new Point(j, i))).ToList();
                        if (adjacentParts.Count == 2) {total += adjacentParts[0].Value * adjacentParts[1].Value;}
                    }
                }
            }

            Console.WriteLine($"Result Part 2: {total}");
        }

        public static IEnumerable<PartNumber> ParsePartNumbers(char[] schematicRow, int rowNumber)
        {
            int i = 0;
            List<PartNumber> partNumbers = [];

            while (i < schematicRow.Length)
            {
                if (char.IsDigit(schematicRow[i]))
                {
                    int value = 0;
                    int length = 0;
                    Point start = new(i, rowNumber);
                    while (i < schematicRow.Length && char.IsDigit(schematicRow[i]))
                    {
                        value = (value * 10) + (schematicRow[i] - '0');
                        length++;
                        i++;
                    }
                    partNumbers.Add(new PartNumber(value, start, length));
                }
                else
                {
                    i++;
                }   
            }
            return partNumbers;
        }

        public static bool IsPartTouchingSymbol(PartNumber part, char[][] schematic)
        {
            var schematicRowLen = schematic[0].Length;
            for(int i = part.Start.Y - 1; i < part.Start.Y + 2; i++)
            {
                for(int j = part.Start.X - 1; j <= part.Start.X + part.Length; j++)
                {
                    if (i >= 0 && i < schematic.Length && j >= 0 && j < schematicRowLen)
                    {
                        char c = schematic[i][j];
                        if (!char.IsDigit(c) && c != '.' && !char.IsWhiteSpace(c))
                        {
                            Console.WriteLine(part);
                            Console.WriteLine(c);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsPartTouchingGear(PartNumber part, Point gear)
        {
            return (Math.Abs(part.Start.Y - gear.Y) <= 1) && 
                gear.X >= (part.Start.X - 1) && gear.X <= (part.Start.X + part.Length);
        }
    } 

}
