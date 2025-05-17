using Microcharts;
using SkiaSharp;
using Righthere_Demo.Models;
using Righthere_Demo.Data;
using Microcharts.Maui;
using System.Linq;

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
			// สร้าง list entries สำหรับ LineChart แสดง sentiment score ตามวันที่
			var lineEntries = diaries
				.OrderBy(d => d.CreatedAt)
				.Select(d => new ChartEntry((float)d.SentimentScore)  // สมมติ Diary มี property SentimentScore เป็น float/double
				{
					Label = d.CreatedAt.ToString("MM/dd"),    // แสดงวันที่เป็น MM/dd
					ValueLabel = d.SentimentScore.ToString("0.00"),
					Color = SKColors.Blue
				}).ToList();

			LineChart.Chart = new LineChart
			{
				Entries = lineEntries,
				LineMode = LineMode.Straight,
				LineSize = 4,
				PointMode = PointMode.Circle,
				PointSize = 10,
				ValueLabelOrientation = Orientation.Horizontal,
				LabelOrientation = Orientation.Horizontal,
				LabelTextSize = 30,
				ValueLabelTextSize = 30,
				Margin = 20
			};

			// สร้าง BarChart แบบเดิมสำหรับ mood counts
			var moodCounts = diaries
				.GroupBy(d => d.Mood)
				.ToDictionary(g => g.Key, g => g.Count());

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

			// กำหนด TreeImage ตาม mood ที่เด่น
			var maxCount = moodCounts.Max(x => x.Value);
			var allMaxMoods = moodCounts.Where(x => x.Value == maxCount).Select(x => x.Key).ToList();

			string dominantMood = "";

			if (allMaxMoods.Count == moodCounts.Count)
			{
				var latestDiary = diaries.OrderByDescending(d => d.CreatedAt).FirstOrDefault();
				dominantMood = latestDiary?.Mood ?? "empty";
			}
			else
			{
				dominantMood = moodCounts.Aggregate((l, r) => l.Value <= r.Value ? r : l).Key;
			}

			SetTreeImage(dominantMood);
		}
		else
		{
			LineChart.Chart = null;
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
