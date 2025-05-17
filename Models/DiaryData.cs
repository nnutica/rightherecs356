using System;
using SQLite;

namespace Righthere_Demo.Models;

public class DiaryData
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int UserId { get; set; }
    public string Reason { get; set; }
    public string Content { get; set; }
    public string Mood { get; set; }
    public double SentimentScore { get; set; }
    public string Suggestion { get; set; }
    public string Keywords { get; set; }
    public string EmotionalReflection { get; set; }

    public DateTime CreatedAt { get; set; }
}
