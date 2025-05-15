
using Microsoft.Maui.Controls;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System;
using Righthere_Demo.Models;

namespace Righthere_Demo.Views;

public partial class ColorPage : ContentPage
{
	private Users _user;
	private SKColor selectedColor = SKColors.White;


	public ColorPage(Users users)
	{
		InitializeComponent();
		_user = users;

	}
	private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
	{
		var canvas = e.Surface.Canvas;
		var width = e.Info.Width;
		var height = e.Info.Height;
		var radius = Math.Min(width, height) / 2;

		canvas.Clear(SKColors.White);

		using var paint = new SKPaint { IsAntialias = true };

		for (int angle = 0; angle < 360; angle++)
		{
			paint.Color = SKColor.FromHsv(angle, 100, 100);
			paint.StrokeWidth = 2;
			paint.Style = SKPaintStyle.Stroke;

			var radians = angle * Math.PI / 180.0;
			var x = width / 2 + radius * (float)Math.Cos(radians);
			var y = height / 2 + radius * (float)Math.Sin(radians);
			canvas.DrawLine(width / 2, height / 2, x, y, paint);
		}
	}

	private void OnCanvasTouched(object sender, SKTouchEventArgs e)
	{
		var canvasView = sender as SKCanvasView;
		var point = e.Location;

		if (canvasView == null)
			return;

		var width = (float)canvasView.CanvasSize.Width;
		var height = (float)canvasView.CanvasSize.Height;
		var centerX = width / 2;
		var centerY = height / 2;

		var dx = point.X - centerX;
		var dy = point.Y - centerY;
		var distance = Math.Sqrt(dx * dx + dy * dy);

		var radius = Math.Min(width, height) / 2;

		if (distance <= radius)
		{
			var angle = (Math.Atan2(dy, dx) * 180 / Math.PI + 360) % 360;
			selectedColor = SKColor.FromHsv((float)angle, 100, 100);

			// Update BoxView with selected color
			SelectedColorBox.BackgroundColor = Color.FromRgb(
				selectedColor.Red,
				selectedColor.Green,
				selectedColor.Blue
			);
		}

		e.Handled = true;
	}

	private void OnScoreChanged(object sender, ValueChangedEventArgs e)
	{
		scoreLabel.Text = $"คะแนน: {Math.Round(e.NewValue)}";
	}

	private async void OnConfirmClicked(object sender, EventArgs e)
	{
		int score = (int)Math.Round(scoreSlider.Value);
		string hexColor = $"#{selectedColor.Red:X2}{selectedColor.Green:X2}{selectedColor.Blue:X2}";

		Console.WriteLine($"✅ สีที่เลือก: {hexColor}, คะแนน: {score}");

		await Navigation.PushAsync(new DiaryPage(_user, hexColor, score));
	}
}