using SQLite;
using Righthere_Demo.Models;
using System;

namespace Righthere_Demo.Data;

public class DiaryDatabase
{
    SQLiteAsyncConnection _database;

    async Task Init()
    {
        if (_database != null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await _database.CreateTableAsync<DiaryData>();
    }

    public async Task<int> SaveDiaryAsync(DiaryData diary)
    {
        await Init();
        if (diary.Id != 0)
            return await _database.UpdateAsync(diary);
        return await _database.InsertAsync(diary);
    }

    public async Task<List<DiaryData>> GetDiariesByUserAsync(int userId)
    {
        await Init();
        return await _database.Table<DiaryData>().Where(d => d.UserId == userId).OrderByDescending(d => d.CreatedAt).ToListAsync();
    }

    public async Task<int> DeleteDiaryAsync(DiaryData diary)
    {
        await Init();
        return await _database.DeleteAsync(diary);
    }

}
