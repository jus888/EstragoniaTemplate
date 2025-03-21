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

    public void StealFocus(UserInterface from, bool returnWhenCurrentViewModelIsNull)
    {
        from.FocusMode = FocusModeEnum.None;
        this.FocusMode = FocusModeEnum.All;
        GrabFocus();
        from.CurrentViewModel?.OnUserInterfaceFocusLost();

        if (returnWhenCurrentViewModelIsNull && _mainViewModel != null)
        {
            _mainViewModel.Navigated += OnNavigated;
            
            void OnNavigated(object? sender, ViewModel? viewModel)
            {
                if (viewModel == null)
                {
                    _mainViewModel.Navigated -= OnNavigated;
                    from.StealFocus(this, false);
                    from.CurrentViewModel?.OnUserInterfaceFocusReturned();
                }
            }
        }
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventKey key && key.PhysicalKeycode == Key.Space)
        {
            key.Keycode = Key.Enter;
            key.PhysicalKeycode = Key.Enter;
        }

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
