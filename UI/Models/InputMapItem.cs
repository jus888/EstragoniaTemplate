using CommunityToolkit.Mvvm.ComponentModel;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstragoniaTemplate.UI.Models;

public partial class InputMapItem : ObservableObject
{
    public string InputName { get; private init; } = "";
    public string KeyName => KeyEnumValue == null ? "" : ((Key)KeyEnumValue).ToString();
    public string JoyButtonName => ControllerEnumValue == null ? "" : ((JoyButton)ControllerEnumValue).ToString();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(KeyName))]
    private int? _keyEnumValue = (int)Key.Right;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(JoyButtonName))]
    private int? _controllerEnumValue = (int)JoyButton.A;

    private InputEventKey? _inputMapKeyEvent;
    private InputEventJoypadButton? _inputMapJoypadEvent;

    private StringName _inputMapAction;

    public InputMapItem(string inputMapAction, string inputName)
    {
        _inputMapAction = inputMapAction;
        InputName = inputName;

        KeyEnumValue = null;
        ControllerEnumValue = null;

        var inputEvents = InputMap.ActionGetEvents(_inputMapAction);

        foreach (var inputEvent in inputEvents)
        {
            if (inputEvent is InputEventKey inputKey)
            {
                _inputMapKeyEvent = inputKey;
                KeyEnumValue = (int)inputKey.PhysicalKeycode;
            }
            else if (inputEvent is InputEventJoypadButton inputJoypad)
            {
                _inputMapJoypadEvent = inputJoypad;
                ControllerEnumValue = (int)inputJoypad.ButtonIndex;
            }
        }
    }

    public InputMapItem(string inputMapAction, string inputName, Key? key = null, JoyButton? joyButton = null)
    {
        _inputMapAction = inputMapAction;
        InputName = inputName;

        KeyEnumValue = null;
        ControllerEnumValue = null;

        if (key != null)
        {
            KeyEnumValue = (int)key;
        }
        if (joyButton != null)
        {
            ControllerEnumValue = (int)joyButton;
        }
    }

    public void SetKeyboardKey(Key key)
    {
        InputMap.ActionEraseEvent(_inputMapAction, _inputMapKeyEvent);

        var inputKey = new InputEventKey()
        {
            PhysicalKeycode = key,
            Keycode = key
        };

        InputMap.ActionAddEvent(_inputMapAction, inputKey);
        _inputMapKeyEvent = inputKey;
        KeyEnumValue = (int)key;
    }

    public void SetJoypadButton(JoyButton button)
    {
        InputMap.ActionEraseEvent(_inputMapAction, _inputMapJoypadEvent);

        var inputButton = new InputEventJoypadButton()
        {
            ButtonIndex = button
        };

        InputMap.ActionAddEvent(_inputMapAction, inputButton);
        _inputMapJoypadEvent = inputButton;
        ControllerEnumValue = (int)button;
    }
}
