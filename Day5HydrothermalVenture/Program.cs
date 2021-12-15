using System;

namespace Day5HydrothermalVenture
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read Data into Array
            string data = @"TestData.txt";
            List<(int x, int y)[]> ventLines = File.ReadAllLines(data)
                .Select(line => line.Replace(" -> ", ","))
                .Select(line => line.Split(','))
                .Select(line => new []
                        {
                            (int.Parse(line[0]), int.Parse(line[1])),
                            (int.Parse(line[2]), int.Parse(line[3]))
                        })
                .ToList();

            foreach ((int x, int y)[] vent in ventLines)
            {
                Console.WriteLine("{0},{1} -> {2},{3}", vent[0].x, vent[0].y, vent[1].x, vent[1].y);
            }
        }
    }
}
