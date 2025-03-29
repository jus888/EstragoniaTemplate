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
    public HashSet<Key> ReservedKeys => _inputMapGroup.ReservedKeys;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(KeyName))]
    private int? _keyEnumValue = (int)Key.Right;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(JoyButtonName))]
    private int? _controllerEnumValue = (int)JoyButton.A;

    private InputEventKey? _inputMapKeyEvent;
    private InputEventJoypadButton? _inputMapJoypadEvent;

    private StringName _inputMapAction;
    private readonly InputMapGroup _inputMapGroup;

    public InputMapItem(string inputMapAction, string inputName, InputMapGroup inputMapGroup)
    {
        _inputMapAction = inputMapAction;
        InputName = inputName;

        _inputMapGroup = inputMapGroup;

        KeyEnumValue = null;
        ControllerEnumValue = null;

        var inputEvents = InputMap.ActionGetEvents(_inputMapAction);

        foreach (var inputEvent in inputEvents)
        {
            if (inputEvent is InputEventKey inputKey)
            {
                _inputMapKeyEvent = inputKey;
                KeyEnumValue = (int)inputKey.PhysicalKeycode;
                inputMapGroup.KeyMappings.Add(inputKey.PhysicalKeycode, this);
            }
            else if (inputEvent is InputEventJoypadButton inputJoypad)
            {
                _inputMapJoypadEvent = inputJoypad;
                ControllerEnumValue = (int)inputJoypad.ButtonIndex;
                inputMapGroup.JoypadMappings.Add(inputJoypad.ButtonIndex, this);
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

    public void SetKeyboardKey(Key? newKey)
    {
        Key? oldKey = KeyEnumValue == null ? null : (Key)KeyEnumValue;

        if (oldKey != null)
        {
            _inputMapGroup.KeyMappings.Remove(oldKey.Value);
            InputMap.ActionEraseEvent(_inputMapAction, _inputMapKeyEvent);
        }

        if (newKey == null)
        {
            KeyEnumValue = null;
            return;
        }

        if (_inputMapGroup.KeyMappings.TryGetValue(newKey.Value, out var existingInputMapItem))
        {
            existingInputMapItem.SetKeyboardKey(oldKey);
        }

        var inputKey = new InputEventKey()
        {
            PhysicalKeycode = newKey.Value,
            Keycode = newKey.Value
        };

        InputMap.ActionAddEvent(_inputMapAction, inputKey);
        _inputMapGroup.KeyMappings.Add(newKey.Value, this);
        _inputMapKeyEvent = inputKey;
        KeyEnumValue = (int)newKey;
    }

    public void SetJoypadButton(JoyButton? newButton)
    {
        JoyButton? oldButton = ControllerEnumValue == null ? null : (JoyButton)ControllerEnumValue;

        if (oldButton != null)
        {
            _inputMapGroup.JoypadMappings.Remove(oldButton.Value);
            InputMap.ActionEraseEvent(_inputMapAction, _inputMapJoypadEvent);
        }

        if (newButton == null)
        {
            ControllerEnumValue = null;
            return;
        }

        if (_inputMapGroup.JoypadMappings.TryGetValue(newButton.Value, out var existingInputMapItem))
        {
            existingInputMapItem.SetJoypadButton(oldButton);
        }

        var inputJoypad = new InputEventJoypadButton()
        {
            ButtonIndex = newButton.Value
        };

        InputMap.ActionAddEvent(_inputMapAction, inputJoypad);
        _inputMapGroup.JoypadMappings.Add(newButton.Value, this);
        _inputMapJoypadEvent = inputJoypad;
        ControllerEnumValue = (int)newButton;
    }
}
