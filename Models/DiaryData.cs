using System;
using SQLite;

namespace Righthere_Demo.Models;

public class DiaryData
{
    [PrimaryKey, AutoIncrement]
    public int DiaryId { get; set; }
    public string UserId { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
}
