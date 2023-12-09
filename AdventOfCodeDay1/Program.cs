using System;
using System.IO;
using AdventOfCode.Day1;

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

        var total = 0;
        foreach (string line in textFromFile)
        {
            var input = CalibrationParser.NormalizeCalibrationStr(line);
            var value = CalibrationParser.ParseCalibrationValue(input);
            total += value;
        }

        Console.WriteLine($"Total: {total}");
    }
}
