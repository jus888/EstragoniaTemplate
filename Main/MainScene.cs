using EstragoniaTemplate.Resources;
using EstragoniaTemplate.UI.Models;
using EstragoniaTemplate.UI.ViewModels;
using Godot;
using System;
using System.Diagnostics;

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

        InputMapResource.LoadSavedInputMap();
        var options = Options.LoadOrCreate();

        var keyRepeater = new KeyRepeater();
        GetWindow().FocusExited += keyRepeater.ClearRepeatingAndBlockedInput;

        var focusStack = new FocusStack();

        var mainViewModelDialog = new MainViewModel(UserInterfaceDialog);
        var mainViewModel = new MainViewModel(UserInterfaceMain);
        var viewModelFactory = new ViewModelFactory(
            options,
            mainViewModel,
            mainViewModelDialog,
            UserInterfaceMain,
            UserInterfaceDialog,
            keyRepeater,
            focusStack);

        UserInterfaceDialog.Initialize(mainViewModelDialog, keyRepeater);
        UserInterfaceMain.Initialize(
            mainViewModel,
            keyRepeater,
            viewModelFactory.CreateMainMenu(GetTree()));

        focusStack.Push(UserInterfaceMain);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey key && key.PhysicalKeycode == Key.Escape && key.Pressed && !key.Echo)
        {
            Debug.WriteLine("Escape");
            GetViewport().SetInputAsHandled();
        }
    }
}
