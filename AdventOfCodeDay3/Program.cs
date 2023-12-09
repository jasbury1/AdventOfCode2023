using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day3
{
    public record Point(int x, int y);

    public record PartNumber(int value, Point start, int length);
    

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
            List<PartNumber> partNumbers = new();
            foreach (string line in textFromFile)
            {
                var schematicRow = line.ToCharArray();
                schematic[row] = schematicRow;
                partNumbers.AddRange(ParsePartNumbers(schematicRow, row));
                row++;
            }

            var result = partNumbers.Where(x => IsPartTouchingSymbol(x, schematic))
                .Select(x => x.value)
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
                        if (adjacentParts.Count() == 2) {total += adjacentParts[0].value * adjacentParts[1].value;}
                    }
                }
            }

            Console.WriteLine($"Result Part 2: {total}");
        }

        public static IEnumerable<PartNumber> ParsePartNumbers(char[] schematicRow, int rowNumber)
        {
            int i = 0;
            List<PartNumber> partNumbers = new();

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
            for(int i = part.start.y - 1; i < part.start.y + 2; i++)
            {
                for(int j = part.start.x - 1; j <= part.start.x + part.length; j++)
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
            return (Math.Abs(part.start.y - gear.y) <= 1) && 
                (gear.x >= (part.start.x - 1) && gear.x <= (part.start.x + part.length));
        }
    } 

}
