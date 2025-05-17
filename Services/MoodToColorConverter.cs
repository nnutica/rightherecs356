using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics; // ใช้อันนี้แทน SkiaSharp

namespace Righthere_Demo.Services
{
    public class MoodToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string mood = value?.ToString()?.ToLower() ?? "neutral";

            return mood switch
            {
                "joy" => Colors.Gold,
                "sadness" => Colors.DarkBlue,
                "anger" => Colors.Red,
                "surprise" => Colors.Gray,
                "fear" => Colors.Purple,
                "love" => Colors.Pink,
                _ => Colors.LightGray, // Default color for unknown moods
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
