using System;
using Righthere_Demo.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using DotNetEnv;

namespace Righthere_Demo.Services;

public class SupabaseService
{
    private readonly HttpClient _client;
    private readonly string _supabaseUrl;
    private readonly string _supabaseKey;
    private readonly string _tableName = "User"; // Change this to your table name
    public SupabaseService()
    {
        Env.Load(); // Load environment variables from .env file
        _supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? throw new ArgumentNullException("SUPABASE_URL is not set in .env file.");
        _supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY") ?? throw new ArgumentNullException("SUPABASE_KEY is not set in .env file.");

        _client = new HttpClient();
        _client.BaseAddress = new Uri($"{_supabaseUrl}/rest/v1/");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _supabaseKey);
        _client.DefaultRequestHeaders.Add("apikey", _supabaseKey);
    }
    public async Task<bool> RegisterAsync(User user)
    {
        user.Created_at = DateTime.UtcNow;

        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(_tableName, content);
        return response.IsSuccessStatusCode;
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var requestUrl = $"{_tableName}?email=eq.{email}&password=eq.{password}";
        var response = await _client.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<User>>(json);
        return users.FirstOrDefault();
    }



}
