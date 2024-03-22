using System;
using System.Globalization;
using System.Windows.Data;

namespace LaboratoryPanelWPF.Converter;

public class PaginationCountConverter:IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // $"Page {currentPage} of {totalPages}";
        if (value is not int[] pagination)
        {
            return null;
        }
        
        return $"Page {pagination[0]} of {pagination[1]}";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
