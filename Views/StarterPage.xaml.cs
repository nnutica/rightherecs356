using System;
using Righthere_Demo.Models;


namespace Righthere_Demo.Views;

public partial class StarterPage : ContentPage
{


	public StarterPage(Users users)
	{
		InitializeComponent();
		_ = InitializeAsync();
	}

	private async Task InitializeAsync()
	{
		var userId = await SecureStorage.GetAsync("UserId");

		if (!string.IsNullOrEmpty(userId))
		{
			// ถ้ามี UserId ใน SecureStorage
			var currentUser = await App.UserDB.GetUserAsync(int.Parse(userId));

			if (currentUser != null)
			{
				// ตั้งค่าให้ _currentUser และแสดงข้อความ Welcome
				welcomeLabel.Text = $"Welcome, {currentUser.Username}!";
				dateLabel.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy", new System.Globalization.CultureInfo("th-TH"));
			}
		}
		else
		{
			// หากไม่มี UserId ใน SecureStorage
			await Navigation.PushAsync(new LoginPage()); // ถ้ามีการ logout หรือ session expired
		}
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();

		// เรียก InitializeAsync() เพื่อโหลด user info เหมือนเดิม
		await InitializeAsync();

		if (App.User == null)
			return;

		// ดึงไดอารี่ล่าสุดของ user
		var diaries = await App.DiaryDB.GetDiariesByUserAsync(App.User.Userid);

		// ถ้ามีไดอารี่
		if (diaries.Any())
		{
			// เอาไดอารี่ล่าสุด
			var lastDiary = diaries.OrderByDescending(d => d.CreatedAt).FirstOrDefault();

			if (lastDiary != null)
			{
				// หา mood ล่าสุด
				var mood = lastDiary.Mood?.ToLower() ?? "";

				// เลือกรูปตาม mood
				string imageName = mood switch
				{
					"joy" => "joy.png",
					"sadness" => "sadness.png",
					"anger" => "anger.png",
					"surprise" => "surprise.png",
					"fear" => "fear.png",
					"love" => "love.png",
					_ => "empty.png",
				};

				MoodTreeImage.Source = ImageSource.FromFile(imageName);
			}
		}
		else
		{
			// ไม่มีไดอารี่ ให้แสดงรูป empty
			MoodTreeImage.Source = ImageSource.FromFile("empty.png");
		}
	}
	private async void OnLogoutClicked(object sender, EventArgs e)
	{
		bool answer = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
		if (!answer)
			return;

		// รีเซ็ตข้อมูลผู้ใช้ใน App
		App.User = null;
		SecureStorage.Remove("UserId");

		// เปลี่ยนหน้ากลับไปที่ Login
		App.Current.MainPage = new NavigationPage(new LoginPage());
	}
	private async void OnDiaryHistoryClicked(object sender, EventArgs e)
	{
		if (App.User == null)
		{
			await DisplayAlert("Error", "User session expired", "OK");
			await Navigation.PushAsync(new LoginPage());
			return;
		}
		await Navigation.PushAsync(new DiaryHistoryPage()); // หน้านี้คุณสร้างเพิ่มเอง
	}

	private async void OnTreePageClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new TreePage()); // หน้านี้คุณสร้างเพิ่มเอง
	}

	private async void OnDiaryPageClicked(object sender, EventArgs e)
	{
		if (App.User == null)
		{
			await DisplayAlert("Error", "User session expired", "OK");
			await Navigation.PushAsync(new LoginPage());
			return;
		}

		await Navigation.PushAsync(new ColorPage(App.User)); // หน้านี้คุณสร้างเพิ่มเอง
	}
	private async void OnDashboardClicked(object sender, EventArgs e)
	{
		if (App.User == null)
		{
			await DisplayAlert("Error", "User session expired", "OK");
			await Navigation.PushAsync(new LoginPage());
			return;
		}
		await Navigation.PushAsync(new DashboardPage()); // หน้านี้คุณสร้างเพิ่มเอง
	}
}