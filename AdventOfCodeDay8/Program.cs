using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day8;

partial class Program
{
    private static readonly string Directions = "LRLRLRLRRLRRRLRLRLRRRLLRRLRRLRRLLRRLRRLRLRRRLRRLLRRLRRRLRRLRRRLRRRLLLRRLLRLLRRRLLRRLRLLRLLRRRLLRRLRRLRRRLRRLRLRRLRRLRLLRLRRRLRLRRLRLLRRLRRRLRRLRLRRLLLRRLRRRLRRRLRRLRRRLRLRRLRRLRRRLRRLRRLRRLRRLRRRLLRRRLLLRRRLRRLRRRLLRRRLRRLRRLLLLLRRRLRLRRLRRLLRRLRRLRLRLRRRLRRRLRRLLLRRRR";
    
    public record Node(string Left, string Right);

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
        Dictionary<string, Node> nodeDict = [];
        foreach (var line in textFromFile)
        {
            AddNewNode(line, nodeDict);
        }

        List<string> nodeStarts = [];
        foreach (var n in nodeDict.Keys)
        {
            if (n.EndsWith('A'))
            {
                nodeStarts.Add(n);
            }
        }

        var pathLengths = nodeStarts.Select(x => ExecuteMapSequence(x, nodeDict)).ToList();
        //Console.WriteLine(string.Join(',', pathLengths));
        var result = LCMAllMapSequences(pathLengths);
        Console.WriteLine($"Result: {result}");
    }

    public static void AddNewNode(string line, Dictionary<string, Node> nodeDict)
    {
        
        string pattern = @"(?<start>\w+)\s=\s\((?<left>\w+),\s(?<right>\w+)\)";
        Match m = Regex.Match(line, pattern);
        var n = new Node(m.Groups["left"].Value, m.Groups["right"].Value);
        nodeDict.Add(m.Groups["start"].Value, n);
    }

    public static (string, int) ExecuteMapSequence(string startNode, Dictionary<string, Node> nodeDict)
    {
        var currentNode = startNode;
        int iteration = 0;
        while (!currentNode.EndsWith('Z'))
        {
            var move = Directions[iteration % Directions.Length];
            if (move == 'L')
            {
                currentNode = nodeDict[currentNode].Left;
            }
            if (move == 'R')
            {
                currentNode = nodeDict[currentNode].Right;
            }
            iteration++;
        }
        return (currentNode, iteration);
    }

    public static long LCMAllMapSequences(List<(string, int)> nodePaths)
    {
        var traversal = nodePaths.Select(x => x.Item2).ToArray();
        return LCMArray(traversal);
    }

    public static long LCMArray(int[] arr)
    {
        long lcm = 1;
        var divisor = 2;
         
        while (true) {
             
            int counter = 0;
            bool divisible = false;
            for (int i = 0; i < arr.Length; i++) 
            {
                if (arr[i] == 0) 
                {
                    return 0;
                }
                else if (arr[i] < 0) 
                {
                    arr[i] = arr[i] * (-1);
                }
                if (arr[i] == 1) 
                {
                    counter++;
                }
                if (arr[i] % divisor == 0) 
                {
                    divisible = true;
                    arr[i] = arr[i] / divisor;
                }
            }
            if (divisible) 
            {
                lcm *= divisor;
            }
            else 
            {
                divisor++;
            }
            if (counter == arr.Length) 
            {
                return lcm;
            }
        }
    }

}


