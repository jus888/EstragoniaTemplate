using EstragoniaTemplate.Main;
using EstragoniaTemplate.UI.ViewModels;
using Godot;
using System;
using System.Text.Json;
using EstragoniaTemplate.UI.Models;

namespace EstragoniaTemplate.Main;

public partial class MainScene : Node2D
{
    [Export]
    private UserInterface? UserInterface { get; set; }
    [Export]
    private UserInterface? UserInterfaceDialog { get; set; }

    public override void _Ready()
    {
        if (UserInterface == null || UserInterfaceDialog == null)
            throw new NullReferenceException();

        var options = UIOptions.LoadOrCreateOptions();

        var mainViewModelDialog = new MainViewModel(UserInterfaceDialog);
        var mainViewModel = new MainViewModel(UserInterface);
        var viewModelFactory = new ViewModelFactory(
            options, 
            mainViewModel, 
            UserInterface, 
            UserInterfaceDialog);

        var keyRepeater = new KeyRepeater();
        GetWindow().FocusExited += keyRepeater.ClearRepeatingAndBlockedInput;

        UserInterfaceDialog.Initialize(mainViewModelDialog, keyRepeater);
        UserInterface.Initialize(
            mainViewModel, 
            keyRepeater, 
            viewModelFactory.CreateMainMenu(GetTree()));

        UserInterface.GrabFocus();
    }
}
