using System;
using System.Collections.Generic;

class Comment
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }
}

class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Length { get; set; } // Length in seconds
    private List<Comment> comments;

    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return comments.Count;
    }

    public List<Comment> GetComments()
    {
        return comments;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create a list to hold videos
        List<Video> videos = new List<Video>();

        // Create videos and add comments
        Video video1 = new Video("Understanding Abstraction in C#", "John Doe", 300);
        video1.AddComment(new Comment("Alice", "Great explanation!"));
        video1.AddComment(new Comment("Bob", "I learned a lot from this."));
        video1.AddComment(new Comment("Charlie", "Thanks for the tips!"));
        videos.Add(video1);

        Video video2 = new Video("OOP Principles Explained", "Jane Smith", 450);
        video2.AddComment(new Comment("David", "Very informative!"));
        video2.AddComment(new Comment("Eve", "I love the examples."));
        videos.Add(video2);

        Video video3 = new Video("C# for Beginners", "Mike Johnson", 600);
        video3.AddComment(new Comment("Frank", "This is exactly what I needed."));
        video3.AddComment(new Comment("Grace", "Well done!"));
        video3.AddComment(new Comment("Hannah", "Looking forward to more videos."));
        videos.Add(video3);

        // Display video information
        foreach (var video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.Length} seconds");
            Console.WriteLine($"Number of Comments: {video.GetNumberOfComments()}");
            Console.WriteLine("Comments:");
            foreach (var comment in video.GetComments())
            {
                Console.WriteLine($"- {comment.CommenterName}: {comment.Text}");
            }
            Console.WriteLine();
        }
    }
}
