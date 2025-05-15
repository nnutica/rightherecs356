using System;
using Righthere_Demo.Models;

namespace Righthere_Demo.Views;

public partial class StarterPage : ContentPage
{
	private bool _isInitialized = false; // ป้องกัน InitializeAsync ซ้ำ

	public StarterPage(Users users)
	{
		InitializeComponent();
		_ = InitializeAsync(); // เรียกแบบไม่รอ
	}

	private async Task InitializeAsync()
	{
		if (_isInitialized) return;
		_isInitialized = true;

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
		else if (Navigation.NavigationStack.LastOrDefault()?.GetType() != typeof(LoginPage))
		{
			// หากไม่มี UserId และยังไม่ได้อยู่หน้า LoginPage → ไปหน้า Login
			await Navigation.PushAsync(new LoginPage());
		}
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();



		// เรียก initialize อีกครั้ง (จะไม่ซ้ำเพราะมี _isInitialized)
		await InitializeAsync();

		if (App.User == null)
			return;

		// ดึงไดอารี่ล่าสุดของ user
		var diaries = await App.DiaryDB.GetDiariesByUserAsync(App.User.Userid);

		if (diaries.Any())
		{
			var lastDiary = diaries.OrderByDescending(d => d.CreatedAt).FirstOrDefault();

			if (lastDiary != null)
			{
				string mood = lastDiary.Mood?.ToLower() ?? "";

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
			MoodTreeImage.Source = ImageSource.FromFile("empty.png");
		}
	}

	private CancellationTokenSource _animationTokenSource;

	private async void StartTreeAnimation()
	{
		_animationTokenSource = new CancellationTokenSource();
		var token = _animationTokenSource.Token;

		try
		{
			while (!token.IsCancellationRequested)
			{
				await MoodTreeImage.RotateTo(5, 500, Easing.SinInOut);
				await MoodTreeImage.RotateTo(-5, 500, Easing.SinInOut);
			}
		}
		catch (TaskCanceledException)
		{
			// ปลอดภัยเมื่อ animation ถูกยกเลิก
		}
	}

	private async void OnLogoutClicked(object sender, EventArgs e)
	{
		bool answer = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
		if (!answer)
			return;

		App.User = null;
		SecureStorage.Remove("UserId");

		// Reset Navigation Stack กลับไปหน้า Login
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
		await Navigation.PushAsync(new DiaryHistoryPage());
	}

	private async void OnTreePageClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new TreePage());
	}

	private async void OnDiaryPageClicked(object sender, EventArgs e)
	{
		if (App.User == null)
		{
			await DisplayAlert("Error", "User session expired", "OK");
			await Navigation.PushAsync(new LoginPage());
			return;
		}
		await Navigation.PushAsync(new ColorPage(App.User));
	}

	private async void OnDashboardClicked(object sender, EventArgs e)
	{
		if (App.User == null)
		{
			await DisplayAlert("Error", "User session expired", "OK");
			await Navigation.PushAsync(new LoginPage());
			return;
		}
		await Navigation.PushAsync(new DashboardPage());
	}
}
