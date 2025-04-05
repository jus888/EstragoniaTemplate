using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace EstragoniaTemplate.UI.Converters;

public class IntEqualConverter : IValueConverter
{
    public static readonly IntEqualConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null && parameter != null && int.TryParse(value.ToString(), out var num1) && int.TryParse(parameter.ToString(), out var num2))
            return num1 == num2;

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return new BindingNotification(new NotImplementedException(), BindingErrorType.Error);
    }
}