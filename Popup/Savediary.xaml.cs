using CommunityToolkit.Maui.Views;

namespace Righthere_Demo.Popup;

public partial class Savediary : CommunityToolkit.Maui.Views.Popup
{
	public TaskCompletionSource<bool> Response { get; private set; }
	public Savediary()
	{
		InitializeComponent();
		Response = new TaskCompletionSource<bool>();
	}
	private void OnYesClicked(object sender, EventArgs e)
	{
		Response.SetResult(true);
		Close(true); // ส่งค่ากลับว่า "ใช่"
	}

	private void OnNoClicked(object sender, EventArgs e)
	{
		Response.SetResult(false);
		Close(false); // ส่งค่ากลับว่า "ไม่"
	}
}