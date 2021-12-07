using System;

namespace Day3BinaryDiagnostic
{
    class Program
    {
        private static List<string> BinaryStrings = new List<string>();
        
        // Maintain lists of each ratings that mach the criteria.
        private static List<string> OxygenList = new List<string>();
        private static List<string> CO2List = new List<string>();
        
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
            Console.WriteLine("Gamma: {0}\nEpsilon: {1}", string.Join("", gamma), string.Join("", epsilon));
            Console.WriteLine("The power consumption of the submarine is : {0}", gammaInt * epsilonInt);

            // --- Part Two ---
            Console.WriteLine("\n-- Part Two --");

            // Calculate the oxygen generator rating.
            OxygenList = BinaryStrings.ToList();

            // foreach (string OxygenRating in OxygenList)
            for (int i = 0; i < binaryLength; i++) 
            {
                // If the list has one value we have the rating and we can break the loop
                if (OxygenList.Count == 1 ) break;

                // For the Oxygen rating, calculate which bit is most significant at a specific index position. 
                Dictionary<string, int> oxygenSigBits = CalculateBitSignificance(OxygenList, i, 1); 

                // Filter the list provide at an index with a specific bit(0 or 1).
                OxygenList = FilterList(OxygenList, i, oxygenSigBits["MostSignificantBit"]);

            }

            // Print the rating to the console.
            Console.WriteLine("\n-- Oxygen Generator Rating --");
            Console.WriteLine("Binary: {0}\nDecimal: {1}", OxygenList[0], Convert.ToInt32(OxygenList[0], 2));
            
            // Calculate the CO2 scrubber rating
            CO2List = BinaryStrings.ToList();

            for (int i = 0; i < binaryLength; i++) 
            {
                // If the list has one value we have the rating and we can break the loop
                if (CO2List.Count == 1 ) break;

                // For the CO2 rating, calculate which bit is least significant at a specific index position. 
                Dictionary<string, int> CO2SigBits = CalculateBitSignificance(CO2List, i, 0); 

                // Filter the list provide at an index with a specific bit(0 or 1).
                CO2List = FilterList(CO2List, i, CO2SigBits["LeastSignificantBit"]);
            }

            // Print the rating to the console.
            Console.WriteLine("\n-- CO2 scrubber rating --");
            Console.WriteLine("Binary: {0}\nDecimal: {1}", CO2List[0], Convert.ToInt32(CO2List[0], 2));


            // Multiply the values to determine the life support rating.
            Console.WriteLine("\nThe life support rating of the submarine is : {0}", Convert.ToInt32(OxygenList[0], 2) * Convert.ToInt32(CO2List[0], 2));

        }

        private static Dictionary<string, int> CalculateBitSignificance(List<string> BinaryStrings, int index, int preferred)
        {

            int binaryLength = BinaryStrings[0].Length;

            // Set some variables to keep track of the most and least significant bits
            int ones = 0;
            int zeros = 0;

            int mostSignificantBit = 0;
            int leastSignificantBit = 0;


            // Loop each line of the diagnostic data and count the ones and zeros
            foreach (string BinaryString in BinaryStrings)
            {
                if (BinaryString[index].Equals('1'))
                {
                    ones++;
                    continue;
                }
                zeros++;
                
            }

            // If there are more ones than zeros, then the most significant bit will be 1.
            if (ones > zeros)
            {
                mostSignificantBit   = 1;
                leastSignificantBit  = 0;
            }  
            // If there are more zeros than ones, then the most significant bit will be 0.
            else if(zeros > ones)
            {
                mostSignificantBit   = 0;
                leastSignificantBit  = 1;
            }
            // If the number of ones and zeros are the same, used the preferred bit for both.
            else
            {
                mostSignificantBit  = preferred;
                leastSignificantBit = preferred;
            }

            return new Dictionary<string, int>() {
                { "MostSignificantBit", mostSignificantBit }, 
                { "LeastSignificantBit", leastSignificantBit }
            };
        }
    
        private static List<string> FilterList(List<string> BinaryStrings,int index, int filterBit)
        {
            List<string> FilteredList = new List<string>() {};

            for (int i = 0; i < BinaryStrings.Count; i++) 
            {
                if (BinaryStrings[i][index].ToString() == filterBit.ToString()) 
                {
                    FilteredList.Add(BinaryStrings[i]);
                }
            }

            return FilteredList;
        }

    }
}
