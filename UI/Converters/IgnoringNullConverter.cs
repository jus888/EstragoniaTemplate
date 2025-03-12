using Avalonia.Data.Converters;
using Avalonia.Data;
using Godot;
using System;
using System.Globalization;

namespace EstragoniaTemplate.UI.Converters;

public class IgnoringNullConverter : IValueConverter
{
    public static readonly IgnoringNullConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return BindingOperations.DoNothing;

        return value;
    }
}