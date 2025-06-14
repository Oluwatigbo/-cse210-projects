using System;
using System.Collections.Generic;

public abstract class Activity
{
    private DateTime _date;
    private int _minutes;

    public Activity(DateTime date, int minutes)
    {
        _date = date;
        _minutes = minutes;
    }

    protected DateTime Date => _date;
    protected int Minutes => _minutes;

    public abstract double GetDistance();
    public abstract double GetSpeed();
    public abstract double GetPace();

    public string GetSummary()
    {
        return $"{_date:dd MMM yyyy} {GetType().Name} ({_minutes} min) - Distance: {GetDistance():F1}, Speed: {GetSpeed():F1}, Pace: {GetPace():F1}";
    }
}

public class Running : Activity
{
    private double _distance; // in miles

    public Running(DateTime date, int minutes, double distance) : base(date, minutes)
    {
        _distance = distance;
    }

    public override double GetDistance() => _distance;

    public override double GetSpeed() => (_distance / Minutes) * 60; // mph

    public override double GetPace() => Minutes / _distance; // min per mile
}

public class Cycling : Activity
{
    private double _speed; // in mph

    public Cycling(DateTime date, int minutes, double speed) : base(date, minutes)
    {
        _speed = speed;
    }

    public override double GetDistance() => (_speed * Minutes) / 60; // miles

    public override double GetSpeed() => _speed; // mph

    public override double GetPace() => 60 / _speed; // min per mile
}

public class Swimming : Activity
{
    private int _laps;

    public Swimming(DateTime date, int minutes, int laps) : base(date, minutes)
    {
        _laps = laps;
    }

    public override double GetDistance() => (_laps * 50) / 1000.0; // km

    public override double GetSpeed() => (GetDistance() / Minutes) * 60; // kph

    public override double GetPace() => Minutes / GetDistance(); // min per km
}

public class Program
{
    public static void Main(string[] args)
    {
        List<Activity> activities = new List<Activity>
        {
            new Running(new DateTime(2022, 11, 3), 30, 3.0),
            new Cycling(new DateTime(2022, 11, 4), 45, 15.0),
            new Swimming(new DateTime(2022, 11, 5), 30, 20)
        };

        foreach (var activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}

