using Righthere_Demo.Data;
using Righthere_Demo.Models;
using Righthere_Demo.Services;


namespace Righthere_Demo.Views;

public partial class RegisterPage : ContentPage
{
	private UserDatabase _userDb = new();
	public RegisterPage()
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
	}
	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(usernameEntry.Text) ||
		string.IsNullOrEmpty(passwordEntry.Text) ||
		string.IsNullOrEmpty(ConfirmPasswordEntry.Text) ||
		string.IsNullOrEmpty(emailEntry.Text))
		{
			await DisplayAlert("Error", "Please fill all fields", "OK");
			return;
		}

		// check if password is at least 6 characters
		if (passwordEntry.Text.Length < 6)
		{
			await DisplayAlert("Error", "Password must be at least 6 characters", "OK");
			return;
		}

		// check if password and confirm password match
		if (passwordEntry.Text != ConfirmPasswordEntry.Text)
		{
			await DisplayAlert("Error", "Password and Confirm Password do not match", "OK");
			return;
		}
		// check if username is already exists
		if (await App.UserDB.GetUserByUsernameAsync(usernameEntry.Text) != null)
		{
			await DisplayAlert("Error", "Username already exists", "OK");
			return;
		}

		// check if email is already exists
		if (await App.UserDB.GetUserByEmailAsync(emailEntry.Text) != null)
		{
			await DisplayAlert("Error", "Email already exists", "OK");
			return;
		}

		CreateUser();
	}

	private async void CreateUser()
	{
		string passHash = Services.HashPassword.GetHash256(passwordEntry.Text.Trim());
		Users user = new Users();
		user.Username = usernameEntry.Text.Trim();
		user.Password = passHash;
		user.Email = emailEntry.Text.Trim();



		await App.UserDB.SaveUserAsync(user);
		Console.WriteLine($"[Register] Inserted: {user.Username}, {user.Email}, {user.Password}");
		await DisplayAlert("Success", "User created successfully", "OK");
		ClearTextFields();
		await Navigation.PopAsync();
	}

	private void ClearTextFields()
	{
		usernameEntry.Text = string.Empty;
		passwordEntry.Text = string.Empty;
		ConfirmPasswordEntry.Text = string.Empty;
		emailEntry.Text = string.Empty;
	}

	private async void OnLoginClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new Views.LoginPage());
	}




}