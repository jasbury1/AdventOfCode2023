using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day2
{
    public record GameResult(int GreenCount, int RedCount, int BlueCount)
    {
        public int Power => GreenCount * RedCount * BlueCount;
    }

    partial class Program
    {
        public static readonly int MaxGreen = 13;
        public static readonly int MaxRed = 12;

        public static readonly int MaxBlue = 14;

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
                var rounds = line.Split(';').Select(ParseResult);
                var minValues = GetMinPossibleCounts(rounds);
                total += minValues.Power;
            }

            Console.WriteLine($"Total: {total}");
        }

        public static GameResult ParseResult(string gameState)
        {
            var greenRe = @"(?<green>\d+) green";
            var redRe = @"(?<red>\d+) red";
            var blueRe = @"(?<blue>\d+) blue";

            var greenCount = 0;
            var redCount = 0;
            var blueCount = 0;

            Match m = Regex.Match(gameState, greenRe);
            if (m.Success)
            {
                greenCount = int.Parse(m.Groups["green"].Value);
            }
            m = Regex.Match(gameState, redRe);
            if (m.Success)
            {
                redCount = int.Parse(m.Groups["red"].Value);
            }
            m = Regex.Match(gameState, blueRe);
            if (m.Success)
            {
                blueCount = int.Parse(m.Groups["blue"].Value);
            }

            GameResult result = new(greenCount, redCount, blueCount);
            return result;
        }

        public static bool IsGameValid(GameResult result, int maxGreen, int maxRed, int maxBlue)
        {
            return result.BlueCount <= maxBlue &&
                   result.GreenCount <= maxGreen &&
                   result.RedCount <= maxRed;
        }

        public static GameResult GetMinPossibleCounts(IEnumerable<GameResult> results)
        {
            int minReds = int.MinValue;
            int minBlues = int.MinValue;
            int minGreens = int.MinValue;

            foreach(var result in results)
            {
                if (result.RedCount > minReds) {minReds = result.RedCount;}
                if (result.BlueCount > minBlues) {minBlues = result.BlueCount;}
                if (result.GreenCount > minGreens) {minGreens = result.GreenCount;}
            }
            return new GameResult(minGreens, minReds, minBlues);
        }
    }

}
