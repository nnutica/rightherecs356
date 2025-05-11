using Righthere_Demo.Data;
using Righthere_Demo.Models;


namespace Righthere_Demo.Views;

public partial class DiaryHistoryPage : ContentPage
{
	private DiaryDatabase _diaryDb;
	public DiaryHistoryPage()
	{

		InitializeComponent();
		_diaryDb = new DiaryDatabase();
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (App.User == null)
		{
			await DisplayAlert("Error", "You must be logged in to view history.", "OK");
			await Navigation.PopAsync();
			return;
		}

		var diaries = await _diaryDb.GetDiariesByUserAsync(App.User.Userid);
		DiaryListView.ItemsSource = diaries.OrderByDescending(d => d.CreatedAt);
	}
}