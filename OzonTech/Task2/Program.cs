namespace OzonTech.Task2;

public class Program
{
    public static void Main(string[] args)
    {
        Test();
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
        var files = Directory.GetFiles(Path.Combine("Task2", "validate-result"));
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
                outputList.Add(IsValidInput(input));
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

    static string IsValidInput(StreamReader input)
    {
        var countOrder = int.Parse(input.ReadLine());
        var orders = new SortedSet<Order>(comparer: Comparer<Order>.Create((x, y) =>
            x.Price == y.Price ? x.Name.CompareTo(y.Name) : x.Price.CompareTo(y.Price)));
        for (var i = 0; i < countOrder; i++)
        {
            var order = input.ReadLine().Split(' ');
            orders.Add(new Order(order[0], int.Parse(order[1])));
        }

        var countOrdersToOutput = orders.DistinctBy(orders => orders.Price).Count();
        var outputStr = input.ReadLine();
        var outputOrdersStr = outputStr.Split(',');
        if (outputOrdersStr.Length != countOrdersToOutput)
        {
            return "NO";
        }

        var outputOrders = new SortedSet<Order>(comparer: Comparer<Order>.Create((x, y) =>
            x.Price == y.Price ? x.Name.CompareTo(y.Name) : x.Price.CompareTo(y.Price)));

        for (var i = 0; i < countOrdersToOutput; i++)
        {
            var orderValue = outputOrdersStr[i].Split(':');
            if (orderValue.Length != 2) return "NO";
            if (!int.TryParse(orderValue[1], out var orderPrice))
            {
                return "NO";
            }
            var order = new Order(orderValue[0], orderPrice);
            if (outputOrders.Contains(order, PriceComparer))
                return "NO";
            outputOrders.Add(order);
        }

        var indexInput = 0;
        var indexOutput = 0;
        var orderNext = orders.ElementAt(indexInput);
        var orderNextPrice = orderNext.Price;
        var ordersWithPrice = orders.Where(x => x.Price == orderNextPrice).ToList();
        while (indexInput < orders.Count)
        {
            var outputOrder = outputOrders.ElementAt(indexOutput);
            if (!ordersWithPrice.Contains(outputOrder))
            {
                return "NO";
            }

            indexOutput++;
            indexInput += ordersWithPrice.Count;
            orderNext = orders.ElementAt(indexInput);
            orderNextPrice = orderNext.Price;
            ordersWithPrice = orders.Where(x => x.Price == orderNextPrice).ToList();
        }

        return "YES";
    }

    private sealed class PriceEqualityComparer : IEqualityComparer<Order>
    {
        public bool Equals(Order? x, Order? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null) return false;
            if (y is null) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Price == y.Price;
        }

        public int GetHashCode(Order obj)
        {
            return obj.Price;
        }
    }

    static IEqualityComparer<Order> PriceComparer { get; } = new PriceEqualityComparer();

    class Order
    {
        public Order(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; }
        public int Price { get; }

        

        public override string ToString()
        {
            return $"{Name}:{Price}";
        }
    }
}