using CommunityToolkit.Mvvm.Input;
using EstragoniaTemplate.Main;
using Godot;
using System;
using System.Collections.Generic;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class InputListenerDialogViewModel : ViewModel
{
    public event Action<(Key?, JoyButton?)>? InputPressed;

    public bool ListenToKeyboard { get; } = true;
    public string InputName { get; } = "Input Name";

    private UserInterface _userInterface;
    private EventHandler<InputEvent>? _inputEventHandler;
    private HashSet<Key> _reservedKeys;

    /// <summary>
    /// Intended for designer usage only.
    /// </summary>
    public InputListenerDialogViewModel() { }
    public InputListenerDialogViewModel(UserInterface userInterface, HashSet<Key> reservedKeys, bool listenToKeyboard, string inputName)
    {
        _reservedKeys = reservedKeys;
        ListenToKeyboard = listenToKeyboard;
        InputName = inputName;

        _userInterface = userInterface;
        _inputEventHandler = null;
        _inputEventHandler = (sender, inputEvent) =>
        {
            if (OnInputEvent(sender, inputEvent))
            {
                userInterface.InputEventReceived -= _inputEventHandler;
            }
        };

        userInterface.InputEventReceived += _inputEventHandler;
    }

    /// <summary>
    /// Returns true if the input was valid.
    /// </summary>
    private bool OnInputEvent(object? sender, InputEvent inputEvent)
    {
        var userInterface = (UserInterface)sender!;

        (Key?, JoyButton?)? inputTuple = null;
        if (ListenToKeyboard && inputEvent is InputEventKey keyEvent && keyEvent.Pressed
            && ButtonToIconName.TryGetKeyboard(keyEvent.Keycode, out var name)
            && !_reservedKeys.Contains(keyEvent.PhysicalKeycode))
        {
            // UserInterface will process the inputEvent after this method:
            // set pressed to false to prevent instant press after this dialog is closed.
            keyEvent.Pressed = false;
            inputTuple = (keyEvent.PhysicalKeycode, null);
        }
        else if (!ListenToKeyboard && inputEvent is InputEventJoypadButton joypadEvent && joypadEvent.Pressed
            && ButtonToIconName.TryGetXbox(joypadEvent.ButtonIndex, out _))
        {
            joypadEvent.Pressed = false;
            inputTuple = (null, joypadEvent.ButtonIndex);
        }

        if (inputTuple != null)
        {
            InputPressed?.Invoke(inputTuple.Value);
            Close();
            return true;
        }

        return false;
    }

    [RelayCommand]
    public void Cancel()
    {
        _userInterface.InputEventReceived -= _inputEventHandler;
        InputPressed?.Invoke((null, null));
        Close();
    }
}
