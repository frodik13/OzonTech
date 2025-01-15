namespace OzonTech;

public class Data<T> where T : IComparable<T>
{
    public List<T> Input { get; set; }
    public List<T> Output { get; set; }

    public bool IsValidData()
    {
        if (Input == null || Output == null)
            return false;
        
        if (Input.Count != Output.Count)
            return false;

        for (var i = 0; i < Input.Count; i++)
            if (!Input[i].Equals(Output[i]))
            {
                Console.WriteLine($"Input and Output are different. Index = {i}");
                return false;
            }
        
        return true;
    }
}