namespace OzonTech.OrderPlanner;

public class OrderPlanner
{
    public string GetListTruck(Order[] orders, Truck[] trucks)
    {
        orders = orders.OrderBy(order => order.Arrival).ToArray();
        
        var listTruck = new int[orders.Length];
        
        foreach (var order in orders)
        {
            var trucksList = trucks
                .Where(t => t.Start <= order.Arrival && order.Arrival <= t.End && t.Capacity > 0)
                .ToList();
            if (trucksList.Count == 0)
            {
                listTruck[order.Index] = -1;
                continue;
            }
            
            trucksList.Sort(Comparison);
            trucksList[0].AddOrder();
            listTruck[order.Index] = trucksList[0].Index;
        }

        return string.Join(" ", listTruck) + " ";
    }
    
    private int Comparison(Truck truck, Truck truck1)
    {
        if (truck.Start == truck1.Start)
        {
            return truck.Index > truck1.Index ? 1 : -1;
        }
        return truck.Start > truck1.Start ? 1 : -1;
    }
}