using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using Righthere_Demo.Services;
using Righthere_Demo.Models;
using Righthere_Demo.Data;

namespace Righthere_Demo.Views
{
    public partial class DiaryPage : ContentPage
    {
        private string selectedReason = "friend";
        private readonly Users _user;
        private readonly string _colorHex;
        private readonly int _sentimentScore;

        public DiaryPage(Users users, string colorHex = "#FFFFFF", int sentimentScore = 0)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _user = users;
            _colorHex = colorHex;
            _sentimentScore = sentimentScore;

            this.BackgroundColor = Color.FromArgb(_colorHex);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("📍 DiaryPage Appeared");
            Console.WriteLine($"🎨 Color: {_colorHex}, Score: {_sentimentScore}");
            if (App.User == null)
            {
                DisplayAlert("Error", "User not logged in. Redirecting to login...", "OK");
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }

        private void OnReasonButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                selectedReason = button.Text;
                UpdateReasonButtonStyles();
            }
        }
        private void UpdateReasonButtonStyles()
        {
            FriendButton.BackgroundColor = selectedReason == "friend" ? Colors.DarkGreen : Colors.LightGray;
            FriendButton.TextColor = selectedReason == "friend" ? Colors.White : Colors.DarkGray;

            WorkButton.BackgroundColor = selectedReason == "work" ? Colors.DarkGreen : Colors.LightGray;
            WorkButton.TextColor = selectedReason == "work" ? Colors.White : Colors.DarkGray;

            FamilyButton.BackgroundColor = selectedReason == "family" ? Colors.DarkGreen : Colors.LightGray;
            FamilyButton.TextColor = selectedReason == "family" ? Colors.White : Colors.DarkGray;

            SchoolButton.BackgroundColor = selectedReason == "school" ? Colors.DarkGreen : Colors.LightGray;
            SchoolButton.TextColor = selectedReason == "school" ? Colors.White : Colors.DarkGray;
        }



        private async void OnAnalyzeClicked(object sender, EventArgs e)
        {
            string content = DiaryEntry.Text;
            if (string.IsNullOrWhiteSpace(content))
            {
                await DisplayAlert("Error", "Please write something before analyzing.", "OK");
                return;
            }

            AnalyzeButton.IsEnabled = false;
            AnalyzeButton.Text = "Analyzing...";
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var api = new Services.API();
            await api.SendData(content);

            string mood = api.GetMood();
            string suggestion = api.GetSuggestion();
            string keyword = api.GetKeywords();
            string emotion = api.GetEmotionalReflection();
            double score = _sentimentScore;
            string reason = selectedReason;

            if (string.IsNullOrWhiteSpace(mood) || string.IsNullOrWhiteSpace(suggestion) || string.IsNullOrWhiteSpace(keyword))
            {
                await DisplayAlert("Error", "Failed to analyze content. Please try again.", "OK");
                AnalyzeButton.IsEnabled = true;
                AnalyzeButton.Text = "Next ➤";
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                return;
            }

            // 👤 สมมุติว่าคุณมี CurrentUser จาก Login
            var currentUser = App.User;
            if (currentUser == null)
            {
                await DisplayAlert("Error", "User session expired. Please log in again.", "OK");
                App.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            AnalyzeButton.IsEnabled = true;
            AnalyzeButton.Text = "Next ➤";
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;

            await Navigation.PushAsync(new SummaryPage(mood, suggestion, keyword, emotion, content, score, reason));
        }
    }
}
