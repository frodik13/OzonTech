using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OzonTech.VirusFile;

public class Program
{
    public static void Main(string[] args)
    {
        Test();
        //Solution();
    }

    private static void Solution()
    {
        using var input = new StreamReader(Console.OpenStandardInput());
        using var output = new StreamWriter(Console.OpenStandardOutput());

        var inputCount = Convert.ToInt32(input.ReadLine());
        var outputList = new List<int>();
        for (var i = 0; i < inputCount; i++)
        {
            outputList.Add(GetVirusFilesCount(input));
        }

        foreach (var outputValue in outputList)
        {
            output.WriteLine(outputValue);
        }
    }

    private static void Test()
    {
        var files = Directory.GetFiles(Path.Combine("VirusFile", "virus-files-cs"));
        var dataMap = new Dictionary<int, Data<int>>();
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            if (file.EndsWith('a'))
            {
                var lines = File.ReadAllLines(file).Select(int.Parse).ToList();
                fileName = fileName.Replace(".a", "");
                if (dataMap.TryGetValue(Convert.ToInt32(fileName), out var d))
                {
                    d.Output = [.. lines];
                }
                else
                {
                    dataMap.TryAdd(Convert.ToInt32(fileName), new Data<int> { Output = lines.ToList() });
                }
                continue;
            }
            Console.WriteLine(file);
            var stopwatch = Stopwatch.StartNew();
            using var input = new StreamReader(new FileStream(file, FileMode.Open));
            using var output = new StreamWriter(Console.OpenStandardOutput());

            var inputCount = Convert.ToInt32(input.ReadLine());
            var outputList = new List<int>();
            for (var i = 0; i < inputCount; i++)
            {
                outputList.Add(GetVirusFilesCount(input));
            }

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

            //foreach (var outputStr in outputList)
            //{
            //    output.WriteLine(outputStr);
            //}

            if (dataMap.TryGetValue(Convert.ToInt32(fileName), out var data))
            {
                data.Input = outputList;
            }
            else
            {
                dataMap.TryAdd(Convert.ToInt32(fileName), new Data<int> { Input = outputList });
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

    private static int GetVirusFilesCount(StreamReader input)
    {
        var lengthCount = Convert.ToInt32(input.ReadLine());
        var json = new StringBuilder();
        for (var j = 0; j < lengthCount; j++)
        {
            json.Append(input.ReadLine());
        }

        var gameFolder = JsonSerializer.Deserialize<Folder>(json.ToString(), new JsonSerializerOptions() { MaxDepth = 1000 });
        return AntiVirus.VirusCount(gameFolder);
    }
}

public class Folder
{
    public string dir { get; set; }
    public ICollection<string> files { get; set; }
    public ICollection<Folder> folders { get; set; }
} 