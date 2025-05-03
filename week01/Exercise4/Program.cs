using System;
using System.Collections.Generic;

namespace Exercise4
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int>();
            int input;

            Console.WriteLine("Enter a list of numbers, type 0 when finished.");

            // Collecting numbers from the user
            do
            {
                Console.Write("Enter number: ");
                input = Convert.ToInt32(Console.ReadLine());

                if (input != 0)
                {
                    numbers.Add(input);
                }

            } while (input != 0);

            // Core Requirements
            if (numbers.Count > 0)
            {
                // Compute the sum
                int sum = 0;
                foreach (int number in numbers)
                {
                    sum += number;
                }

                // Compute the average
                double average = (double)sum / numbers.Count;

                // Find the maximum number
                int max = numbers[0];
                foreach (int number in numbers)
                {
                    if (number > max)
                    {
                        max = number;
                    }
                }

                // Display results
                Console.WriteLine($"The sum is: {sum}");
                Console.WriteLine($"The average is: {average}");
                Console.WriteLine($"The largest number is: {max}");

                // Stretch Challenge
                // Find the smallest positive number
                int smallestPositive = int.MaxValue;
                foreach (int number in numbers)
                {
                    if (number > 0 && number < smallestPositive)
                    {
                        smallestPositive = number;
                    }
                }

                if (smallestPositive != int.MaxValue)
                {
                    Console.WriteLine($"The smallest positive number is: {smallestPositive}");
                }

                // Sort the list
                numbers.Sort();
                Console.WriteLine("The sorted list is:");
                foreach (int number in numbers)
                {
                    Console.WriteLine(number);
                }
            }
            else
            {
                Console.WriteLine("No numbers were entered.");
            }
        }
    }
}