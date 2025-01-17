using System.Drawing;

namespace OzonTech.AsciiRobotTask;

public static class RouteBuilder
{
    private const char Rack = '#';
    public static List<string> BuildRoutes(char[][] map, params Robot[] robots)
    {
        var nearbyRobotIndex = 0;
        var sum = Math.Sqrt(Math.Pow(robots[0].Position.x, 2) + Math.Pow(robots[0].Position.y, 2));
        for (var i = 1; i < robots.Length; i++)
        {
            var nextSum = Math.Sqrt(Math.Pow(robots[i].Position.x, 2) + Math.Pow(robots[i].Position.y, 2));
            if (!(nextSum < sum)) continue;
            
            sum = nextSum;
            nearbyRobotIndex = i;
        }
        
        var firstRobot = robots[nearbyRobotIndex];
        BuildRouteRobot(map, firstRobot, Direction.Up);

        nearbyRobotIndex = nearbyRobotIndex == 0 ? 1 : 0;
        var nextRobot = robots[nearbyRobotIndex];
        BuildRouteRobot(map, nextRobot, Direction.Down);


        return map.Select(line => new string(line)).ToList();
    }

    private static void BuildRouteRobot(char[][] map, Robot robot, Direction direction)
    {
        var neededX = direction == Direction.Up ? 0 : map.Length - 1;
        var neededY = direction == Direction.Up ? 0 : map[0].Length - 1;
        while (robot.Position != (neededX, neededY))
        {
            var nextPosition = CalculateNextPosition(robot, map, direction);
            robot.Position = nextPosition;
            map[robot.Position.x][robot.Position.y] = robot.Symbol;
        }
    }

    private static (int x, int y) CalculateNextPosition(Robot robot, char[][] map, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                if (robot.Position.x > 0)
                {
                    return map[robot.Position.x - 1][robot.Position.y] == Rack
                        ? (robot.Position.x, robot.Position.y - 1)
                        : (robot.Position.x - 1, robot.Position.y);
                }
                
                return (robot.Position.x, robot.Position.y - 1);
            case Direction.Down:
                if (robot.Position.x < map.Length - 1)
                {
                    return map[robot.Position.x + 1][robot.Position.y] == Rack
                        ? (robot.Position.x, robot.Position.y + 1)
                        : (robot.Position.x + 1, robot.Position.y);
                }

                return (robot.Position.x, robot.Position.y + 1);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private enum Direction
    {
        Up,
        Down
    }
}
