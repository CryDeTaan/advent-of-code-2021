using System;

namespace Day5HydrothermalVenture
{
    class Program
    {
        private static int[,] _diagram = new int[10,10];
        static void Main(string[] args)
        {
            // Read Data into Array
            string data = @"TestData.txt";
            List<(int x, int y)[]> ventLines = File.ReadAllLines(data)
                .Select(line => line.Replace(" -> ", ","))
                .Select(line => line.Split(','))
                .Select(line => new[]
                        {
                            (int.Parse(line[0]), int.Parse(line[1])),
                            (int.Parse(line[2]), int.Parse(line[3]))
                        })
                .ToList();

            foreach ((int x, int y)[] vent in ventLines)
            {

                char axis = GetAxis(vent);
                if (!Char.IsLetter(axis)) continue;

                int[] range = GetRange(vent, axis);

                int position = GetPosition(vent, axis);
                PlotLines(axis, position , range);
            }

            WriteDiagram(_diagram);
        }

        private static int GetPosition((int x, int y)[] vent, char axis)
        {
            int position = new int { };

            if (axis == 'x') position = vent[0].x;
            if (axis == 'y') position = vent[0].y;

            return position;
        }
        private static int[] GetRange((int x, int y)[] vent, char axis)
        {
            int[] boundaries = new int[] { };

            if (axis == 'x') boundaries = boundaries.Concat( new int[] { vent[0].y, vent[1].y }).ToArray();
            if (axis == 'y') boundaries = boundaries.Concat( new int[] { vent[0].x, vent[1].x }).ToArray();

            Array.Sort(boundaries);

            int min = Math.Min(boundaries[0], boundaries[1]);
            int max = Math.Max(boundaries[0], boundaries[1]) + 1;

            List<int> rangeList = new List<int>();
            int i = min;
            while (i < max )
            {
                rangeList.Add(i);
                i++;
            }

            int[] range = rangeList.ToArray();

            return range;
        }

        private static char GetAxis((int x, int y)[] vent)
        {
            char axis = new char { };

            if (vent[0].x == vent[1].x) axis = 'x';
            if (vent[0].y == vent[1].y) axis = 'y';
            
            return axis;
        }

        private static void PlotLines(char axis, int position, int[] range)
        {
            foreach(int number in range)
            {
                if(axis == 'x') _diagram[number, position]++;
                if(axis == 'y') _diagram[position, number]++;
            }
        }

        private static void WriteDiagram(int[,] diagram)
        {
            Console.WriteLine();
            for (int y = 0; y < 10 ; y++) 
            {
                Console.WriteLine("{0, 2} {1, 2} {2, 2} {3, 2} {4, 2} {5, 2} {6, 2} {7, 2} {8, 2} {9, 2}",
                                   diagram[y, 0] == 0 ? "." : diagram[y, 0],
                                   diagram[y, 1] == 0 ? "." : diagram[y, 1], 
                                   diagram[y, 2] == 0 ? "." : diagram[y, 2], 
                                   diagram[y, 3] == 0 ? "." : diagram[y, 3], 
                                   diagram[y, 4] == 0 ? "." : diagram[y, 4],
                                   diagram[y, 5] == 0 ? "." : diagram[y, 5], 
                                   diagram[y, 6] == 0 ? "." : diagram[y, 6], 
                                   diagram[y, 7] == 0 ? "." : diagram[y, 7], 
                                   diagram[y, 8] == 0 ? "." : diagram[y, 8], 
                                   diagram[y, 9] == 0 ? "." : diagram[y, 9] 
                                   );
            }
            Console.WriteLine();
        } 
    }
}
