using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day4
{
    public class Scratchcard
    {
        public HashSet<int> WinningNumbers { get; }
        public List<int> CardNumbers { get; }

        public int Matches { get; }

        public Scratchcard (HashSet<int> winningNumbers, List<int> cardNumbers)
        {
            WinningNumbers = winningNumbers;
            CardNumbers = cardNumbers;

            int matches = 0;
            foreach (var num in CardNumbers)
            {
                if (WinningNumbers.Contains(num))
                {
                    matches++;
                }
            }
            Matches = matches;
        }

        public int GetScore()
        {
            int score = 0;
            foreach (var num in CardNumbers)
            {
                if (WinningNumbers.Contains(num))
                {
                    score = (score == 0 ? 1 : score * 2);
                }
            }
            return score;
        }
    }

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

            List<Scratchcard> cards = new();
            string[] textFromFile = File.ReadAllLines(filePath);
            foreach (var line in textFromFile)
            {
                cards.Add(ParseScratchcard(line));
            }

            var cardCounts = new int[cards.Count()];
            Array.Fill(cardCounts, 1);

            for (int i = 0; i < cardCounts.Length; i++)
            {
                ProcessScratchCard(i, cardCounts, cards);
            }
            
            Console.WriteLine($"Total: {cardCounts.Sum()}");
        }

        public static void ProcessScratchCard(int cardNumber, int[] cardCounts, List<Scratchcard> cards)
        {
            var matches = cards[cardNumber].Matches;
            for(int i = cardNumber + 1; i < cardNumber + 1 + matches; i++)
            {
                cardCounts[i] += cardCounts[cardNumber];
            }
        }

        public static Scratchcard ParseScratchcard(string line)
        {
            HashSet<int> winningNumbers = new();
            List<int> cardNumbers = new();

            ICollection<int> currentSet = winningNumbers;

            int i = 0;

            // Iterate past header info on each line
            while(line[i] != ':')
            {
                i++;
            }

            while (i < line.Length)
            {
                if (line[i] == '|')
                {
                    currentSet = cardNumbers;
                }
                if (char.IsDigit(line[i]))
                {
                    int value = 0;
                    while (i < line.Length && char.IsDigit(line[i]))
                    {
                        value = (value * 10) + (line[i] - '0');
                        i++;
                    }
                    currentSet.Add(value);
                }
                else
                {
                    i++;
                }
            }

            return new Scratchcard(winningNumbers, cardNumbers);
        }
    } 

}
