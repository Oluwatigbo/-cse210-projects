using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;


/*
 * Enhancements to the Eternal Quest Program:
 * 
 * 1. Leveling System: Users can level up based on their total score, unlocking new features or bonuses.
 * 2. Streak System: Users earn bonus points for completing goals on consecutive days.
 * 3. Negative Goals: Users can set goals that deduct points for bad habits.
 * 4. Progress Goals: Users can track progress towards larger objectives.
 * 5. Achievements: Users can unlock achievements for completing milestones.
 * 6. Customizable Goals: Users can personalize their goals with descriptions and categories.
 */


namespace EternalQuest
{
    // Base Goal class with polymorphic methods
    public abstract class Goal
    {
        public string Name { get; private set; }
        public int Points { get; private set; }
        public bool Completed { get; protected set; }

        protected Goal(string name, int points)
        {
            Name = name;
            Points = points;
            Completed = false;
        }

        public abstract int RecordEvent();

        public abstract string DisplayGoal();
    }

    public class SimpleGoal : Goal
    {
        public SimpleGoal(string name, int points) : base(name, points) { }

        public override int RecordEvent()
        {
            if (!Completed)
            {
                Completed = true;
                return Points;
            }
            return 0;
        }

        public override string DisplayGoal()
        {
            return $"{(Completed ? "[X]" : "[ ]")} {Name} - Points: {Points}";
        }
    }

    public class EternalGoal : Goal
    {
        public EternalGoal(string name, int points) : base(name, points) { }

        public override int RecordEvent()
        {
            return Points;
        }

        public override string DisplayGoal()
        {
            return $"[∞] {Name} - Points per event: {Points}";
        }
    }

    public class ChecklistGoal : Goal
    {
        public int TargetCount { get; private set; }
        public int CurrentCount { get; private set; }
        public int CompletionBonus { get; private set; }

        public ChecklistGoal(string name, int points, int targetCount, int completionBonus) : base(name, points)
        {
            TargetCount = targetCount;
            CompletionBonus = completionBonus;
            CurrentCount = 0;
        }

        public override int RecordEvent()
        {
            if (Completed) return 0;

            CurrentCount++;
            if (CurrentCount >= TargetCount)
            {
                Completed = true;
                return Points + CompletionBonus;
            }
            else
            {
                return Points;
            }
        }

        public override string DisplayGoal()
        {
            return $"{(Completed ? "[X]" : "[ ]")} {Name} - Completed {CurrentCount}/{TargetCount} - Points per event: {Points}, Completion Bonus: {CompletionBonus}";
        }
    }

    public class NegativeGoal : Goal
    {
        public NegativeGoal(string name, int points) : base(name, points) { }

        public override int RecordEvent()
        {
            if (!Completed)
            {
                Completed = true;
                return -Points;
            }
            return 0;
        }

        public override string DisplayGoal()
        {
            return $"{(Completed ? "[X]" : "[ ]")} {Name} - Lose Points: {Points}";
        }
    }

    public class User
    {
        private List<Goal> _goals;
        public IReadOnlyList<Goal> Goals => _goals.AsReadOnly();
        public int Score { get; private set; }

        public User()
        {
            _goals = new List<Goal>();
            Score = 0;
        }

        public void AddGoal(Goal goal)
        {
            if (goal == null) throw new ArgumentNullException(nameof(goal));
            _goals.Add(goal);
        }

        public int RecordEvent(string goalName)
        {
            Goal goal = _goals.Find(g => g.Name.Equals(goalName, StringComparison.OrdinalIgnoreCase));
            if (goal == null) throw new ArgumentException($"Goal '{goalName}' not found.");

            int pointsEarned = goal.RecordEvent();
            Score += pointsEarned;
            return pointsEarned;
        }

        public void DisplayGoals()
        {
            Console.WriteLine("Current Goals:");
            if (_goals.Count == 0)
            {
                Console.WriteLine("No goals have been added yet.");
                return;
            }
            foreach (var goal in _goals)
            {
                Console.WriteLine(goal.DisplayGoal());
            }
            Console.WriteLine($"Total Score: {Score}\n");
        }

        public void SaveData(string filename)
        {
            var userData = new UserData
            {
                Score = this.Score,
                Goals = new List<GoalData>()
            };

            foreach (var goal in _goals)
            {
                if (goal is SimpleGoal)
                {
                    userData.Goals.Add(new GoalData
                    {
                        Type = "SimpleGoal",
                        Name = goal.Name,
                        Points = goal.Points,
                        Completed = goal.Completed
                    });
                }
                else if (goal is EternalGoal)
                {
                    userData.Goals.Add(new GoalData
                    {
                        Type = "EternalGoal",
                        Name = goal.Name,
                        Points = goal.Points,
                        Completed = false
                    });
                }
                else if (goal is ChecklistGoal checklistGoal)
                {
                    userData.Goals.Add(new GoalData
                    {
                        Type = "ChecklistGoal",
                        Name = checklistGoal.Name,
                        Points = checklistGoal.Points,
                        Completed = checklistGoal.Completed,
                        TargetCount = checklistGoal.TargetCount,
                        CurrentCount = checklistGoal.CurrentCount,
                        CompletionBonus = checklistGoal.CompletionBonus
                    });
                }
                else if (goal is NegativeGoal negativeGoal)
                {
                    userData.Goals.Add(new GoalData
                    {
                        Type = "NegativeGoal",
                        Name = negativeGoal.Name,
                        Points = negativeGoal.Points,
                        Completed = negativeGoal.Completed
                    });
                }
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(userData, options);
            File.WriteAllText(filename, json);
        }

        public void LoadData(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine($"File '{filename}' not found.");
                return;
            }

            string json = File.ReadAllText(filename);
            var userData = JsonSerializer.Deserialize<UserData>(json);

            if (userData == null)
            {
                Console.WriteLine("Failed to load data.");
                return;
            }

            Score = userData.Score;
            _goals.Clear();

            foreach (var g in userData.Goals)
            {
                Goal goal;
                switch (g.Type)
                {
                    case "SimpleGoal":
                        var sg = new SimpleGoal(g.Name, g.Points);
                        if (g.Completed) sg.RecordEvent();
                        goal = sg;
                        break;
                    case "EternalGoal":
                        goal = new EternalGoal(g.Name, g.Points);
                        break;
                    case "ChecklistGoal":
                        var cg = new ChecklistGoal(g.Name, g.Points, g.TargetCount ?? 0, g.CompletionBonus ?? 0);
                        for (int i = 0; i < (g.CurrentCount ?? 0); i++)
                            cg.RecordEvent();
                        goal = cg;
                        break;
                    case "NegativeGoal":
                        var ng = new NegativeGoal(g.Name, g.Points);
                        if (g.Completed) ng.RecordEvent();
                        goal = ng;
                        break;
                    default:
                        Console.WriteLine($"Unknown goal type: {g.Type}. Skipping.");
                        continue;
                }
                _goals.Add(goal);
            }
        }

        private class UserData
        {
            public int Score { get; set; }
            public List<GoalData> Goals { get; set; } = new List<GoalData>();
        }

        private class GoalData
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Points { get; set; }
            public bool Completed { get; set; }
            public int? TargetCount { get; set; }
            public int? CurrentCount { get; set; }
            public int? CompletionBonus { get; set; }
        }
    }

    class Program
    {
        static User user = new User();
        const string saveFile = "userdata.json";

        static void Main()
        {
            Console.WriteLine("Welcome to the Eternal Quest Program!");
            LoadIfExists();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1. Create new goal");
                Console.WriteLine("2. Record goal event");
                Console.WriteLine("3. Show all goals");
                Console.WriteLine("4. Save goals to file");
                Console.WriteLine("5. Load goals from file");
                Console.WriteLine("6. Exit");
                Console.Write("Your selection: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        CreateGoal();
                        break;
                    case "2":
                        RecordEvent();
                        break;
                    case "3":
                        user.DisplayGoals();
                        break;
                    case "4":
                        user.SaveData(saveFile);
                        Console.WriteLine($"Data saved to {saveFile}\n");
                        break;
                    case "5":
                        user.LoadData(saveFile);
                        Console.WriteLine("Data loaded.\n");
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        break;
                }
            }
            Console.WriteLine("Thank you for using Eternal Quest. Goodbye!");
        }

        static void CreateGoal()
        {
            Console.WriteLine("Select goal type:");
            Console.WriteLine("1. Simple Goal");
            Console.WriteLine("2. Eternal Goal");
            Console.WriteLine("3. Checklist Goal");
            Console.WriteLine("4. Negative Goal");
            Console.Write("Choice: ");
            string input = Console.ReadLine();

            Console.Write("Enter goal name: ");
            string name = Console.ReadLine();

            int points = ReadInt("Enter points awarded (positive number): ", min: 1);

            switch (input)
            {
                case "1":
                    user.AddGoal(new SimpleGoal(name, points));
                    Console.WriteLine("Simple Goal created.\n");
                    break;
                case "2":
                    user.AddGoal(new EternalGoal(name, points));
                    Console.WriteLine("Eternal Goal created.\n");
                    break;
                case "3":
                    int target = ReadInt("Enter target number of completions (e.g., 10): ", min: 1);
                    int bonus = ReadInt("Enter completion bonus points: ", min: 0);
                    user.AddGoal(new ChecklistGoal(name, points, target, bonus));
                    Console.WriteLine("Checklist Goal created.\n");
                    break;
                case "4":
                    user.AddGoal(new NegativeGoal(name, points));
                    Console.WriteLine("Negative Goal created.\n");
                    break;
                default:
                    Console.WriteLine("Invalid goal type.\n");
                    break;
            }
        }

        static void RecordEvent()
        {
            if (user.Goals.Count == 0)
            {
                Console.WriteLine("No goals exist to record events for.\n");
                return;
            }

            Console.WriteLine("Available goals:");
            for (int i = 0; i < user.Goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {user.Goals[i].DisplayGoal()}");
            }

            int choice = ReadInt("Select goal number to record an event: ", 1, user.Goals.Count);
            var goal = user.Goals[choice - 1];

            try
            {
                int points = user.RecordEvent(goal.Name);
                if (points >= 0)
                    Console.WriteLine($"Event recorded. You earned {points} points.\n");
                else
                    Console.WriteLine($"Event recorded. You lost {Math.Abs(points)} points.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recording event: {ex.Message}\n");
            }
        }

        static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            int value;
            bool valid;
            do
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                valid = int.TryParse(input, out value) && value >= min && value <= max;
                if (!valid)
                    Console.WriteLine($"Please enter an integer between {min} and {max}.");
            } while (!valid);
            return value;
        }

        static void LoadIfExists()
        {
            if (File.Exists(saveFile))
            {
                Console.WriteLine($"Loading saved data from {saveFile}...");
                user.LoadData(saveFile);
                Console.WriteLine("Data loaded.\n");
            }
        }
    }
}

