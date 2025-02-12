﻿namespace QuizApp.Domain.Models;

public class Quiz
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public string ImageUrl { get; set; }

    public ushort? Rate { get; set; }

    public uint PassCount { get; set; }

    public Guid OwnerId { get; set; }

    public uint Duration { get; set; }

    public List<Question> Questions { get; set; }

    public List<Feedback> Feedbacks { get; set; }
}
