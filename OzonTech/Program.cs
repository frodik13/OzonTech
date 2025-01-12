namespace OzonTech;

public class Program
{
    static void Main(string[] args)
    {
        var files = Directory.GetFiles("validate-output");

        var dataMap = new Dictionary<int, Data>();

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
                    dataMap.TryAdd(Convert.ToInt32(fileName), new Data() { Output = lines.ToList() });
                }
                continue;
            }

            Console.WriteLine(file);

            using var input = new StreamReader(new FileStream(file, FileMode.Open));
            using var output = new StreamWriter(Console.OpenStandardOutput());

            var outputList = new List<string>();
            var inputCount = Convert.ToInt32(input.ReadLine());

            for (var i = 0; i < inputCount; i++)
            {
                var validator = new Validator();

                var countNumber = input.ReadLine();
                var inputNumbers = input.ReadLine();
                var outputNumbers = input.ReadLine();

                var result = validator.Validate(countNumber, inputNumbers, outputNumbers);
                outputList.Add(result);
            }

            foreach (var outputStr in outputList)
            {
                output.WriteLine(outputStr);
            }

            if (dataMap.TryGetValue(Convert.ToInt32(fileName), out var data))
            {
                data.Input = outputList;
            }
            else
            {
                dataMap.TryAdd(Convert.ToInt32(fileName), new Data() { Input = outputList });
            }
        }

        foreach (var data in dataMap)
        {
            Console.WriteLine(data.Key);
            var result = true;
            for (var i = 0; i < data.Value.Input.Count; i++)
            {
                if (data.Value.Input[i] == data.Value.Output[i]) continue;
                result = false;
                break;
            }

            Console.WriteLine(result);
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

public class Validator
{
    private const string Yes = "yes";
    private const string No = "no";

    public string Validate(string? countNumberStr, string? inputNumbersStr, string? outputNumbersStr)
    {
        if (countNumberStr == null || inputNumbersStr == null || outputNumbersStr == null)
            throw new NullReferenceException();
        
        var countNumber = Convert.ToInt32(countNumberStr);
        var inputNumbersStrList = inputNumbersStr.Split(" ");
        var outputNumberStrList = outputNumbersStr.Split(" ");

        if (inputNumbersStrList.Length != countNumber || outputNumberStrList.Length != countNumber)
            return No;

        var inputNumberList = new List<int>();
        var outputNumberList = new List<int>();
        for (var i = 0; i < countNumber; i++)
        {
            if (!int.TryParse(inputNumbersStrList[i], out var number))
                return No;
            inputNumberList.Add(number);

            if (!int.TryParse(outputNumberStrList[i], out var outputNumber))
                return No;
            outputNumberList.Add(outputNumber);
        }

        inputNumberList.Sort();

        return inputNumberList.SequenceEqual(outputNumberList) ? Yes : No;
        for (var i = 0; i < countNumber; i++)
        {
            if (inputNumberList[i] != outputNumberList[i])
                return No;
        }

        return Yes;
    }
}

public class Data
{
    public List<string> Input { get; set; }
    public List<string> Output { get; set;}
}