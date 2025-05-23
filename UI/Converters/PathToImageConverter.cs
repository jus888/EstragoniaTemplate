using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace EstragoniaTemplate.UI.Converters;

public class PathToImageConverter : IValueConverter
{
    public static readonly GodotWindowModeConverter Instance = new();

    public const string ImageFolderPath = "UI/Images";

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string path)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

        return Utilities.LoadImageFromResource(new Uri($"avares://EstragoniaTemplate/{ImageFolderPath}/{path}.png"));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}