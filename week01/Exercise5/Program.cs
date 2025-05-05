using System;

namespace Exercise5
{
    class Program
    {
        static void Main(string[] args)
        {
            // Call the functions in order
            DisplayWelcome();
            string userName = PromptUserName(); // Corrected function name
            int favoriteNumber = PromptUserNumber(); // Corrected function name
            int squaredNumber = SquareNumber(favoriteNumber);
            DisplayResult(userName, squaredNumber);
        }

        static void DisplayWelcome()
        {
            Console.WriteLine("Welcome to the Program!");
        }

        static string PromptUserName() // Corrected function name
        {
            Console.Write("Please enter your name: ");
            return Console.ReadLine();
        }

        static int PromptUserNumber() // Corrected function name
        {
            Console.Write("Please enter your favorite number: ");
            // Parse the input to an integer
            return int.Parse(Console.ReadLine());
        }

        static int SquareNumber(int number)
        {
            return number * number;
        }

        static void DisplayResult(string userName, int squaredNumber)
        {
            Console.WriteLine($"{userName}, the square of your number is {squaredNumber}");
        }
    }
}