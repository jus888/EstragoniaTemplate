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
    public MainViewModel? MainViewModel { get; private set; }

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
        MainViewModel = mainViewModel;
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

        if (returnWhenCurrentViewModelIsNull && MainViewModel != null)
        {
            MainViewModel.Navigated += OnNavigated;
            
            void OnNavigated(object? sender, ViewModel? viewModel)
            {
                if (viewModel == null)
                {
                    MainViewModel.Navigated -= OnNavigated;
                    from.StealFocus(this, false);
                    from.MainViewModel?.CurrentViewModel?.OnNavigatorReturnedFocus(true);
                }
            }
        }
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
