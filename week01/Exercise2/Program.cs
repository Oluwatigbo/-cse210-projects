using System;

class Program
{
    static void Main(string[] args)
    {
        // Ask the user for their grade percentage
        Console.Write("Enter your grade percentage: ");
        string userInput = Console.ReadLine();
        int gradePercentage;

        // Convert the input to an integer
        if (int.TryParse(userInput, out gradePercentage))
        {
            // Variable to hold the letter grade
            string letter = "";
            string sign = "";

            // Determine the letter grade
            if (gradePercentage >= 90)
            {
                letter = "A";
            }
            else if (gradePercentage >= 80)
            {
                letter = "B";
            }
            else if (gradePercentage >= 70)
            {
                letter = "C";
            }
            else if (gradePercentage >= 60)
            {
                letter = "D";
            }
            else
            {
                letter = "F";
            }

            // Determine the sign for the letter grade
            if (letter != "F" && letter != "A") // No A+ or F+/- grades
            {
                int lastDigit = gradePercentage % 10;

                if (lastDigit >= 7)
                {
                    sign = "+";
                }
                else if (lastDigit < 3)
                {
                    sign = "-";
                }
            }

            // Print the final letter grade with sign
            Console.WriteLine($"Your letter grade is: {letter}{sign}");

            // Check if the user passed the course
            if (gradePercentage >= 70)
            {
                Console.WriteLine("Congratulations! You passed the course.");
            }
            else
            {
                Console.WriteLine("Don't be discouraged! Keep trying for next time.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }
}