using Righthere_Demo.Data;
using Righthere_Demo.Models;


namespace Righthere_Demo.Views;

public partial class DiaryHistoryPage : ContentPage
{
	private DiaryDatabase _diaryDb = new DiaryDatabase();
	public DiaryHistoryPage()
	{

		InitializeComponent();
		_diaryDb = new DiaryDatabase();

	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var diaries = await _diaryDb.GetDiariesByUserAsync(App.User.Userid);
		DiaryListView.ItemsSource = diaries;
	}


	private async void DiaryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is DiaryData selectedDiary)
		{
			await Navigation.PushAsync(new Views.DiaryDetailPage(selectedDiary));
			DiaryListView.SelectedItem = null;
		}
	}
}