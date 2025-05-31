using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MindfulnessProgram
{
    abstract class MindfulnessActivity
    {
        protected int DurationSeconds;
        private static readonly int PreparePauseSeconds = 3;
        private static readonly int EndPauseSeconds = 3;

        protected MindfulnessActivity(int duration)
        {
            DurationSeconds = duration;
        }

        public void Start()
        {
            Console.Clear();
            ShowStartMessage();
            Console.WriteLine("Get ready...");
            PauseWithSpinner(PreparePauseSeconds);
            PerformActivity();
            ShowEndMessage();
        }

        protected virtual void ShowStartMessage()
        {
            Console.WriteLine($"--- {GetType().Name} ---");
            Console.WriteLine(GetDescription());
            Console.WriteLine($"Duration: {DurationSeconds} seconds.");
            Console.WriteLine();
        }

        protected virtual void ShowEndMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Good job! You have completed the activity.");
            Console.WriteLine($"You have spent {DurationSeconds} seconds on the {GetType().Name}.");
            PauseWithSpinner(EndPauseSeconds);
        }

        protected abstract string GetDescription();

        protected abstract void PerformActivity();

        protected void PauseWithSpinner(int seconds)
        {
            string[] spinner = { "|", "/", "-", "\\" };
            int spinnerIndex = 0;
            DateTime endTime = DateTime.Now.AddSeconds(seconds);
            while (DateTime.Now < endTime)
            {
                Console.Write(spinner[spinnerIndex]);
                spinnerIndex = (spinnerIndex + 1) % spinner.Length;
                Thread.Sleep(250);
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
            Console.Write(" ");
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            Console.WriteLine();
        }

        protected void PauseWithCountdown(int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                Console.Write($"\r{i} ");
                Thread.Sleep(1000);
            }
            Console.Write("\r  \r");
        }
    }

    class BreathingActivity : MindfulnessActivity
    {
        public BreathingActivity(int duration) : base(duration) { }

        protected override string GetDescription()
        {
            return "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.";
        }

        protected override void PerformActivity()
        {
            DateTime endTime = DateTime.Now.AddSeconds(DurationSeconds);
            while (DateTime.Now < endTime)
            {
                Console.WriteLine("\nBreathe in...");
                AnimateBreath(4);
                Console.WriteLine("Breathe out...");
                AnimateBreath(4);
            }
        }

        private void AnimateBreath(int seconds)
        {
            for (int i = 1; i <= seconds; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth / 2 - i));
                Console.WriteLine("O");
                Thread.Sleep(1000);
            }
            for (int i = seconds; i >= 1; i--)
            {
                Console.Write(new string(' ', Console.WindowWidth / 2 - i));
                Console.WriteLine("O");
                Thread.Sleep(1000);
            }
        }
    }

    class ReflectionActivity : MindfulnessActivity
    {
        private static List<string> Prompts = new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };

        private static List<string> Questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        };

        private Random random = new Random();

        public ReflectionActivity(int duration) : base(duration) { }

        protected override string GetDescription()
        {
            return "This activity will help you reflect on times in your life when you have shown strength and resilience.";
        }

        protected override void PerformActivity()
        {
            Console.WriteLine("\n" + GetRandomPrompt(Prompts));
            DateTime endTime = DateTime.Now.AddSeconds(DurationSeconds);
            while (DateTime.Now < endTime)
            {
                string question = GetRandomQuestion(Questions);
                Console.WriteLine($"\n{question}");
                PauseWithSpinner(5);
            }
        }

        private string GetRandomPrompt(List<string> prompts)
        {
            if (prompts.Count == 0) return "No more prompts available.";
            int index = random.Next(prompts.Count);
            string prompt = prompts[index];
            prompts.RemoveAt(index); // Remove to avoid repetition
            return prompt;
        }

        private string GetRandomQuestion(List<string> questions)
        {
            if (questions.Count == 0) return "No more questions available.";
            int index = random.Next(questions.Count);
            string question = questions[index];
            questions.RemoveAt(index); // Remove to avoid repetition
            return question;
        }
    }

    class ListingActivity : MindfulnessActivity
    {
        private static List<string> Prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };
        private Random random = new Random();

        public ListingActivity(int duration) : base(duration) { }

        protected override string GetDescription()
        {
            return "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.";
        }

        protected override void PerformActivity()
        {
            string prompt = GetRandomPrompt(Prompts);
            Console.WriteLine("\n" + prompt);
            Console.WriteLine("You will have 5 seconds to think...");
            PauseWithCountdown(5);

            Console.WriteLine("Start listing items. Enter each item and press Enter. Type 'done' to finish early.");
            List<string> items = new List<string>();
            DateTime endTime = DateTime.Now.AddSeconds(DurationSeconds);

            while (DateTime.Now < endTime)
            {
                if (Console.KeyAvailable)
                {
                    string input = Console.ReadLine();
                    if (input.Trim().ToLower() == "done")
                        break;
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        items.Add(input.Trim());
                    }
                }
                else
                {
                    // No input available, small wait
                    Thread.Sleep(100);
                }
            }

            Console.WriteLine($"\nYou listed {items.Count} items.");
        }

        private string GetRandomPrompt(List<string> prompts)
        {
            if (prompts.Count == 0) return "No more prompts available.";
            int index = random.Next(prompts.Count);
            string prompt = prompts[index];
            prompts.RemoveAt(index); // Remove to avoid repetition
            return prompt;
        }
    }

    class GratitudeActivity : MindfulnessActivity
    {
        private static List<string> Prompts = new List<string>
        {
            "What is something that made you smile today?",
            "Who is someone you are grateful for?",
            "What is a lesson you learned recently?",
            "What is a small thing that you appreciate?",
            "What is a memory that brings you joy?"
        };

        private Random random = new Random();

        public GratitudeActivity(int duration) : base(duration) { }

        protected override string GetDescription()
        {
            return "This activity will help you reflect on the things you are grateful for.";
        }

        protected override void PerformActivity()
        {
            Console.WriteLine("\n" + GetRandomPrompt(Prompts));
            DateTime endTime = DateTime.Now.AddSeconds(DurationSeconds);
            while (DateTime.Now < endTime)
            {
                Console.WriteLine("Take a moment to think about it...");
                PauseWithSpinner(5);
            }
        }

        private string GetRandomPrompt(List<string> prompts)
        {
            if (prompts.Count == 0) return "No more prompts available.";
            int index = random.Next(prompts.Count);
            string prompt = prompts[index];
            prompts.RemoveAt(index); // Remove to avoid repetition
            return prompt;
        }
    }

    class Program
    {
        private static Dictionary<string, int> activityLog = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            LoadActivityLog();
            Console.WriteLine("Welcome to the Mindfulness Program!");

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Breathing Activity");
                Console.WriteLine("2. Reflection Activity");
                Console.WriteLine("3. Listing Activity");
                Console.WriteLine("4. Gratitude Activity");
                Console.WriteLine("5. View Activity Log");
                Console.WriteLine("6. Save Activity Log");
                Console.WriteLine("7. Exit");
                Console.Write("Select an activity by entering the number: ");

                string choice = Console.ReadLine();
                if (choice == "7")
                {
                    Console.WriteLine("Thank you for using the Mindfulness Program. Goodbye!");
                    break;
                }

                int duration = 0;
                Console.Write("Enter the duration of the activity in seconds: ");
                while (!int.TryParse(Console.ReadLine(), out duration) || duration <= 0)
                {
                    Console.Write("Please enter a positive integer for the duration in seconds: ");
                }

                MindfulnessActivity activity = null;
                switch (choice)
                {
                    case "1":
                        activity = new BreathingActivity(duration);
                        break;
                    case "2":
                        activity = new ReflectionActivity(duration);
                        break;
                    case "3":
                        activity = new ListingActivity(duration);
                        break;
                    case "4":
                        activity = new GratitudeActivity(duration);
                        break;
                    case "5":
                        ViewActivityLog();
                        continue;
                    case "6":
                        SaveActivityLog();
                        continue;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        continue;
                }

                activity.Start();
                LogActivity(activity.GetType().Name);
            }
        }

        private static void LogActivity(string activityName)
        {
            if (activityLog.ContainsKey(activityName))
            {
                activityLog[activityName]++;
            }
            else
            {
                activityLog[activityName] = 1;
            }
        }

        private static void ViewActivityLog()
        {
            Console.WriteLine("\nActivity Log:");
            foreach (var entry in activityLog)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value} times");
            }
        }

        private static void SaveActivityLog()
        {
            using (StreamWriter writer = new StreamWriter("activity_log.txt"))
            {
                foreach (var entry in activityLog)
                {
                    writer.WriteLine($"{entry.Key}: {entry.Value}");
                }
            }
            Console.WriteLine("Activity log saved to activity_log.txt.");
        }

        private static void LoadActivityLog()
        {
            if (File.Exists("activity_log.txt"))
            {
                using (StreamReader reader = new StreamReader("activity_log.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(':');
                        if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int count))
                        {
                            activityLog[parts[0].Trim()] = count;
                        }
                    }
                }
            }
        }
    }
}
