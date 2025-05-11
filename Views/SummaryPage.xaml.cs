using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using Righthere_Demo.Popup;
using Righthere_Demo.Models;
using System;
using System.Threading.Tasks;

namespace Righthere_Demo.Views
{
    public partial class SummaryPage : ContentPage
    {
        private string mood;
        private string Sugges;
        private string keyword;
        private string Emotion;
        private string content;

        public SummaryPage(string mood, string Suggestion, string Keyword, string Emotion, string content)
        {
            InitializeComponent();

            this.mood = mood;
            this.Sugges = Suggestion;
            this.keyword = Keyword;
            this.Emotion = Emotion;
            this.content = content;

            // ✅ ใช้ OnAppearing() เพื่ออัปเดต UI
        }

        // ✅ ใช้ OnAppearing() เพื่ออัปเดต UI เมื่อหน้าพร้อม
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Moodtex != null)
                Moodtex.Text = this.mood;  // ✅ อัปเดตค่า Label
            Advicetext.Text = this.Sugges;
            Keywordtext.Text = this.keyword;
            Emotiontext.Text = this.Emotion;
        }
        private async void GoMainPage(object sender, EventArgs e)
        {
            var Pop = new Savediary();

            var result = await this.ShowPopupAsync(Pop) as bool?;

            if (result == true)
            {
                var currentUser = App.User;
                if (currentUser == null)
                {
                    await DisplayAlert("Error", "User session expired. Please log in again.", "OK");
                    App.Current.MainPage = new NavigationPage(new LoginPage());
                    return;
                }

                var diary = new DiaryData
                {
                    UserId = currentUser.Userid,
                    Content = content,  // ต้องแน่ใจว่า content, mood, Sugges, keyword และ Emotion ถูกกำหนดค่าก่อน
                    Mood = mood,
                    Suggestion = Sugges,
                    Keywords = keyword,
                    EmotionalReflection = Emotion,
                };

                await App.DiaryDB.SaveDiaryAsync(diary);
                await DisplayAlert("Saved", "Diary entry saved successfully.", "OK");
            }

            Application.Current.MainPage = new NavigationPage(new StarterPage(users: App.User));
        }


    }
}
