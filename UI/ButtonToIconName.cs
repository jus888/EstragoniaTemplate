using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        {JoyButton.DpadUp, "xbox_dpad_up"},
        {JoyButton.DpadDown, "xbox_dpad_down"},
        {JoyButton.DpadLeft, "xbox_dpad_left"},
        {JoyButton.DpadRight, "xbox_dpad_right"},
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
