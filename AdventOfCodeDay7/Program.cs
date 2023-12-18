using System;
using System.IO;

namespace AdventOfCode.Day5
{
    public enum HandStrength
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    public class Hand : IComparable
    {
        public string Cards { get; init; }

        public HandStrength Strength {get; private set;}

        public Hand(string cards)
        {
            Cards = cards;
            SetStrength();
        }

        public override string ToString()
        {
            return $"{Cards}: {Strength}";
        }

        public int CompareTo(object? obj) {
            if (obj == null) return 1;

            Hand? otherHand = obj as Hand;
            if (otherHand != null)
            {
                if (otherHand == null)
                {
                    return -1;
                }
                else if (this == null)
                {
                    return 1;
                }
                else if ((int)this.Strength > (int)otherHand.Strength)
                {
                    return -1;
                }
                else if ((int)this.Strength < (int)otherHand.Strength)
                {
                    return 1;
                }
                else 
                {
                    for (int i = 0; i < this.Cards.Length; i++)
                    {
                        var compare = Hand.CompareCards(this.Cards[i], otherHand.Cards[i]);
                        if (compare != 0)
                        {
                            return compare;
                        }
                    }
                    Console.WriteLine("Zero!");
                    return 0;
                }
            }
            else
            {
                throw new ArgumentException("Object is not a Hand");
            }   
        }

        public static int CompareCards(char card1, char card2)
        {
            char[] cardOrder = {'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'};
            var card1Index = Array.IndexOf(cardOrder, card1);
            var card2Index = Array.IndexOf(cardOrder, card2);

            if (card1Index == card2Index)
            {
                return 0;
            }
            if (card1Index < card2Index)
            {
                return -1;
            }
            return 1;
        }

        private void SetStrength()
        {
            if (Cards == "JJJJJ")
            {
                Strength = HandStrength.FiveOfAKind;
                return;
            }

            var cardArray = Cards.ToCharArray();
            Console.WriteLine(Cards);
            var mostCommonChar = cardArray.Where(x => x != 'J').GroupBy(item => item).OrderByDescending(g => g.Count()).Select(g => g.Key).First();

            for (int i = 0; i < cardArray.Length; ++i)
            {
                if (cardArray[i] == 'J')
                {
                    cardArray[i] = mostCommonChar;
                }
            }
            Array.Sort(cardArray);
            var distinct = cardArray.Distinct().Count();

            if (cardArray[0] == cardArray[4])
            {
                Strength = HandStrength.FiveOfAKind;
            }
            else if (cardArray[0] == cardArray[3] || cardArray[1] == cardArray[4])
            {
                Strength = HandStrength.FourOfAKind;
            }
            else if (distinct == 2)
            {
                Strength = HandStrength.FullHouse;
            }
            else if (cardArray[0] == cardArray[2] || cardArray[1] == cardArray[3] || cardArray[2] == cardArray[4])
            {
                Strength = HandStrength.ThreeOfAKind;
            }
            else if (distinct == 3)
            {
                Strength = HandStrength.TwoPair;
            }
            else if (distinct == 4)
            {
                Strength = HandStrength.OnePair;
            }
            else
            {
                Strength = HandStrength.HighCard;
            }
        }
    }

    public record Bid(Hand hand, int amount);

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
            List<Bid> bids = new();
            foreach (var line in textFromFile)
            {
                string[] lineSplit = line.Split(' ');
                bids.Add(new Bid(new Hand(lineSplit[0]), int.Parse(lineSplit[1])));
            }

            bids.Sort((x, y) => x.hand.CompareTo(y.hand));
            Console.WriteLine(string.Join('\n', bids));

            var total = 0;
            var bidCount = bids.Count();
            for (int i = 0; i < bidCount; ++i)
            {
                total += (bids[i].amount * (bidCount - i));
            }

            Console.WriteLine($"Total: {total}");
        } 
    }

}
