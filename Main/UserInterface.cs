using Godot;
using System;
using EstragoniaTemplate.UI;
using EstragoniaTemplate.UI.Views;
using EstragoniaTemplate.UI.ViewModels;
using JLeb.Estragonia;
using System.Linq;
using System.Diagnostics;
using Avalonia.Controls;
using EstragoniaTemplate.UI.Models;
using System.Threading;

namespace EstragoniaTemplate.Main;

public partial class UserInterface : AvaloniaControl, IFocussable
{
    public event EventHandler<InputEvent>? InputEventReceived;

    public ViewModel? CurrentViewModel
    {
        get => _mainViewModel?.CurrentViewModel;
    }

    private MainViewModel? _mainViewModel;

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

    public void Initialize(MainViewModel mainViewModel, KeyRepeater keyRepeater, ViewModel? initialViewModel = null)
    {
        _mainViewModel = mainViewModel;
        _keyRepeater = keyRepeater;

        if (initialViewModel != null)
        {
            mainViewModel.NavigateTo(initialViewModel);
        }

        Control = new MainView()
        {
            DataContext = mainViewModel
        };
    }

    public new void GrabFocus()
    {
        FocusMode = FocusModeEnum.All;
        base.GrabFocus();
        CurrentViewModel?.OnUserInterfaceFocusReturned();
    }

    public new void ReleaseFocus()
    {
        FocusMode = FocusModeEnum.None;
        base.ReleaseFocus();
        CurrentViewModel?.OnUserInterfaceFocusLost();
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (_keyRepeater != null && _keyRepeater.Input(@event))
            return;

        InputEventReceived?.Invoke(this, @event);

        if (@event is InputEventKey key && key.PhysicalKeycode == Key.Space)
        {
            key.Keycode = Key.Enter;
            key.PhysicalKeycode = Key.Enter;
        }

        base._GuiInput(@event);
    }

    public void ForceGuiInput(InputEvent @event)
    {
        InputEventReceived?.Invoke(this, @event);

        base._GuiInput(@event);
    }

    public override void _Process(double delta)
    {
        _keyRepeater?.Process((float)delta, this);

        base._Process(delta);
    }
}
