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

    private string _inputMapAction;

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
}
