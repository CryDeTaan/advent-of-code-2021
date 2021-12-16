using System;

namespace Day5HydrothermalVenture
{
    class Program
    {
        private static int[,] _diagram = new int[,]{};
        static void Main(string[] args)
        {
            // Read Data into Array
            // string data = @"TestData.txt";
            string data = @"VentLines.txt";
            List<(int x, int y)[]> ventLines = File.ReadAllLines(data)
                .Select(line => line.Replace(" -> ", ","))
                .Select(line => line.Split(','))
                .Select(line => new[]
                        {
                            (int.Parse(line[0]), int.Parse(line[1])),
                            (int.Parse(line[2]), int.Parse(line[3]))
                        })
                .ToList();

            (int value1, int value2) maxValues = GetMaxValues(ventLines);
            _diagram = new int[maxValues.value2, maxValues.value1];

            foreach ((int x, int y)[] vent in ventLines)
            {
                char axis = GetAxis(vent);
                if (!Char.IsLetter(axis)) continue;

                int[] range = GetRange(vent, axis);
                int position = GetPosition(vent, axis);

                PlotLines(axis, position, range);
            }

            CountLineOverlap(_diagram, maxValues);
        }

        private static (int value1, int value2) GetMaxValues(List<(int x, int y)[]> ventLines)
        {
            int maxKey = 0;
            int maxValue = 0;
            for (int i = 0; i < ventLines.Count; i++)
            {
                int maxLKey = ventLines[i].Max(x => x.Item1);
                int maxLValue = ventLines[i].Max(x => x.Item2);

                maxKey = maxLKey > maxKey ? maxLKey : maxKey;
                maxValue = maxLValue > maxValue ? maxLValue : maxValue;

            }
            // +1 as we are counting from 0
            return (maxKey+1, maxValue+1);
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

        private static void CountLineOverlap(int[,] diagram, (int value1, int value2) maxValues)
        {
            int counter = 0;
            for (int y = 0; y < maxValues.value2 ; y++) 
            {
                for (int x = 0; x < maxValues.value2; x++)
                {
                    if(diagram[y, x] > 1) counter++;
                }
            }
            Console.WriteLine("The vent lines overlap {0} times.", counter);
        } 
    }
}
