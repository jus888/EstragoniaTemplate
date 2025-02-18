using Godot;
using System;
using EstragoniaTemplate.UI;
using EstragoniaTemplate.UI.Views;
using EstragoniaTemplate.UI.ViewModels;
using JLeb.Estragonia;

namespace EstragoniaTemplate;

public partial class UserInterface : AvaloniaControl
{
    public override void _Ready()
    {
        RenderScaling = AvaloniaLoader.Instance.UIScaling;
        AvaloniaLoader.Instance.UIScaleChanged += (_, scale) => 
        { 
            RenderScaling = scale; 
        };

        var mainViewModel = new MainViewModel(new MainMenuViewModel());

        Control = new MainView()
        {
            DataContext = mainViewModel
        };

        base._Ready();
    }
}
