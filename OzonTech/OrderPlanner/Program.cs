namespace OzonTech.OrderPlanner;

public class Program
{
    public static void Main(string[] args)
    {
        Test();
        //Solution();
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
            
            var inputCount = int.Parse(input.ReadLine());
            var outputList = new List<string>();
            
            for (var i = 0; i < inputCount; i++)
            {
                outputList.Add(GetListOfCarsForAllOrders(input));
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

    private static string GetListOfCarsForAllOrders(StreamReader input)
    {
        var countOrders = Convert.ToInt32(input.ReadLine());
        var arrivals = input.ReadLine()!.Split(' ').Select((a, i) => new Order{Arrival = int.Parse(a), Index = i}).ToList();
        var numbersOfTrucks = int.Parse(input.ReadLine());
        var trucks = new Truck[numbersOfTrucks];
        for (var i = 0; i < numbersOfTrucks; i++)
        {
            var truckData = input.ReadLine()!.Split(' ').Select(int.Parse).ToList();
            trucks[i] = new Truck
            {
                Start = truckData[0],
                End = truckData[1],
                Capacity = truckData[2],
                Index = i + 1,
            };
        }

        return new OrderPlanner().GetListTruck(arrivals, trucks);
    }

    private static void Solution()
    {
        using var input = new StreamReader(Console.OpenStandardInput());
        using var output = new StreamWriter(Console.OpenStandardOutput());

        var inputCount = Convert.ToInt32(input.ReadLine());
        var outputList = new List<string>();
        for (var i = 0; i < inputCount; i++)
        {
            outputList.Add(GetListOfCarsForAllOrders(input));
        }

        foreach (var outputValue in outputList)
        {
            output.WriteLine(outputValue);
        }
    }
}

public class Truck
{
    public int Index { get; init; }
    public int Start { get; init; }
    public int End { get; init; }
    public int Capacity { get; set; }

    public void AddOrder()
    {
        Capacity--;
    }
}


public struct Order
{
    public int Arrival { get; init; }
    public int Index { get; init; }
}