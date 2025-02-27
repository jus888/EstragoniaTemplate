using Godot;
using System;
using EstragoniaTemplate.UI;
using EstragoniaTemplate.UI.Views;
using EstragoniaTemplate.UI.ViewModels;
using JLeb.Estragonia;
using System.Linq;
using System.Diagnostics;

namespace EstragoniaTemplate.Main;

public partial class UserInterface : AvaloniaControl
{
    /// <summary>
    /// Prevents auto key repeat when holding accept button(s).
    /// </summary>
    private bool _stopAcceptRepeat = false;
    private StringName _uiAcceptName = "ui_accept";

    private KeyRepeater? _keyRepeater;

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
        _keyRepeater = new KeyRepeater(mainViewModel.UIOptions, GetWindow());

        var mainMenuViewModel = new MainMenuViewModel(mainViewModel);
        mainViewModel.NavigateTo(mainMenuViewModel);

        Control = new MainView()
        {
            DataContext = mainViewModel
        };
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (_keyRepeater != null && _keyRepeater.Input(@event))
            return;

        base._GuiInput(@event);
    }

    public void ForceGuiInput(InputEvent @event)
    {
        base._GuiInput(@event);
    }

    public override void _Process(double delta)
    {
        _keyRepeater?.Process((float)delta, this);

        base._Process(delta);
    }
}
