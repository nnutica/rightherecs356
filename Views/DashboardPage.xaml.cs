using Microcharts;
using SkiaSharp;
using Righthere_Demo.Data;
using Righthere_Demo.Models;
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

		var diaries = await _diaryDb.GetDiariesByUserAsync(App.User.Userid);

		if (diaries.Count == 0)
		{
			await DisplayAlert("Info", "No diary entries found.", "OK");
			return;
		}

		var moodScores = diaries.Select(d => ConvertMoodToScore(d.Mood)).ToList();
		var averageMood = moodScores.Average();

		AverageMoodLabel.Text = $"Average Mood Score: {averageMood:F2}";
		var chart = new DonutChart
		{
			Entries = new List<ChartEntry>
			{
				new ChartEntry((float)averageMood)
				{
					Label = "Mood",
					ValueLabel = averageMood.ToString("F2"),
					Color = SKColor.Parse("#3498db")
				},
			},
			HoleRadius = 0.6f
		};

		MoodChart.Chart = chart;
	}
	private int ConvertMoodToScore(string mood)
	{
		return mood switch
		{
			"Happy" => 5,
			"Good" => 4,
			"Neutral" => 3,
			"Bad" => 2,
			"Sad" => 1,
			_ => 3
		};
	}

}