using System;
using System.Linq;

namespace Day2Dive
{
    class Program
    {
        private static List<(string, int)> navigationPositions = new List<(string, int)>();
        static void Main(string[] args)
        {

            // Read Data into Array
            // string data = @"TestData.txt";
            string data = @"NavigationData.txt";
            navigationPositions = File.ReadAllLines(data)
                .Select(line => line.Split(' '))                // Split the line into an array.
                .Select(line => (line[0], int.Parse(line[1])))  // Add line to tupled list.
                .ToList();

            // Day 2a: 
            // Loop each navigation point to calculate the horizontal position and depth,
            // then multiple these values to calculate the answer.

            int horizontalPosition = 0;
            int depth = 0;

            foreach ((string Direction, int Units)navigationPosition in navigationPositions)
            {
                switch (navigationPosition.Direction) 
                {
                case "forward":
                    horizontalPosition += navigationPosition.Units;
                    break;
                case "down":
                    depth += navigationPosition.Units;
                    break;
                case "up":
                    depth -= navigationPosition.Units;
                    break;
                }

            }

            Console.WriteLine("--- Day 2: Dive! ---");
            Console.WriteLine("Moved forward by {0} positions.", horizontalPosition);
            Console.WriteLine("And to a depth of {0}", depth);

            // Calculate the answer.
            Console.WriteLine("These values multipled are: {0}\n", horizontalPosition * depth);

            // Day 2b
            // Track a third value, aim. The up and down actions does not affect the depth directly,
            // but rather affects the aim of the submarine. The forward action will then determine 
            // the position in a forward diagonal direction based on the aim.
            // The depth is calculated by multiplying the aim with the forward units.

            int horizontalPositionPartTwo = 0;
            int depthPartTwo = 0;

            int aim = 0; 

            foreach ((string Direction, int Units)navigationPosition in navigationPositions)
            {
                switch (navigationPosition.Direction) 
                {
                case "forward":
                    horizontalPositionPartTwo += navigationPosition.Units;
                    depthPartTwo += navigationPosition.Units * aim;
                    break;
                case "down":
                    aim += navigationPosition.Units;
                    break;
                case "up":
                    aim -= navigationPosition.Units;
                    break;
                }

            }

            Console.WriteLine("--- Part Two ---");
            Console.WriteLine("Moved forward by {0} positions.", horizontalPositionPartTwo);
            Console.WriteLine("And to a depth of: {0}", depthPartTwo);

            // Calculate the answer.
            Console.WriteLine("These values multipled are: {0}", horizontalPositionPartTwo * depthPartTwo);

        }
    }
}
