using System.ComponentModel.Design;

namespace OzonTech.AsciiRobotTask;

public class Program
{
    public static void Main(string[] args)
    {
        Test();
    }
    
    private static void Test()
    {
        var files = Directory.GetFiles(Path.Combine("AsciiRobotTask", "ascii-robots"));
        var dataMap = new Dictionary<int, Data<string>>();

        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            if (file.EndsWith('a'))
            {
                var lines = File.ReadAllLines(file).ToList();
                fileName = fileName.Replace(".a", "");
                if (dataMap.TryGetValue(Convert.ToInt32(fileName), out var d))
                {
                    d.Output = [.. lines];
                }
                else
                {
                    dataMap.TryAdd(Convert.ToInt32(fileName), new Data<string> { Output = lines.ToList() });
                }
                continue;
            }
            
            Console.WriteLine(file);
            
            using var input = new StreamReader(new FileStream(file, FileMode.Open));
            using var output = new StreamWriter(Console.OpenStandardOutput());
            
            var inputCount = Convert.ToInt32(input.ReadLine());
            var outputList = new List<string>();

            for (var i = 0; i < inputCount; i++)
            {
                outputList.AddRange(GetWayRobots(input));
            }
            
            if (dataMap.TryGetValue(Convert.ToInt32(fileName), out var data))
            {
                data.Input = outputList;
            }
            else
            {
                dataMap.TryAdd(Convert.ToInt32(fileName), new Data<string> { Input = outputList });
            }
        }
        
        foreach (var data in dataMap)
        {
            Console.WriteLine($"""
                               {new string('-', 80)}
                               {data.Key} = {data.Value.IsValidData()}
                               {new string('-', 80)}
                               """);
        }
    }

    private static List<string> GetWayRobots(StreamReader input)
    {
        var counts = input.ReadLine().Split(' ');
        var n = int.Parse(counts[0]);
        var m = int.Parse(counts[1]);
        var map = new char[n][];
        Robot robotA = null;
        Robot robotB = null;
        for (var i = 0; i < n; i++)
        {
            map[i] = new char[m];
            var line = input.ReadLine();
            for (var j = 0; j < m; j++)
            {
                map[i][j] = line[j];
                if (line[j] == 'A')
                {
                    robotA = new Robot((i, j), 'a');
                }

                if (line[j] == 'B')
                {
                    robotB = new Robot((i, j), 'b');
                }
            }
        }
        
        return RouteBuilder.BuildRoutes(map, robotA, robotB);
    }

    private static void Solution()
    {
        using var input = new StreamReader(Console.OpenStandardInput());
        using var output = new StreamWriter(Console.OpenStandardOutput());

        var inputCount = Convert.ToInt32(input.ReadLine());
        var outputList = new List<string>();
        for (var i = 0; i < inputCount; i++)
        {
            outputList.AddRange(GetWayRobots(input));
        }

        foreach (var outputValue in outputList)
        {
            output.WriteLine(outputValue);
        }
    }
}