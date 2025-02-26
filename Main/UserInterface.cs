using Godot;
using System;
using EstragoniaTemplate.UI;
using EstragoniaTemplate.UI.Views;
using EstragoniaTemplate.UI.ViewModels;
using JLeb.Estragonia;
using System.Linq;

namespace EstragoniaTemplate.Main;

public partial class UserInterface : AvaloniaControl
{
    /// <summary>
    /// Prevents auto key repeat when holding accept button(s).
    /// </summary>
    private bool _stopAcceptRepeat = false;
    private StringName _uiAcceptName = "ui_accept";

    public override void _Ready()
    {   
        RenderScaling = AvaloniaLoader.Instance.UIScaling;
        AvaloniaLoader.Instance.UIScaleChanged += (_, scale) => 
        {
            RenderScaling = scale;
        };

        base._Ready();
    }

    public void Initialize(MainViewModel mainViewModel)
    {
        var mainMenuViewModel = new MainMenuViewModel(mainViewModel);
        mainViewModel.NavigateTo(mainMenuViewModel);

        Control = new MainView()
        {
            DataContext = mainViewModel
        };
    }

    //public override void _GuiInput(InputEvent @event)
    //{
    //    var inputEventKey = @event as InputEventKey;
    //    var joypadButton = @event as InputEventJoypadButton;
    //    if (inputEventKey != null || joypadButton != null)
    //    {
    //        bool isAcceptInput = false;
    //        bool pressed = false;
    //        var mapAcceptEvents = InputMap.ActionGetEvents(_uiAcceptName);
    //        foreach (var inputEvent in mapAcceptEvents)
    //        {
    //            if (inputEvent is InputEventKey key && key.PhysicalKeycode == inputEventKey?.PhysicalKeycode)
    //            {
    //                isAcceptInput = true;
    //                pressed = inputEventKey.Pressed;
    //                break;
    //            }

    //            if (inputEvent is InputEventJoypadButton joypad && joypad.ButtonIndex == joypadButton?.ButtonIndex)
    //            {
    //                isAcceptInput = true;
    //                pressed = joypadButton.Pressed;
    //                break;
    //            }
    //        }

    //        if (isAcceptInput)
    //        {
    //            if (_stopAcceptRepeat == false)
    //            {
    //                base._GuiInput(@event);
    //            }

    //            _stopAcceptRepeat = false;
    //            if (pressed)
    //            {
    //                _stopAcceptRepeat = true;
    //            }

    //            return;
    //        }
    //    }

    //    base._GuiInput(@event);
    //}
}
