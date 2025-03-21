using Avalonia.Data.Converters;
using Avalonia.Data;
using Godot;
using System;
using System.Globalization;
using System.Diagnostics;
using Godot.Collections;

namespace EstragoniaTemplate.UI.Converters;

public class JoyButtonToImageSourceConverter : IValueConverter
{
    public const string ImageFilePath = "UI/Images/Controller";
    public static readonly GodotWindowModeConverter Instance = new();

    public static Dictionary<JoyButton, string> XboxButtonNames { get; } = new()
    {
        {JoyButton.A, "xbox_button_a"},
        {JoyButton.B, "xbox_button_b"},
        {JoyButton.X, "xbox_button_x"},
        {JoyButton.Y, "xbox_button_y"},
        {JoyButton.DpadUp, "xbox_dpad_up"},
        {JoyButton.DpadDown, "xbox_dpad_down"},
        {JoyButton.DpadLeft, "xbox_dpad_left"},
        {JoyButton.DpadRight, "xbox_dpad_right"},
        {JoyButton.LeftShoulder, "xbox_lb"},
        {JoyButton.RightShoulder, "xbox_rb"}
    };

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int joyButtonInt && parameter is string controllerType)
        {
            var joyButton = (JoyButton)joyButtonInt;
            string? buttonName = null;
            if (controllerType == "xbox" && XboxButtonNames.TryGetValue(joyButton, out var xboxButtonName))
            {
                buttonName = xboxButtonName;
            }

            if (buttonName == null)
                return "";

            return Utilities.LoadImageFromResource(new Uri($"avares://EstragoniaTemplate/{ImageFilePath}/{buttonName}.png"));
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}