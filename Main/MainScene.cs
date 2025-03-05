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
        UIOptions options;
        if (FileAccess.FileExists("user://settings.json"))
        {
            using var file = FileAccess.Open("user://settings.json", FileAccess.ModeFlags.Read);
            options = JsonSerializer.Deserialize<UIOptions>(file.GetAsText()) ?? new();
        }
        else
        {
            options = new();
            options.SaveOverrideFile();

            using var file = FileAccess.Open("user://settings.json", FileAccess.ModeFlags.Write);
            file.StoreString(JsonSerializer.Serialize(options));
        }
        options.Apply();

        var mainViewModelDialog = new MainViewModel(UserInterfaceDialog!, options);
        var mainViewModel = new MainViewModel(UserInterface!, options, mainViewModelDialog);

        UserInterface!.Initialize(mainViewModel, new MainMenuViewModel(mainViewModel));
        UserInterfaceDialog!.Initialize(mainViewModelDialog);

        UserInterface.GrabFocus();
    }
}
