using Righthere_Demo.Models;
using System;

namespace Righthere_Demo.Views;

public partial class DiaryDetailPage : ContentPage
{
	public DiaryDetailPage(DiaryData diary)
	{
		InitializeComponent();
		Emotiontext.Text = $"You felt {diary.Mood} on this day.";
		Reasontext.Text = diary.Reason;
		EmotionImage.Source = GetMoodImage(diary.Mood);
		Keywordtext.Text = diary.Keywords;
		Moodtex.Text = diary.Mood;
		Advicetext.Text = diary.Suggestion;
		scoretext.Text = diary.SentimentScore.ToString("F2");
	}
	private string GetMoodImage(string mood)
	{
		return mood switch
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