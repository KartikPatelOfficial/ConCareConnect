using DiagnosticServicesModel.DataClass;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace LaboratoryPanelWPF.Converter
{
    public class TestsToStringConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not List<Test> tests || tests.Count < 1)
                return "No tests found.";

            var sb = new StringBuilder();
            foreach (var test in tests)
            {
                sb.Append(test.Name);
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string tests)
                return null;

            var testsArray = tests.Split(',');
            return testsArray.Select(test => new Test { Name = test.Trim() }).ToList();
        }
    }
}
