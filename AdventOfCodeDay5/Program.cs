using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace AdventOfCode.Day5
{

    public record Mapping(long DestRangeStart, long SrcRangeStart, long RangeLength);

    public record SeedRange(long SeedStart, long RangeLength);

    partial class Program
    {
        public static readonly long[] seeds = [104847962, 3583832, 1212568077, 114894281, 3890048781, 333451605, 
                                               1520059863, 217361990, 310308287, 12785610, 3492562455, 292968049, 
                                               1901414562, 516150861, 2474299950, 152867148, 3394639029, 59690410, 
                                               862612782, 176128197];
                
        private static object _lock = new();

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

            
            var almanac = ParseAlmanac(textFromFile);

            // Sort the mappings in each almanac category so that binary search can be performed
            foreach (var mapping in almanac)
            {
                mapping.Sort(new MapComparer());
            }

            // Parse all the seed ranges into a list
            List<SeedRange> seedRanges = new();
            for (int i = 0; i < seeds.Length - 1; i += 2)
            {
                seedRanges.Add(new SeedRange(seeds[i], seeds[i + 1]));
            }

            // Brute force: Find the best result
            var bestResult = long.MaxValue;
            Parallel.For(0, seedRanges.Count(),
                         index => { var seedRange = seedRanges[index];
                                    var result = GetBestSeedLocationFromRange(almanac, seedRange);
                                    lock (_lock)
                                    {
                                        if (result < bestResult)
                                        {
                                            bestResult = result;
                                        }
                                    }
                         } );

            Console.WriteLine($"Best location: {bestResult}");
        }
        
        static List<List<Mapping>> ParseAlmanac(string[] almanacLines)
        {
            int index = 0;
            List<List<Mapping>> almanac = new();
            almanac.Add(new List<Mapping>());

            foreach (var line in almanacLines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    index++;
                    almanac.Add(new List<Mapping>());
                }
                else if (char.IsDigit(line[0]))
                {
                    var values = line.Split(' ').Select(x => long.Parse(x)).ToList();
                    almanac[index].Add(new Mapping(values[0], values[1], values[2]));
                }
            }
            return almanac;
        }

        static bool ValueWithinMappedRange(Mapping mapping, long val)
        {
            return val >= mapping.SrcRangeStart && val < mapping.SrcRangeStart + mapping.RangeLength;
        }

        static long GetMappedValue(Mapping mapping, long val)
        {
            return (val - mapping.SrcRangeStart) + mapping.DestRangeStart;
        }

        public static long GetBestSeedLocationFromRange(List<List<Mapping>> almanac, SeedRange seedRange)
        {
            long bestResult = long.MaxValue;
            for (long i = seedRange.SeedStart; i < seedRange.SeedStart + seedRange.RangeLength; i++)
            {
                var result = GetSeedLocation(almanac, i);
                if (result < bestResult)
                {   
                    bestResult = result;
                }
            }
            return bestResult;
        }

        public static long GetSeedLocation(List<List<Mapping>> almanac, long seed)
        {
            long result = seed;
            foreach (var map in almanac)
            {
                var mapIdx = map.BinarySearch(new Mapping(0, result, 0), new MapComparer());
                if (mapIdx > -1)
                {
                    result = GetMappedValue(map[mapIdx], result);
                }
            }
            return result;
        }
    }

    public class MapComparer : Comparer<Mapping>
    {
        // Compares by Length, Height, and Width.
        public override int Compare(Mapping? m1, Mapping? m2)
        {
            if (m2 == null)
            {
                return -1;
            }
            if (m1 == null)
            {
                return 1;
            }
            if (m2.SrcRangeStart >= m1.SrcRangeStart && m2.SrcRangeStart + m2.RangeLength > m1.SrcRangeStart + m1.RangeLength)
            {
                return -1;
            }
            if (m1.SrcRangeStart >= m2.SrcRangeStart && m1.SrcRangeStart + m1.RangeLength > m2.SrcRangeStart + m2.RangeLength)
            {
                return 1;
            }
            return 0;
        }
    }

}
