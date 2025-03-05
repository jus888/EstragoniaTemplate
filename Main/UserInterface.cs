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

namespace EstragoniaTemplate.Main;

public partial class UserInterface : AvaloniaControl
{
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

    public void Initialize(MainViewModel mainViewModel, ViewModel? initialViewModel = null)
    {
        _keyRepeater = new();
        GetWindow().FocusExited += _keyRepeater.ClearRepeatingAndBlockedInput;
        mainViewModel.UIOptions.Applied += (s, e) => _keyRepeater.UpdateDirectionalKeys();
        mainViewModel.Navigated += (s, e) => _keyRepeater.ClearRepeatingAndBlockedInput();

        if (initialViewModel != null)
        {
            mainViewModel.NavigateTo(initialViewModel);
        }

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
