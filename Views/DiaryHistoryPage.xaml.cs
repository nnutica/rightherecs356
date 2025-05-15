using Righthere_Demo.Data;
using Righthere_Demo.Models;

namespace Righthere_Demo.Views;

public partial class DiaryHistoryPage : ContentPage
{
	private DiaryDatabase _diaryDb = new DiaryDatabase();

	public DiaryHistoryPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var diaries = await _diaryDb.GetDiariesByUserAsync(App.User.Userid);

		// เรียงลำดับจากวันที่ล่าสุดลงมา
		DiaryListView.ItemsSource = diaries.OrderByDescending(d => d.CreatedAt).ToList();
	}


}
