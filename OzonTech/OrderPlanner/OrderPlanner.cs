namespace OzonTech.OrderPlanner;

public class OrderPlanner
{
    public string GetListTruck(Order[] orders, Truck[] trucks)
    {
        orders = orders.OrderBy(order => order.Arrival).ToArray();
        trucks = trucks.OrderBy(t => t.Start).ThenBy(t => t.Index).ToArray();

        var availableTrucks = new SortedSet<Truck>(Comparer<Truck>.Create((truck, truck1) =>
            truck.Start == truck1.Start ? truck.Index.CompareTo(truck1.Index) : truck.Start.CompareTo(truck1.Start)));
        var truckIndex = 0;
        
        var listTruck = new int[orders.Length];
        
        foreach (var order in orders)
        {
            while (truckIndex < trucks.Length && trucks[truckIndex].Start <= order.Arrival)
                availableTrucks.Add(trucks[truckIndex++]);
            
            
            while (availableTrucks.Count > 0 && availableTrucks.Min.End < order.Arrival)
                availableTrucks.Remove(availableTrucks.Min);
            
            var truck = availableTrucks.Min;
            
            if (truck == null)
            {
                listTruck[order.Index] = -1;
            }
            else
            {
                listTruck[order.Index] = truck.Index;
                truck.AddOrder();
                if (truck.Capacity == 0)
                {
                    availableTrucks.Remove(truck);
                }
            }
        }

        return string.Join(" ", listTruck) + " ";
    }
}