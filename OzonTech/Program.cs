using System.Diagnostics;

namespace OzonTech;

public class Program
{
    static void MainX(string[] args)
    {
        var files = Directory.GetFiles("validate-output");

        var dataMap = new Dictionary<int, Data<string>>();

        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            var outputInfo = new List<string>();
            if (file.EndsWith('a'))
            {
                var lines = File.ReadAllLines(file);
                fileName = fileName.Replace(".a", "");
                if (dataMap.TryGetValue(Convert.ToInt32(fileName), out var d))
                {
                    d.Output = [..lines];
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

            var outputList = new List<string>();
            var inputCount = Convert.ToInt32(input.ReadLine());
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < inputCount; i++)
            {
                var countNumber = input.ReadLine();
                var inputNumbers = input.ReadLine();
                var outputNumbers = input.ReadLine();

                var result = Validator.Validate(countNumber, inputNumbers, outputNumbers);
                outputList.Add(result);
            }

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

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

        //using var input = new StreamReader(Console.OpenStandardInput());
        //using var output = new StreamWriter(Console.OpenStandardOutput());

        //var outputList = new List<string>();
        //var inputCount = Convert.ToInt32(input.ReadLine());

        //for (var i = 0; i < inputCount; i++)
        //{
        //    var validator = new Validator();

        //    var countNumber = input.ReadLine();
        //    var inputNumbers = input.ReadLine();
        //    var outputNumbers = input.ReadLine();

        //    var result = validator.Validate(countNumber, inputNumbers, outputNumbers);
        //    outputList.Add(result);
        //}

        //foreach (var outputStr in outputList)
        //{
        //    output.WriteLine(outputStr);
        //}
    }
}

public static class Validator
{
    private const string Yes = "yes";
    private const string No = "no";

    public static string Validate(string? countNumberStr, string? inputNumbersStr, string? outputNumbersStr)
    {
        if (countNumberStr == null || inputNumbersStr == null || outputNumbersStr == null)
            return No;

        if (outputNumbersStr.StartsWith(' ') || outputNumbersStr.EndsWith(' '))
            return No;

        var countNumber = Convert.ToInt32(countNumberStr);
        var inputNumbersStrList = inputNumbersStr.Split(" ", StringSplitOptions.TrimEntries);
        var outputNumberStrList = outputNumbersStr.Split(" ");

        if (outputNumberStrList.Length != countNumber)
            return No;

        var inputNumberList = new int[countNumber];
        var outputNumberList = new int[countNumber];
        for (var i = 0; i < countNumber; i++)
        {
            if (!int.TryParse(inputNumbersStrList[i], out var number))
                return No;
            inputNumberList[i] = number;

            if (outputNumberStrList[i].StartsWith("0") || outputNumberStrList[i].StartsWith("-0"))
                return No;
            
            if (!int.TryParse(outputNumberStrList[i], out var outputNumber))
                return No;
            outputNumberList[i] = outputNumber;
        }

        //Array.Sort(inputNumberList);
        inputNumberList = inputNumberList.OrderBy(x => x).ToArray();

        for (var i = 0; i < countNumber; i++)
        {
            if (inputNumberList[i] != outputNumberList[i])
                return No;
        }

        return Yes;
    }
}