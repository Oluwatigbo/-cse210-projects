using System;

class Program
{
    static void Main(string[] args)
    {
        Journal journal = new Journal();
        bool running = true;

        while (running)
        {
            Console.WriteLine("Journal Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display journal");
            Console.WriteLine("3. Save journal to file");
            Console.WriteLine("4. Load journal from file");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // Get a random prompt from the journal and display it to the user
                    string prompt = journal.GetRandomPrompt();
                    Console.WriteLine($"Prompt: {prompt}"); // Show the prompt to the user
                    Console.Write("Enter your response: ");
                    string response = Console.ReadLine();
                    journal.AddEntry(prompt, response); // Add the entry with the prompt and response
                    break;
                case "2":
                    journal.DisplayEntries();
                    break;
                case "3":
                    Console.Write("Enter filename to save: ");
                    string saveFilename = Console.ReadLine();
                    journal.SaveToFile(saveFilename);
                    break;
                case "4":
                    Console.Write("Enter filename to load: ");
                    string loadFilename = Console.ReadLine();
                    journal.LoadFromFile(loadFilename);
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}

// Exceeding Requirements:
// 1. Added a method to get a random prompt and display it to the user before they enter their response.
// 2. Improved user experience by ensuring that the prompt is visible, allowing for more thoughtful responses.
// 3. Consider implementing data validation for user inputs to ensure non-empty responses.
// 4. Future enhancements could include saving entries in a CSV format for easier data manipulation in Excel.
// 5. Potential integration with a database for persistent storage of journal entries, allowing for more complex queries and data management.
