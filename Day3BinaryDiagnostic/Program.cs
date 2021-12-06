using System;

namespace Day3BinaryDiagnostic
{
    class Program
    {
        private static List<string> BinaryStrings = new List<string>();
        static void Main(string[] args)
        {
            // Read Data into Array
            // string data = @"TestData.txt";
            string data = @"DiagnosticData.txt";
            BinaryStrings = File.ReadAllLines(data).ToList();

            // Just so tha the arrays we'll be working with is not static
            int binaryLength = BinaryStrings[0].Length;

            // Set some arrays to keep track of the most and least significant bits
            int[] gamma = new int[binaryLength];
            int[] epsilon = new int[binaryLength];


            // Loop each line of the diagnostic data 
            foreach (string BinaryString in BinaryStrings)
            {
                // Loop each position of the binary string's position, and
                // count number of times 1 and 0 is seen for each position.
                for (int i = 0; i < binaryLength ; i++) 
                {
                    if (BinaryString[i].Equals('1'))
                    {
                        gamma[i]++;
                        continue;
                    }
                    epsilon[i]++;
                }
            }

            // Determine if the position is should be 1 or 0 depending on the
            // number of times a 1 or 0 were observed. 
            for (int i = 0; i < binaryLength ; i++) 
            {
                if (gamma[i] > epsilon[i])
                {
                    gamma[i]    = 1;
                    epsilon[i]  = 0;
                    continue;
                }
                gamma[i]    = 0;
                epsilon[i]  = 1;
            }

            // Convert the array of 1's and 0's to a string and calculating the
            // decimal value from the binary value.
            int gammaInt = Convert.ToInt32(string.Join("", gamma), 2);
            int epsilonInt = Convert.ToInt32(string.Join("", epsilon), 2);

            // Multiply the values to determine the power consumption.
            Console.WriteLine("The power consumption of the submarine is : {0}", gammaInt * epsilonInt);

        }
    }
}
