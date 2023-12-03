using System.Text;

namespace AdventOfCode.Day1
{
    public class CalibrationParser
    {
        private static readonly Dictionary<string, string> NormalizationDict = new()
        {
            { "one", "one1one" },
            { "two", "two2two" },
            { "three", "three3three" },
            { "four", "four4four" },
            { "five", "five5five" },
            { "six", "six6six" },
            { "seven", "seven7seven" },
            { "eight", "eight8eight" },
            { "nine", "nine9nine" }
        };

        public static string NormalizeCalibrationStr(string input)
        {
            StringBuilder sb = new(input);
            foreach(var (key, val) in NormalizationDict)
            {
                sb.Replace(key, val);
            }
            return sb.ToString();
        }

        public static int ParseCalibrationValue(string line)
        {
            var firstval = -1;
            var lastval = -1;
            int i = 0;

            // First loop grabs the first occurrance of a number
            for (; i < line.Length; i++)
            {
                if (char.IsDigit(line[i]))
                {
                    firstval = line[i] - '0';
                    lastval = firstval;
                    i++;
                    break;
                }
            }
            // Second loop grabs the last occurrance of a number
            for (; i < line.Length; i++)
            {
                if (char.IsDigit(line[i]))
                {
                    lastval = line[i] - '0';
                }
            }
            return firstval * 10 + lastval;
        }
    }
}