using EstragoniaTemplate.Main;
using EstragoniaTemplate.UI.ViewModels;
using Godot;
using System;
using System.Text.Json;
using EstragoniaTemplate.UI.Models;
using System.Diagnostics;
using EstragoniaTemplate.Resources;

namespace EstragoniaTemplate.Main;

public partial class MainScene : Node2D
{
    [Export]
    private UserInterface? UserInterfaceMain { get; set; }
    [Export]
    private UserInterface? UserInterfaceDialog { get; set; }

    public override void _Ready()
    {
        if (UserInterfaceMain == null || UserInterfaceDialog == null)
            throw new NullReferenceException();

        var options = GraphicsOptions.LoadOrCreateOptions();

        InputMapResource.LoadSavedInputMap();

        var keyRepeater = new KeyRepeater();
        GetWindow().FocusExited += keyRepeater.ClearRepeatingAndBlockedInput;

        var mainViewModelDialog = new MainViewModel(UserInterfaceDialog);
        var mainViewModel = new MainViewModel(UserInterfaceMain);
        var viewModelFactory = new ViewModelFactory(
            options, 
            mainViewModel, 
            mainViewModelDialog,
            UserInterfaceMain, 
            UserInterfaceDialog,
            keyRepeater);

        UserInterfaceDialog.Initialize(mainViewModelDialog, keyRepeater);
        UserInterfaceMain.Initialize(
            mainViewModel, 
            keyRepeater, 
            viewModelFactory.CreateMainMenu(GetTree()));

        UserInterfaceMain.GrabFocus();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey key && key.PhysicalKeycode == Key.Escape && key.Pressed && !key.Echo)
        {
            Debug.WriteLine("Unhandled escape");
            GetViewport().SetInputAsHandled();
        }
    }
}
