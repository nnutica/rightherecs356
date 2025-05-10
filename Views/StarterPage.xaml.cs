using System;
using Righthere_Demo.Models;


namespace Righthere_Demo.Views;

public partial class StarterPage : ContentPage
{
	private Users _currentUser;

	public StarterPage(Users user)
	{
		InitializeComponent();
		_currentUser = user;
		welcomeLabel.Text = $"Welcome, {_currentUser.Username}!";
	}

	private async void OnLogoutClicked(object sender, EventArgs e)
	{
		bool answer = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
		if (!answer)
			return;

		App.User = null;
		App.Current.MainPage = new NavigationPage(new LoginPage());
	}
}