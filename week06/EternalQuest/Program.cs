using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    // Abstract base class for all types of goals
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

        // Record an event towards this goal, returning points earned
        public abstract int RecordEvent();

        // Display the goal status as string
        public abstract string DisplayGoal();
    }

    // SimpleGoal: completed once, then done with points awarded
    public class SimpleGoal : Goal
    {
        public SimpleGoal(string name, int points) : base(name, points)
        {
        }

        public override int RecordEvent()
        {
            if (!Completed)
            {
                Completed = true;
                return Points;
            }
            return 0; // No points if already completed
        }

        public override string DisplayGoal()
        {
            return $"{(Completed ? "[X]" : "[ ]")} {Name} - Points: {Points}";
        }
    }

    // EternalGoal: never completed, points earned every time recorded
    public class EternalGoal : Goal
    {
        public EternalGoal(string name, int points) : base(name, points)
        {
        }

        public override int RecordEvent()
        {
            // Points earned every time; goal never marked completed
            return Points;
        }

        public override string DisplayGoal()
        {
            return $"[∞] {Name} - Points per event: {Points}";
        }
    }

    // ChecklistGoal: requires N completions; gains points each time + bonus at completion
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
            if (Completed)
                return 0; // Already completed, no points

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

    // NegativeGoal: loses points for bad habits
    public class NegativeGoal : Goal
    {
        public NegativeGoal(string name, int points) : base(name, points)
        {
        }

        public override int RecordEvent()
        {
            if (!Completed)
            {
                Completed = true;
                return -Points; // Lose points for this goal
            }
            return 0; // No points if already completed
        }

        public override string DisplayGoal()
        {
            return $"{(Completed ? "[X]" : "[ ]")} {Name} - Lose Points: {Points}";
        }
    }

    // Class to manage user's goals and score
    public class User
    {
        private List<Goal> goals;
        public IReadOnlyList<Goal> Goals => goals.AsReadOnly();
        public int Score { get; private set; }
        public int Level { get; private set; }
        private int streakCount;
        private DateTime lastEventDate;

        public User()
        {
            goals = new List<Goal>();
            Score = 0;
            Level = 1;
            streakCount = 0;
            lastEventDate = DateTime.MinValue;
        }

        // Add a new goal (any derived type)
        public void AddGoal(Goal goal)
        {
            if (goal == null)
                throw new ArgumentNullException(nameof(goal));
            goals.Add(goal);
        }

        // Record an event on the goal specified by name, returning points earned
        public int RecordEvent(string goalName)
        {
            Goal goal = goals.Find(g => g.Name == goalName);
            if (goal == null)
                throw new ArgumentException($"Goal named '{goalName}' not found.");

            int pointsEarned = goal.RecordEvent();
            Score += pointsEarned;

            // Check for streaks
            if (lastEventDate.Date == DateTime.Today.AddDays(-1))
            {
                streakCount++;
                if (streakCount % 5 == 0) // Bonus every 5 days
                {
                    Score += 100; // Bonus points
                    Console.WriteLine("Bonus points earned for streak!");
                }
            }
            else if (lastEventDate.Date != DateTime.Today)
            {
                streakCount = 1; // Reset streak if not consecutive
            }

            lastEventDate = DateTime.Today;

            // Level up based on score
            Level = Score / 1000 + 1; // Level up every 1000 points
            return pointsEarned;
        }

        // Display all goals and their status
        public void DisplayGoals()
        {
            Console.WriteLine($"Level: {Level}");
            Console.WriteLine("Goals:");
            foreach (var goal in goals)
            {
                Console.WriteLine(goal.DisplayGoal());
            }
            Console.WriteLine($"Total Score: {Score}\n");
        }

        // Save user data to JSON file
        public void SaveData(string filename)
        {
            var userData = new UserData
            {
                Score = this.Score,
                Level = this.Level,
                StreakCount = this.streakCount,
                LastEventDate = this.lastEventDate,
                Goals = new List<GoalData>()
            };

            foreach (var goal in goals)
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

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(userData, jsonOptions);
            System.IO.File.WriteAllText(filename, json);
        }

        // Load user data from JSON file
        public void LoadData(string filename)
        {
            if (!System.IO.File.Exists(filename))
                throw new System.IO.FileNotFoundException($"File '{filename}' not found.");

            string json = System.IO.File.ReadAllText(filename);
            var userData = JsonSerializer.Deserialize<UserData>(json);

            if (userData == null)
                throw new Exception("Failed to deserialize user data.");

            this.Score = userData.Score;
            this.Level = userData.Level;
            this.streakCount = userData.StreakCount;
            this.lastEventDate = userData.LastEventDate;
            this.goals = new List<Goal>();

            foreach (var goalData in userData.Goals)
            {
                Goal goal;
                switch (goalData.Type)
                {
                    case "SimpleGoal":
                        var simpleGoal = new SimpleGoal(goalData.Name, goalData.Points);
                        if (goalData.Completed)
                            simpleGoal.RecordEvent(); // Mark completed
                        goal = simpleGoal;
                        break;
                    case "EternalGoal":
                        goal = new EternalGoal(goalData.Name, goalData.Points);
                        break;
                    case "ChecklistGoal":
                        var checklistGoal = new ChecklistGoal(goalData.Name, goalData.Points, goalData.TargetCount ?? 0, goalData.CompletionBonus ?? 0);
                        // Set current count and completed status
                        for (int i = 0; i < (goalData.CurrentCount ?? 0); i++)
                            checklistGoal.RecordEvent();
                        goal = checklistGoal;
                        break;
                    case "NegativeGoal":
                        goal = new NegativeGoal(goalData.Name, goalData.Points);
                        if (goalData.Completed)
                            goal.RecordEvent(); // Mark completed
                        break;
                    default:
                        throw new Exception($"Unknown goal type: {goalData.Type}");
                }
                goals.Add(goal);
            }
        }

        // Helper classes for JSON serialization
        private class UserData
        {
            public int Score { get; set; }
            public int Level { get; set; }
            public int StreakCount { get; set; }
            public DateTime LastEventDate { get; set; }
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

    // Example of usage
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User();

            user.AddGoal(new SimpleGoal("Run a marathon", 1000));
            user.AddGoal(new EternalGoal("Read scriptures", 100));
            user.AddGoal(new ChecklistGoal("Attend the temple", 50, 10, 500));
            user.AddGoal(new NegativeGoal("Skip junk food", 200)); // Negative goal

            Console.WriteLine("Initial goals and score:");
            user.DisplayGoals();

            Console.WriteLine("Recording some goals...");
            user.RecordEvent("Run a marathon");    // Should complete simple goal
            user.RecordEvent("Read scriptures");   // Points for eternal goal
            user.RecordEvent("Attend the temple"); // Checklist progress
            user.RecordEvent("Attend the temple");
            user.RecordEvent("Skip junk food");    // Lose points for negative goal

            user.DisplayGoals();

            // Save and load example
            string filename = "userdata.json";
            user.SaveData(filename);
            Console.WriteLine($"User  data saved to {filename}");

            User loadedUser = new User();
            loadedUser.LoadData(filename);
            Console.WriteLine("Loaded user data:");
            loadedUser.DisplayGoals();
        }
    }
}
