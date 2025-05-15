using Microcharts;
using SkiaSharp;
using Righthere_Demo.Models;
using Righthere_Demo.Data;
using Microcharts.Maui;

namespace Righthere_Demo.Views;

public partial class DashboardPage : ContentPage
{
	private DiaryDatabase _diaryDb;

	public DashboardPage()
	{
		InitializeComponent();
		_diaryDb = new DiaryDatabase();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (App.User == null)
		{
			await DisplayAlert("Error", "Please log in.", "OK");
			return;
		}

		var diaries = await _diaryDb.GetDiariesByUserAsync(App.User.Userid);
		if (diaries.Any())
		{
			var moodCounts = diaries
				.GroupBy(d => d.Mood)
				.ToDictionary(g => g.Key, g => g.Count());

			// Donut Chart
			var donutEntries = moodCounts.Select(mc => new ChartEntry(mc.Value)
			{
				Label = mc.Key,
				ValueLabel = mc.Value.ToString(),
				Color = GetMoodColor(mc.Key)
			}).ToList();

			DonutChart.Chart = new DonutChart
			{
				Entries = donutEntries,
				LabelTextSize = 48,
			};

			// Horizontal Bar Chart
			var barEntries = moodCounts.Select(mc => new ChartEntry(mc.Value)
			{
				Label = mc.Key,
				ValueLabel = mc.Value.ToString(),
				Color = GetMoodColor(mc.Key)
			}).ToList();

			BarChart.Chart = new BarChart
			{
				Entries = barEntries,
				LabelTextSize = 48
			};

			// Tree Logic (เช็คกรณีพิเศษ)
			var maxCount = moodCounts.Max(x => x.Value);
			var allMaxMoods = moodCounts.Where(x => x.Value == maxCount).Select(x => x.Key).ToList();

			string dominantMood = "";

			if (allMaxMoods.Count == moodCounts.Count)
			{
				// ทุก mood count เท่ากันหมด → เอา mood ล่าสุด
				var latestDiary = diaries.OrderByDescending(d => d.CreatedAt).FirstOrDefault();
				if (latestDiary != null)
					dominantMood = latestDiary.Mood;
				else
					dominantMood = "empty";
			}
			else
			{
				// มี mood เด่นชัด → ใช้ logic เดิม
				dominantMood = moodCounts.Aggregate((l, r) => l.Value <= r.Value ? r : l).Key;
			}

			SetTreeImage(dominantMood);
		}
		else
		{
			DonutChart.Chart = null;
			BarChart.Chart = null;
			TreeImage.Source = "empty.png";
		}
	}

	private SKColor GetMoodColor(string mood)
	{
		return mood.ToLower() switch
		{
			"joy" => SKColors.Gold,
			"sadness" => SKColors.DarkBlue,
			"anger" => SKColors.Red,
			"surprise" => SKColors.Gray,
			"fear" => SKColors.Purple,
			"love" => SKColors.Pink,
			_ => SKColors.Green,
		};
	}

	private void SetTreeImage(string mood)
	{
		TreeImage.Source = mood.ToLower() switch
		{
			"joy" => "joy.png",
			"sadness" => "sadness.png",
			"anger" => "anger.png",
			"surprise" => "surprise.png",
			"fear" => "fear.png",
			"love" => "love.png",
			_ => "empty.png",
		};
	}
}
