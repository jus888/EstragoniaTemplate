using EstragoniaTemplate.UI.Models;
using EstragoniaTemplate.UI.ViewModels;
using Godot;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace EstragoniaTemplate.Main;

public partial class MainScene : Node2D
{
    [Export]
    private UserInterface? UserInterfaceMain { get; set; }
    [Export]
    private UserInterface? UserInterfaceDialog { get; set; }

    private ViewModelFactory _viewModelFactory;

    public override void _Ready()
    {
        MusicManager.Instance?.PlayMusic(this, MusicManager.Music.MainMenu);

        if (UserInterfaceMain == null || UserInterfaceDialog == null)
            throw new NullReferenceException();

        SerializableInputMap.LoadAndApplyInputMap();
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
            focusStack,
            GetTree());
        _viewModelFactory = viewModelFactory;

        UserInterfaceDialog.Initialize(mainViewModelDialog, keyRepeater);
        UserInterfaceMain.Initialize(
            mainViewModel,
            keyRepeater,
            viewModelFactory.CreateMainMenu());

        focusStack.Push(UserInterfaceMain);
    }

    public override void _Input(InputEvent @event)
    {
        using (@event)
        {
            if (@event is InputEventKey key && key.PhysicalKeycode == Key.Escape && key.Pressed && !key.Echo)
            {
                if (UserInterfaceMain != null &&
                    UserInterfaceMain is not
                    {
                        CurrentViewModel: MainMenuViewModel
                        or EscapeMenuViewModel
                        or OptionsViewModel
                        or IOptionsTabViewModel
                    })
                {
                    UserInterfaceMain.MainViewModel?.NavigateTo(_viewModelFactory.CreateEscapeMenu());
                    GetViewport().SetInputAsHandled();
                }
            }
        }
    }
}
