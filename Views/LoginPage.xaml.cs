using Righthere_Demo.Services;

namespace Righthere_Demo.Views;

public partial class LoginPage : ContentPage
{
	private readonly SupabaseService _supabaseService;
	public LoginPage()
	{
		InitializeComponent();
		_supabaseService = new SupabaseService();
	}

	private async void OnLoginClicked(object sender, EventArgs e)
	{
		string email = emailEntry.Text;
		string password = passwordEntry.Text;

		if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
		{
			await DisplayAlert("Error", "กรุณากรอกข้อมูลให้ครบถ้วน", "ตกลง");
			return;
		}

		var user = await _supabaseService.LoginAsync(email, password);

		if (user != null)
		{
			await DisplayAlert("ยินดีต้อนรับ", $"เข้าสู่ระบบในชื่อ {user.Username}", "ตกลง");
			// ไปหน้าหลัก หรือทำอย่างอื่น
		}
		else
		{
			await DisplayAlert("ล้มเหลว", "อีเมลหรือรหัสผ่านไม่ถูกต้อง", "ตกลง");
		}
	}

	private async void OnRegisterLinkTapped(object sender, TappedEventArgs e)
	{
		await Navigation.PushAsync(new RegisterPage());
	}

}