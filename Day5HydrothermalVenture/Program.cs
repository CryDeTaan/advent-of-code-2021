using System;

namespace Day5HydrothermalVenture
{
    class Program
    {
        private static int[,] _diagram = new int[9,9];
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

            WriteDiagram(_diagram);
        }

        private static void WriteDiagram(int[,] diagram)
        {
            for (int y = 0; y < 9 ; y++) 
            {
                Console.WriteLine("{0, 2} {1, 2} {2, 2} {3, 2} {4, 2} {5, 2} {6, 2} {7, 2} {8, 2}",
                                   diagram[y, 0] == 0 ? "." : diagram[y, 0],
                                   diagram[y, 1] == 0 ? "." : diagram[y, 1], 
                                   diagram[y, 2] == 0 ? "." : diagram[y, 2], 
                                   diagram[y, 3] == 0 ? "." : diagram[y, 3], 
                                   diagram[y, 4] == 0 ? "." : diagram[y, 4],
                                   diagram[y, 5] == 0 ? "." : diagram[y, 5], 
                                   diagram[y, 6] == 0 ? "." : diagram[y, 6], 
                                   diagram[y, 7] == 0 ? "." : diagram[y, 7], 
                                   diagram[y, 8] == 0 ? "." : diagram[y, 8] 
                                   );
            }
            Console.WriteLine();
        } 
    }
}
