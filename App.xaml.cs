using Righthere_Demo.Data;
using Righthere_Demo.Models;

namespace Righthere_Demo;

public partial class App : Application
{

	public static UserDatabase UserDB;
	public static Users User;

	public App()
	{
		InitializeComponent();

		UserDB = new UserDatabase();
		User = new Users();

		MainPage = new NavigationPage(new Views.LoginPage());
	}
}
