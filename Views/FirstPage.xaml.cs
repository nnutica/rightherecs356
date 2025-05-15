using System.Threading.Tasks;

namespace Righthere_Demo.Views;

public partial class FirstPage : ContentPage
{
	public FirstPage()
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);

	}

	private async void StartButton_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new LoginPage());
	}
}