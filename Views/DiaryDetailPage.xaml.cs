namespace Righthere_Demo.Views;

using Righthere_Demo.Models;
using Righthere_Demo.Data;

public partial class DiaryDetailPage : ContentPage
{
	public DiaryDetailPage(DiaryData diary)
	{
		InitializeComponent();

		Emotiontext.Text = diary.EmotionalReflection ?? "-";
		Keywordtext.Text = diary.Keywords ?? "-";
		Moodtex.Text = diary.Mood ?? "-";
		Advicetext.Text = diary.Suggestion ?? "-";

		EmotionImage.Source = "default.png";
	}




}