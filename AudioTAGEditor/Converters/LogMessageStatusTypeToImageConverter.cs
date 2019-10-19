using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AudioTAGEditor.Converters
{
    class LogMessageStatusTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var logMessageStatusType = value.ToString();
                switch (logMessageStatusType)
                {
                    case "Information":
                        return (BitmapImage)App.Current.TryFindResource("info");
                    case "Warning":
                        return (BitmapImage)App.Current.TryFindResource("warning");
                    case "Error":
                        return (BitmapImage)App.Current.TryFindResource("error");
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
