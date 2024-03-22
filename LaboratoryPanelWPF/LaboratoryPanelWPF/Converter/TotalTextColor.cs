using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LaboratoryPanelWPF.Converter
{
    public class TotalTextColor : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var paymentStatus = (value as string)?.ToLower();

            return paymentStatus == "paid"
                ? new SolidColorBrush(Color.FromRgb(52, 168, 83))
                : new SolidColorBrush(Color.FromRgb(234, 67, 53));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}