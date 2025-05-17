using Righthere_Demo.Data;
using Righthere_Demo.Models;
using System.Collections.ObjectModel;

namespace Righthere_Demo.Views;

public partial class DiaryHistoryPage : ContentPage
{
	private DiaryDatabase _diaryDb = new DiaryDatabase();

	public ObservableCollection<DiaryGroup> GroupedDiaries { get; set; } = new();

	public DiaryHistoryPage()
	{
		InitializeComponent();
		BindingContext = this;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		SetupDateLabels();

		var allDiaries = await _diaryDb.GetDiariesByUserAsync(App.User.Userid);

		var grouped = allDiaries
			.GroupBy(d => d.CreatedAt.Date)
			.OrderByDescending(g => g.Key);

		GroupedDiaries.Clear();

		foreach (var group in grouped)
		{
			// หาค่า Mood ที่พบบ่อยที่สุดในกลุ่ม
			var dominantMood = group
				.GroupBy(d => d.Mood)
				.OrderByDescending(g => g.Count())
				.FirstOrDefault()?.Key ?? "Neutral";

			GroupedDiaries.Add(new DiaryGroup
			{
				Date = group.Key,
				Mood = dominantMood,
				Diaries = group.OrderByDescending(d => d.CreatedAt).ToList()
			});
		}




	}

	void SetupDateLabels()
	{
		DateTime monday = GetMondayOfThisWeek();
		DateTime Nextmonday = monday.AddDays(7);

		LeftDayLabel.Text = monday.Day.ToString();
		LeftMonthLabel.Text = monday.ToString("MMMM").ToUpper();

		RightDayLabel.Text = Nextmonday.Day.ToString();
		RightMonthLabel.Text = Nextmonday.ToString("MMMM").ToUpper();
	}
	DateTime GetMondayOfThisWeek()
	{
		var today = DateTime.Today;
		int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
		return today.AddDays(-diff);
	}

	private async void OnDiaryTapped(object sender, TappedEventArgs e)
	{
		if (e.Parameter is DiaryData diary)
		{
			await Navigation.PushAsync(new DiaryDetailPage(diary));
		}
	}

}

public class DiaryGroup
{
	public DateTime Date { get; set; }
	public string Mood { get; set; }
	public List<DiaryData> Diaries { get; set; }
}
