using System;
using Righthere_Demo.Models;
using Righthere_Demo.Data;
using Righthere_Demo.Services;



namespace Righthere_Demo.Views;

public partial class LoginPage : ContentPage

{

	private readonly UserDatabase _userDb = new UserDatabase();
	public LoginPage()
	{
		InitializeComponent();


	}


	private async void OnLoginClicked(object sender, EventArgs e)
	{
		string username = usernameEntry.Text?.Trim();
		string password = passwordEntry.Text?.Trim();
		Console.WriteLine($"[Login] Username: {username}, Password: {password}");

		if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
		{
			await DisplayAlert("Error", "Please fill all fields", "OK");
			return;
		}

		string passHash = Services.HashPassword.GetHash256(password.Trim());

		App.User = await App.UserDB.GetUserByCredentialsAsync(username.Trim(), passHash);

		if (App.User == null)
		{
			await DisplayAlert("Error", "Invalid Username or Password", "OK");
			return;
		}

		App.Current.MainPage = new NavigationPage(new Views.StarterPage(user: App.User));
	}

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new RegisterPage());
	}


}