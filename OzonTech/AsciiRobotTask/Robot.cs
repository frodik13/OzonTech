namespace OzonTech.AsciiRobotTask;

public class Robot
{
    public Robot((int x, int y) position, char symbol)
    {
        Position = position;
        Symbol = symbol;
    }

    public (int x, int y) Position { get; set; }
    public char Symbol { get; private set; }
}