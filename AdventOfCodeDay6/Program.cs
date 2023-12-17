using System;

namespace AdventOfCode.Day6
{

    partial class Program
    {
        public static void Main(string[] args)
        {
            long[] times = {48876981};
            long[] distances = {255128811171623};

            long result = 1;
            for (int i = 0; i < times.Length; i++)
            {
                result *= (GetAllWinningValues(distances[i], times[i]));
            }
            Console.WriteLine(result);
        }

        public static long GetAllWinningValues(long distanceToBeat, long timeAllowed)
        {
            var (min, max) = GetTimeHeldBounds(distanceToBeat, timeAllowed);
            var minBound = (long)min;
            if (minBound <= distanceToBeat)
            {
                minBound += 1;
            }
            var maxBound = (int)max - 1;
            return maxBound - minBound + 1;
        }

        public static (double min, double max) GetTimeHeldBounds(long minDistance, long timeAllowed)
        {
            var discriminant = (timeAllowed * timeAllowed) - 4 * (minDistance);
            var d1 = (((-1 * timeAllowed) + Math.Sqrt(discriminant)) / -2);
            var d2 = (((-1 * timeAllowed) - Math.Sqrt(discriminant)) / -2);
            if (d1 < d2) 
            {
                return (d1, d2);
            }
            return (d2, d1);
        }

    }

}
