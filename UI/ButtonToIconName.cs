﻿using Godot;
using System.Collections.Generic;

namespace EstragoniaTemplate.UI;

internal static class ButtonToIconName
{
    public static bool TryGetXbox(JoyButton joyButton, out string? name)
        => XboxButtonNames.TryGetValue(joyButton, out name);

    private static Dictionary<JoyButton, string> XboxButtonNames { get; } = new()
    {
        {JoyButton.A, "xbox_button_a"},
        {JoyButton.B, "xbox_button_b"},
        {JoyButton.X, "xbox_button_x"},
        {JoyButton.Y, "xbox_button_y"},
        {JoyButton.DpadUp, "xbox_dpad_up_outline"},
        {JoyButton.DpadDown, "xbox_dpad_down_outline"},
        {JoyButton.DpadLeft, "xbox_dpad_left_outline"},
        {JoyButton.DpadRight, "xbox_dpad_right_outline"},
        {JoyButton.LeftShoulder, "xbox_lb"},
        {JoyButton.RightShoulder, "xbox_rb"}
    };

    public static bool TryGetKeyboard(Key key, out string? name)
    {
        name = key switch
        {
            (>= Key.Key0) and (<= Key.Key9) => ((int)key - 48).ToString(),

            ((>= Key.A) and (<= Key.Z))
                or Key.Shift or Key.Ctrl
                or Key.Enter or Key.Space
                or Key.Backspace
                or Key.Period or Key.Comma => key.ToString().ToLower(),

            _ => null
        };

        if (name == null)
            return false;

        name = "keyboard_" + name;
        return true;
    }
}
