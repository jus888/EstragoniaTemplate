using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Animation;
using EstragoniaTemplate.UI.Controls;
using System;
using static EstragoniaTemplate.UI.Utilities;
using Godot;
using EstragoniaTemplate.Main;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class InputListenerDialogViewModel : ViewModel
{
    public event Action<(Key?, JoyButton?)>? InputPressed;

    public InputListenerDialogViewModel(UserInterface userInterface)
    {
        userInterface.InputEventReceived += OnInputEvent;
    }

    private void OnInputEvent(InputEvent inputEvent)
    {
        (Key?, JoyButton?)? inputTuple = null;
        if (inputEvent is InputEventKey keyEvent && keyEvent.Pressed && ButtonToIconName.TryGetKeyboard(keyEvent.Keycode, out _))
        {
            // UserInterface will process the inputEvent after this method: set pressed to false to prevent instant press after this dialog is closed.
            //keyEvent.Pressed = false;
            inputTuple = (keyEvent.Keycode, null);
        }
        else if (inputEvent is InputEventJoypadButton joypadEvent && joypadEvent.Pressed && ButtonToIconName.TryGetXbox(joypadEvent.ButtonIndex, out _))
        {
            //joypadEvent.Pressed = false;
            inputTuple = (null, joypadEvent.ButtonIndex);
        }

        if (inputTuple != null)
        {
            InputPressed?.Invoke(inputTuple.Value);
            Close();
        }
    }

    [RelayCommand]
    public void Cancel()
    {
        InputPressed?.Invoke((null, null));
        Close();
    }
}
