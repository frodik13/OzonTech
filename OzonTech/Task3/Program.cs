namespace OzonTech.Task3;

public class Program
{
    public static void MainX(string[] args)
    {
        using var input = new StreamReader(Console.OpenStandardInput());
        using var output = new StreamWriter(Console.OpenStandardOutput());

        var inputCount = Convert.ToInt32(input.ReadLine());
        var outputList = new List<int>();
        for (var i = 0; i < inputCount; i++)
        {
            //outputList.Add(GetVirusFilesCount(input));
        }

        foreach (var outputValue in outputList)
        {
            output.WriteLine(outputValue);
        }
    }
    
    private void Solution()
    {
        using var input = new StreamReader(Console.OpenStandardInput());
        using var output = new StreamWriter(Console.OpenStandardOutput());

        var inputCount = Convert.ToInt32(input.ReadLine());
        var outputList = new List<int>();
        for (var i = 0; i < inputCount; i++)
        {
            //outputList.Add(GetVirusFilesCount(input));
        }

        foreach (var outputValue in outputList)
        {
            output.WriteLine(outputValue);
        }
    }
    
    private static void Test()
    {
        var files = Directory.GetFiles(Path.Combine("OrderPlanner", "order-planner"));
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
                //outputList.Add(GetVirusFilesCount(input));
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
}