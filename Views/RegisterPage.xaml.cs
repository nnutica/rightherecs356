using Righthere_Demo.Models;
using Righthere_Demo.Services;

namespace Righthere_Demo.Views;

public partial class RegisterPage : ContentPage
{
	private readonly SupabaseService _supabaseService;
	public RegisterPage()
	{
		InitializeComponent();
		_supabaseService = new SupabaseService();
	}

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		string username = usernameEntry.Text;
		string email = emailEntry.Text;
		string password = passwordEntry.Text;

		if (string.IsNullOrWhiteSpace(username) ||
			string.IsNullOrWhiteSpace(email) ||
			string.IsNullOrWhiteSpace(password))
		{
			await DisplayAlert("Error", "กรุณากรอกข้อมูลให้ครบถ้วน", "ตกลง");
			return;
		}

		var user = new User
		{
			Userid = Guid.NewGuid().ToString(),
			Username = username,
			Email = email,
			Password = password // ควร Hash ก่อนใช้งานจริง
		};

		var success = await _supabaseService.RegisterAsync(user);

		if (success)
		{
			await DisplayAlert("สำเร็จ", "สมัครสมาชิกเรียบร้อยแล้ว", "ตกลง");
			await Navigation.PopAsync(); // กลับไปหน้า Login
		}
		else
		{
			await DisplayAlert("ล้มเหลว", "เกิดข้อผิดพลาดในการสมัคร", "ตกลง");
		}
	}
}