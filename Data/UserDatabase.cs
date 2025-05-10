using System;
using Righthere_Demo.Models;
using SQLite;

namespace Righthere_Demo.Data;

public class UserDatabase
{
    SQLiteAsyncConnection _database;

    async Task Init()
    {
        if (_database != null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await _database.CreateTableAsync<Users>();
    }

    public async Task<List<Users>> GetUsersAsync()
    {
        await Init();
        return await _database.Table<Users>().ToListAsync();
    }

    public async Task<Users> GetUserAsync(int id)
    {
        await Init();
        return await _database.Table<Users>().Where(i => i.Userid == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveUserAsync(Users user)
    {
        await Init();
        if (user.Userid != 0)
            return await _database.UpdateAsync(user);
        else
            return await _database.InsertAsync(user);
    }

    public async Task<int> DeleteUserAsync(Users user)
    {
        await Init();
        return await _database.DeleteAsync(user);
    }

    public async Task<Users> GetUserByEmailAsync(string email)
    {
        await Init();
        return await _database.Table<Users>().Where(i => i.Email == email).FirstOrDefaultAsync();
    }

    public async Task<Users> GetUserByUsernameAsync(string username)
    {
        await Init();
        return await _database.Table<Users>().Where(i => i.Username == username).FirstOrDefaultAsync();
    }

    public async Task<Users> GetUserByCredentialsAsync(string Username, string password)
    {
        await Init();
        return await _database.Table<Users>().Where(i => i.Username == Username && i.Password == password)
        .FirstOrDefaultAsync();
    }

}
