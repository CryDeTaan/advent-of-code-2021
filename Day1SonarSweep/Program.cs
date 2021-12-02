using System;
using System.IO;
using System.Linq;

namespace Day1SonarSweep
{
    class Program
    {
        private static int[] numbers = new int[] {};

        static void Main(string[] args)
        {

            // Read Data into Array
            // string data = @"TestData.txt";
            string data = @"DepthMeasurements.txt";
            numbers = File.ReadAllLines(data)
                                .Select(number => int.Parse(number))
                                .ToArray();


            // Day 1a: 
            // Loop each number in to count the number of times a depth 
            // measurement increases from the previous measurement.

            int current = numbers[0];   // The initial value.
            int depthIncreases = 0;     // Counter for the number of depth increases.

            foreach (int number in numbers)
            {
                if (number > current)
                {
                    depthIncreases++;
                }

                current = number;
            }
                        
            Console.WriteLine($"The number of times the depth increased: {depthIncreases}");


            // Day 1b
            // Using the sums of a three-measurement sliding window by counting the number 
            // of times the sum of measurements in this sliding window increases from the previous sum.

            int ThreesdepthIncreases = 0; // Counter for the number of depth increases in a sliding window.

            // Sliding window loop will have to end three index positions sooner as the window is three-measurement.
            for (int i = 0; i < numbers.Length - 3; i++) 
            {
                // Compare the two three-measurement sliding window
                if (ThreesTotal(i+1) > ThreesTotal(i))
                {
                    ThreesdepthIncreases++;
                }
            }

            Console.WriteLine($"The number of times the depth increased in a three-measurement window: {ThreesdepthIncreases}");

        }

        private static int ThreesTotal(int index)
        {
            // Return the value of the three-measurement window.
            return numbers[index] + numbers[index+1] + numbers[index+2];
        }
    }
}
