using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LaboratoryPanelWPF.Converter
{
    internal class TotalBackgroundColor : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var paymentStatus = (value as string)?.ToLower();

            return paymentStatus == "paid"
                ? new SolidColorBrush(Color.FromArgb(30, 52, 168, 83))
                : new SolidColorBrush(Color.FromArgb(30, 234, 67, 53));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}