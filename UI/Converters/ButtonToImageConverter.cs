using Avalonia.Data.Converters;
using Avalonia.Data;
using Godot;
using System;
using System.Globalization;
using System.Diagnostics;
using Godot.Collections;

namespace EstragoniaTemplate.UI.Converters;

public class ButtonToImageConverter : IValueConverter
{
    public static readonly GodotWindowModeConverter Instance = new();

    public const string ImageFolderPath = "UI/Images";

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int valueInt || parameter is not string type)
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

        string? buttonName = null;
        string subFolder = "";
        if (type == "keyboard")
        {
            subFolder = "Keyboard";
            var key = (Key)valueInt;
            ButtonToIconName.TryGetKeyboard(key, out buttonName);
        }
        else if (type == "xbox")
        {
            subFolder = "Controller";
            var joyButton = (JoyButton)valueInt;
            ButtonToIconName.TryGetXbox(joyButton, out buttonName);
        }

        if (buttonName != null)
        {
            return Utilities.LoadImageFromResource(new Uri($"avares://EstragoniaTemplate/{ImageFolderPath}/{subFolder}/{buttonName}.png"));
        }

        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}