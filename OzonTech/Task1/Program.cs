namespace OzonTech.Task1;

public class Program
{
    public static void MainX(string[] args)
    {
        Test();
    }

    private void Solution()
    {
        using var input = new StreamReader(Console.OpenStandardInput());
        using var output = new StreamWriter(Console.OpenStandardOutput());

        var inputCount = Convert.ToInt32(input.ReadLine());
        var outputList = new List<string>();
        for (var i = 0; i < inputCount; i++)
        {
            outputList.AddRange(GetMinNumberOfLightsAndLocation(input));
        }

        foreach (var outputValue in outputList)
        {
            output.WriteLine(outputValue);
        }
    }
    
    private static void Test()
    {
        var files = Directory.GetFiles(Path.Combine("Task1", "dark-room"));
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
                outputList.AddRange(GetMinNumberOfLightsAndLocation(input));
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

    private static IEnumerable<string> GetMinNumberOfLightsAndLocation(StreamReader input)
    {
        var result = new List<string>();

        var sizeRoom = input.ReadLine().Split(' ');
        var n = int.Parse(sizeRoom[0]);
        var m = int.Parse(sizeRoom[1]);
        var lights = new List<(int x, int y, char direction)>();
        if (n == m && n > 1)
        {
            lights.Add((1, 1, 'R'));
            lights.Add((n, m, 'L'));
        }
        else if (n == m && n == 1)
        {
            lights.Add((1, 1, 'R'));
        }
        else if (n > m && m == 1)
        {
            lights.Add((1, 1, 'D'));
        }
        else if (n < m && n == 1)
        {
            lights.Add((1, 1, 'R'));
        }
        else
        {
            lights.Add((1, 1, 'D'));
            lights.Add((n, m, 'U'));
        }

        result.Add(lights.Count.ToString());
        foreach (var light in lights)
        {
            result.Add($"{light.x} {light.y} {light.direction}");
        }
        
        return result;
    }

    class Room
    {
        public Room(int n, int m)
        {
            N = n;
            M = m;
        }

        public int N { get; }
        public int M { get; }
    }
}