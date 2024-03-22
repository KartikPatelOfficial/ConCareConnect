using System;
using System.Globalization;
using System.Windows.Data;

namespace LaboratoryPanelWPF.Converter
{
    internal class StringToJsonConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string json)
            {
                return null;
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json) ?? "";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is not string json ? null : Newtonsoft.Json.JsonConvert.SerializeObject(json);
        }
    }
}
