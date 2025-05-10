using System;
using SQLite;

namespace Righthere_Demo.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public string Userid { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime Created_at { get; set; }
}

